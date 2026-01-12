using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static readonly string savePath = Path.Combine(Application.persistentDataPath, "SaveFile.json");

    public static void SaveGame()
    {

    }

    public static void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogError("Unable to locate existing save file");
            CreateNewSaveFile();
            return;
        }

    }

    private static void CreateNewSaveFile()
    {

    }
}

[System.Serializable]
public class PlayerData
{
    public int deaths;
}