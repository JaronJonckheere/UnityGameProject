using UnityEngine;

public class Block : MonoBehaviour
{
    public int blockID;  // Unique identifier for the block
    public Vector3 position;  // Position of the block in the world
    public string blockType;  // Type of block (e.g., "stone", "dirt", etc.)

    void Start()
    {
        // Initialize block properties
        blockID = 1;  // You can assign unique IDs or make this dynamic
        position = transform.position;  // Set position to the transform's position
        blockType = "stone";  // Example type
    }
}
