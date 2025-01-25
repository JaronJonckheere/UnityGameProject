using System;
using System.Collections.Generic;

[Serializable]
public class ChestData
{
    public int chestID;
    public List<Item> chestItems;  // Assuming chest stores items
    public Inventory inventory;  // Add this line if chest holds an inventory

    public ChestData(ChestController chest)
    {
        chestID = chest.chestID;
        chestItems = new List<Item>(chest.inventory.items);  // Assuming chest has an inventory system
    }
}
