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
        Global.deaths = 0;
        Global.dialogueTriggers = new bool[5];
        for (int i = 0; i < Global.dialogueTriggers.Length; i++)
        {
            Global.dialogueTriggers[i] = false;
        }

        SaveGame();
    }
}

[System.Serializable]
public class PlayerData
{
    public int deaths;
    public bool[] dialogueTriggers;
}