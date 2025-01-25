using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  // Import the new Input System

public class ChestDistanceLogger : MonoBehaviour
{
    private Transform playerTransform;
    private ChestController[] allChests;
    private float logInterval = 2f;  // Time interval between logs
    private float timeSinceLastLog = 0f;

    void Start()
    {
        // Find the player's transform
        playerTransform = GameObject.FindWithTag("Player").transform;

        // Find all chests in the scene
        allChests = FindObjectsByType<ChestController>(FindObjectsSortMode.None);
    }

    void Update()
    {
        // Update the time since last log
        timeSinceLastLog += Time.deltaTime;

        // If 2 seconds have passed, log the distances
        if (timeSinceLastLog >= logInterval)
        {
            LogChestDistances();
            timeSinceLastLog = 0f;  // Reset the timer
        }
    }

    void LogChestDistances()
    {
        foreach (ChestController chest in allChests)
        {
            float distance = Vector2.Distance(playerTransform.position, chest.transform.position);
            Debug.Log("Distance from player to chest: " + distance);
        }
    }
}
