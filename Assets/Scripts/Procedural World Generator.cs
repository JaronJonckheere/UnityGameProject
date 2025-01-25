using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkTerrainGenerator : MonoBehaviour
{
    private enum TileType
    {
        None,   // No tile
        Grass,  // Grass tile
        Dirt    // Dirt tile
    }

    public Tilemap terrainTilemap;
    public Tilemap featureTilemap;

    public TileBase[] grassTiles;  // Grass tiles
    public TileBase[] dirtTiles;   // Dirt tiles (plains_0 to plains_21)
    public TileBase[] wallTiles;   // Wall tiles
    public TileBase[] resourceTiles; // Resource tiles

    public int chunkSize = 16;
    public float noiseScale = 0.1f;
    public Transform player;

    private Dictionary<Vector2Int, bool> generatedChunks = new Dictionary<Vector2Int, bool>();

    void Start()
    {
        if (player != null)
        {
            GenerateChunksAroundPlayer();
        }
        else
        {
            Debug.LogError("Player Transform is not assigned!");
        }
    }

    void Update()
    {
        GenerateChunksAroundPlayer();
    }

    void GenerateChunksAroundPlayer()
    {
        if (player == null) return;

        Vector2Int playerChunkPos = new Vector2Int(
            Mathf.FloorToInt(player.position.x / chunkSize),
            Mathf.FloorToInt(player.position.y / chunkSize)
        );

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int chunkPos = new Vector2Int(playerChunkPos.x + x, playerChunkPos.y + y);

                if (!generatedChunks.ContainsKey(chunkPos))
                {
                    LoadChunk(chunkPos);
                    generatedChunks[chunkPos] = true;
                }
            }
        }
    }

    void LoadChunk(Vector2Int chunkPos)
    {
        TileBase[,] terrainChunk = GenerateTerrainChunk(chunkPos);
        PlaceChunkOnTilemap(chunkPos, terrainChunk, terrainTilemap);

        TileBase[,] featureChunk = GenerateFeatureChunk(chunkPos, terrainChunk);
        PlaceChunkOnTilemap(chunkPos, featureChunk, featureTilemap);
    }

    TileBase[,] GenerateTerrainChunk(Vector2Int chunkPos)
    {
        TileBase[,] chunk = new TileBase[chunkSize, chunkSize];

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                int worldX = chunkPos.x * chunkSize + x;
                int worldY = chunkPos.y * chunkSize + y;

                float perlinValue = Mathf.PerlinNoise(worldX * noiseScale, worldY * noiseScale);

                if (perlinValue < 0.3f)
                {
                    chunk[x, y] = dirtTiles[0];
                }
                else
                {
                    chunk[x, y] = ChooseGrassTile(chunk, x, y);
                }
            }
        }

        return ApplyDirtTileLogic(chunk);
    }

    TileBase ChooseGrassTile(TileBase[,] chunk, int x, int y)
    {
        // Default to element 0 (grass_0) with higher probability
        return Random.value < 0.7f ? grassTiles[0] : grassTiles[Random.Range(1, grassTiles.Length)];
    }

    TileBase[,] ApplyDirtTileLogic(TileBase[,] chunk)
    {
        for (int x = 1; x < chunkSize - 1; x++)
        {
            for (int y = 1; y < chunkSize - 1; y++)
            {
                if (chunk[x, y] == dirtTiles[0])
                {
                    TileType[] neighbors = GetTileTypeNeighbors(chunk, x, y);

                    if (neighbors[0] == TileType.Dirt && neighbors[1] == TileType.Dirt &&
                        neighbors[2] == TileType.Dirt && neighbors[3] == TileType.Dirt)
                    {
                        // Check diagonals when all 4 main neighbors are dirt
                        TileType[] diagonals = GetDiagonalNeighbors(chunk, x, y);

                        if (diagonals[3] == TileType.Grass) chunk[x, y] = dirtTiles[16]; // Bottom-right grass
                        else if (diagonals[2] == TileType.Grass) chunk[x, y] = dirtTiles[17]; // Bottom-left grass
                        else if (diagonals[1] == TileType.Grass) chunk[x, y] = dirtTiles[18]; // Top-right grass
                        else if (diagonals[0] == TileType.Grass) chunk[x, y] = dirtTiles[19]; // Top-left grass
                        else if (diagonals[0] == TileType.Grass && diagonals[3] == TileType.Grass)
                            chunk[x, y] = dirtTiles[20]; // Top-left and Bottom-right grass
                        else if (diagonals[1] == TileType.Grass && diagonals[2] == TileType.Grass)
                            chunk[x, y] = dirtTiles[21]; // Top-right and Bottom-left grass
                        else
                            chunk[x, y] = dirtTiles[6]; // Default to element 8
                    }
                    else
                    {
                        // Use basic rules when not all neighbors are dirt
                        for (int i = 0; i < 16; i++)
                        {
                            if (MatchDirtTileRule(i, neighbors))
                            {
                                chunk[x, y] = dirtTiles[i];
                                break;
                            }
                        }
                    }
                }
            }
        }
        return chunk;
    }

    bool MatchDirtTileRule(int dirtIndex, TileType[] neighbors)
    {
        bool[][] dirtRules = new bool[][]
        {
            new bool[] { true, true, true, false },  // plains_0
            new bool[] { true, true, false, false }, // plains_1
            new bool[] { true, false, false, false }, // plains_2
            new bool[] { true, false, true, false },  // plains_3
            new bool[] { false, true, true, false },  // plains_6
            new bool[] { false, true, false, false }, // plains_7
            new bool[] { false, false, false, false }, // plains_8
            new bool[] { false, false, true, false },  // plains_9
            new bool[] { false, true, true, true },    // plains_12
            new bool[] { false, true, false, true },   // plains_13
            new bool[] { false, false, false, true },  // plains_14
            new bool[] { false, false, true, true },   // plains_15
            new bool[] { true, true, true, true },     // plains_18
            new bool[] { true, true, false, true },    // plains_19
            new bool[] { true, false, false, true },   // plains_20
            new bool[] { true, false, true, true },    // plains_21
        };

        if (dirtIndex >= dirtRules.Length) return false;

        bool[] rules = dirtRules[dirtIndex];

        for (int i = 0; i < neighbors.Length; i++)
        {
            if (rules[i] && neighbors[i] != TileType.Grass)
                return false;

            if (!rules[i] && neighbors[i] == TileType.Grass)
                return false;
        }

        return true;
    }

    TileType[] GetTileTypeNeighbors(TileBase[,] chunk, int x, int y)
    {
        return new TileType[]
        {
            GetTileType(chunk, x, y + 1), // Top
            GetTileType(chunk, x - 1, y), // Left
            GetTileType(chunk, x + 1, y), // Right
            GetTileType(chunk, x, y - 1)  // Bottom
        };
    }

    TileType[] GetDiagonalNeighbors(TileBase[,] chunk, int x, int y)
    {
        return new TileType[]
        {
            GetTileType(chunk, x - 1, y + 1), // Top-left
            GetTileType(chunk, x + 1, y + 1), // Top-right
            GetTileType(chunk, x - 1, y - 1), // Bottom-left
            GetTileType(chunk, x + 1, y - 1)  // Bottom-right
        };
    }

    TileType GetTileType(TileBase[,] chunk, int x, int y)
    {
        if (x < 0 || y < 0 || x >= chunkSize || y >= chunkSize)
            return TileType.None;

        TileBase tile = chunk[x, y];

        if (tile == null)
            return TileType.None;

        if (System.Array.Exists(grassTiles, t => t == tile))
            return TileType.Grass;

        if (System.Array.Exists(dirtTiles, t => t == tile))
            return TileType.Dirt;

        return TileType.None;
    }

    TileBase[,] GenerateFeatureChunk(Vector2Int chunkPos, TileBase[,] terrainChunk)
    {
        TileBase[,] chunk = new TileBase[chunkSize, chunkSize];

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                if (terrainChunk[x, y] != null)
                {
                    if (Random.value < 0.05f)
                    {
                        chunk[x, y] = resourceTiles[Random.Range(0, resourceTiles.Length)];
                    }
                    else if (terrainChunk[x, y] == dirtTiles[0] && Random.value < 0.3f)
                    {
                        chunk[x, y] = wallTiles[Random.Range(0, wallTiles.Length)];
                    }
                }
            }
        }

        return chunk;
    }

    void PlaceChunkOnTilemap(Vector2Int chunkPos, TileBase[,] chunk, Tilemap tilemap)
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                Vector3Int tilePos = new Vector3Int(chunkPos.x * chunkSize + x, chunkPos.y * chunkSize + y, 0);
                if (chunk[x, y] != null)
                {
                    tilemap.SetTile(tilePos, chunk[x, y]);
                }
            }
        }
    }
}
