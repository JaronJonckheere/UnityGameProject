using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // Player-related data
    public int playerLevel;
    public int playerHealth;
    public float[] playerPosition;
    public List<Item> playerInventory;

    // Chest data
    public List<ChestData> chests = new List<ChestData>();

    // Monster data
    public List<MonsterData> monsters = new List<MonsterData>();

    // Map data
    public MapData mapData;

    // Constructor for saving game data
    public GameData(Player player, Monster[] monsters, ChestController[] chests, MapData map)
    {
        // Save player data
        playerLevel = player.level;
        playerHealth = player.health;
        playerPosition = new float[3] { player.transform.position.x, player.transform.position.y, player.transform.position.z };

        // Save player's inventory (assuming the player has an inventory list of items)
        playerInventory = new List<Item>(player.inventory.items);  // Assuming an Inventory system is set up with items.

        // Save monsters
        foreach (Monster monster in monsters)
        {
            this.monsters.Add(new MonsterData(monster));
        }

        // Save chests
        foreach (ChestController chest in chests)
        {
            this.chests.Add(new ChestData(chest));  // Assuming ChestData is a class that stores chest info.
        }

        // Store map data
        this.mapData = map;
    }
}
