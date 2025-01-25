using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public int level;
    public int health;
    public float[] position;
    public List<Item> inventoryItems;  // Save the inventory items

    public PlayerData(Player player)
    {
        level = player.level;
        health = player.health;

        // Save player position
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        // Save player's inventory items
        inventoryItems = new List<Item>();
        foreach (Item item in player.inventory.items)
        {
            inventoryItems.Add(item);
        }
    }
}
