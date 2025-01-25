using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveGame(Player player, Monster[] monsters, ChestController[] chests, MapData map)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gameData.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(player, monsters, chests, map);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Game saved to " + path);
    }

    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/gameData.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            Debug.Log("Game loaded from " + path);
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
