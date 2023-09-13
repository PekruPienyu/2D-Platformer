using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    private PlayerData_SceneLoad data = new();
    public event Action pauseGame;
    public event Action resumeGame;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        data.live = Player.instance.GetCurrentLiveCount();
    }

    public void SavePlayerData_NextScene(int _score, int _coin, int _live, Vector3 spawnPoint)
    {
        data.score = _score;
        data.coin = _coin;
        data.live = _live;
        data.spawnPos = spawnPoint;
    }

    public void ResertPlayerData()
    {
        data.score = 0;
        data.coin = 0;
        data.live = 3;
    }

    public PlayerData_SceneLoad GetPlayerData()
    {
        return data;
    }

    public void StartConvertTime()
    {
        StartCoroutine(ConvertTimeToPoints());
    }

    private IEnumerator ConvertTimeToPoints()
    {
        while (Player.instance.GetCurrentRemainingTime() != 0)
        {
            Player.instance.DecreaseTime(1);
            Player.instance.AddToScore(100);
            yield return new WaitForSeconds(0.01f);
        }

        CameraScript.instance.CameraFadeOut();
        SceneLoader.instance.Invoke("LoadNextScene", 1f);
        Player.instance.SavePlayerData_NewScene();
    }

    public void PauseGame()
    {
        pauseGame();
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        resumeGame();
        Time.timeScale = 1;
    }

}
