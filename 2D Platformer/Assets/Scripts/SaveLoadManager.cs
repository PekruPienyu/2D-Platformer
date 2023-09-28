using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SavePlayerData(PlayerData_SO data, string filePath)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + filePath, json);
    }

    public PlayerData_SO GetPlayerData(string filePath)
    {
        if (File.Exists(Application.dataPath + filePath))
        {
            string json = File.ReadAllText(Application.dataPath + filePath);
            PlayerData_SO data = ScriptableObject.CreateInstance<PlayerData_SO>();
            JsonUtility.FromJsonOverwrite(json, data);
            return data;
        }
        return null;
    }
}
