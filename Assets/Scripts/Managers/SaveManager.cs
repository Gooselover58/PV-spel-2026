using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static readonly string savePath = Path.Combine(Application.persistentDataPath, "SaveFile.json");

    public static void SaveGame()
    {
        PlayerData data = new PlayerData();

        data.deaths = Global.deaths;
        data.dialogueTriggers = new bool[Global.dialogueTriggerAmount];
        for (int i = 0; i < Global.dialogueTriggers.Length; i++)
        {
            data.dialogueTriggers[i] = Global.dialogueTriggers[i];
        }

        string file = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, file);
    }

    public static void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogError("Unable to locate existing save file");
            CreateNewSaveFile();
            return;
        }

        PlayerData data = JsonUtility.FromJson<PlayerData>(savePath);

        Global.deaths = data.deaths;
        for (int i = 0; i < Global.dialogueTriggers.Length; i++)
        {
            Global.dialogueTriggers[i] = data.dialogueTriggers[i];
        }
    }

    public static void CreateNewSaveFile()
    {
        Global.deaths = 0;
        Global.dialogueTriggers = new bool[Global.dialogueTriggerAmount];
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