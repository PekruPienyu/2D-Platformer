using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public static SlotManager instance;

    [HideInInspector] public string currentFilePath;

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

    public void SetFilePath(string filePath)
    {
        currentFilePath = filePath;
    }

    public void SavePlayerData(PlayerData_SO data)
    {
        SaveLoadManager.instance.SavePlayerData(data, currentFilePath);
    }

    public PlayerData_SO GetPlayerData()
    {
        return SaveLoadManager.instance.GetPlayerData(currentFilePath);
    }
}
