using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class SlotScript : MonoBehaviour
{
    public string filePath;
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        if (File.Exists(Application.dataPath + filePath))
        {
            string json = File.ReadAllText(Application.dataPath + filePath);
            PlayerData_SO data = ScriptableObject.CreateInstance<PlayerData_SO>();
            JsonUtility.FromJsonOverwrite(json, data);

            if(data.worldIndex == 1)
            {
                text.text = "EMPTY";
            }
            else
            {
                text.text = "WORLD " + data.worldIndex;
            }
        }
        else
        {
            text.text = "EMPTY";
        }
    }

    public void OnLoadButtonPressed()
    {
        SlotManager.instance.SetFilePath(filePath);
        if(File.Exists(Application.dataPath + filePath))
        {
            UIManager.instance.OnStartGame(true);
        }
        else
        {
            UIManager.instance.OnStartGame(false);
        }
    }
}
