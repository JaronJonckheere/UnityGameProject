using UnityEngine;
using System.Collections.Generic;  // Needed for List<T>

public class Player : MonoBehaviour
{
    public int level = 3;
    public int health = 40;
    public Inventory inventory;  // Assume you have an Inventory class

    public void SaveGame()
    {
        // Find all monsters in the scene
        Monster[] monsters = FindObjectsByType<Monster>(FindObjectsSortMode.None);  // Find all Monster objects
        // Find all chests in the scene
        ChestController[] chests = FindObjectsByType<ChestController>(FindObjectsSortMode.None);  // Find all Chest objects
        // Find all blocks in the scene (assuming Block is the class representing map blocks)
        List<Block> mapBlocks = new List<Block>(FindObjectsByType<Block>(FindObjectsSortMode.None));  // Find all Block objects

        // Pass the mapBlocks to the MapData constructor
        MapData mapData = new MapData(mapBlocks);

        // Save the game using SaveSystem
        SaveSystem.SaveGame(this, monsters, chests, mapData);
    }

    public void LoadGame(GameData data)
    {
        // Load player-specific data
        level = data.playerLevel;
        health = data.playerHealth;
        transform.position = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);

        // Restore player's inventory
        inventory.items = data.playerInventory;
    }
}
