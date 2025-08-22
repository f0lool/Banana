using UnityEngine;
using Unity.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using static Cinemachine.DocumentationSortingAttribute;

public static class SaveSystem
{
    public static void SaveGame(LevelConfig[] levels)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "GameData.fun";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        GameData gameData = new GameData(levels);
        binaryFormatter.Serialize(fileStream, gameData);
        fileStream.Close();
    }

    public static void SaveSettings(float volumeMusic, float volumeSfx)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "SettingsData.fun";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        SettingsData settingsData = new SettingsData(volumeMusic, volumeSfx);
        binaryFormatter.Serialize(fileStream, settingsData);
        fileStream.Close();
    }

    public static void SaveGame(GameData gameData, int index, bool isComplete)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "GameData.fun";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        gameData.CompleteLevel[index - 1] = isComplete;
        binaryFormatter.Serialize(fileStream, gameData);
        fileStream.Close();
    }

    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "GameData.fun";
        if(File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            GameData gameData = binaryFormatter.Deserialize(fileStream) as GameData;
            fileStream.Close();
            return gameData;

        } else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static SettingsData LoadSettings()
    {
        string path = Application.persistentDataPath + "SettingsData.fun";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            SettingsData settingsData = binaryFormatter.Deserialize(fileStream) as SettingsData;
            fileStream.Close();
            return settingsData;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
