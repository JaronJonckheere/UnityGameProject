using UnityEngine;
using System.Collections.Generic;

public class ChestController : MonoBehaviour
{
    public Animator animator;  // Reference to the chest animator
    public int chestID;        // Unique ID for the chest
    public List<Item> items = new List<Item>();  // List to hold items in the chest
    public GameObject chestUI;  // UI reference for chest inventory
    public Inventory inventory;  // Inventory that holds items for this chest
    public bool isOpen = false;

    void Start()
    {
        // Initialize chest in closed state
        animator.Play("chest_idle_closed");

        // Safeguard check to avoid null reference error
        if (chestUI != null)
        {
            chestUI.SetActive(false);  // Start with the chest UI hidden
        }
        else
        {
            Debug.LogWarning("Chest UI not assigned in the Inspector.");
        }

        // Initialize the chest inventory if it's not already set
        if (inventory == null)
        {
            inventory = new Inventory();
        }
    }

    public void OpenChest()
    {
        isOpen = true;
        // Play chest open animation
        animator.SetTrigger("chest_open_trigger");
        // Show the chest UI
        chestUI.SetActive(true);
        Debug.Log("Chest Opened");
    }

    public void CloseChest()
    {
        isOpen = false;
        // Play chest close animation
        animator.SetTrigger("chest_close_trigger");
        // Hide the chest UI
        chestUI.SetActive(false);
        Debug.Log("Chest Closed");
    }

    // Method to add an item to the chest inventory
    public void AddItemToChest(Item item)
    {
        items.Add(item);
    }

    // Method to remove an item from the chest inventory
    public void RemoveItemFromChest(Item item)
    {
        items.Remove(item);
    }

    // You can expand this script to handle dragging and dropping items from player inventory to chest inventory
}
