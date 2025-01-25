using System;

[Serializable]
public class Item
{
    public string itemName;
    public int itemID;
    public int quantity;

    public Item(string name, int id, int quantity)
    {
        itemName = name;
        itemID = id;
        this.quantity = quantity;
    }
}
