using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    public GameObject saveLoadMenu;  // Assign this to the Canvas in the Unity editor
    private bool isMenuVisible = false;

    public Player player;  // Reference to the player

    void Update()
    {
        // Check if ESC key is pressed to toggle save/load menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSaveLoadMenu();
        }
    }

    private void ToggleSaveLoadMenu()
    {
        isMenuVisible = !isMenuVisible;  // Toggle menu visibility
        saveLoadMenu.SetActive(isMenuVisible);  // Show or hide the menu
    }

    // Callbacks for the UI Buttons
    public void OnSaveButtonPressed()
    {
        player.SaveGame();
    }

    public void OnLoadButtonPressed()
    {
        // Load the game data from the SaveSystem
        GameData data = SaveSystem.LoadGame();

        // Ensure data was successfully loaded before proceeding
        if (data != null)
        {
            player.LoadGame(data);  // Pass the loaded data to the player
        }
        else
        {
            Debug.LogError("Failed to load game data.");
        }
    }
}
