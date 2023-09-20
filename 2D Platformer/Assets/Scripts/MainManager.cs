using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    public PlayerData_SO data;
    public event Action pauseGameEvent;
    public event Action resumeGameEvent;
    
    public int coinCount = 0;
    public int liveCount = 3;
    public int score = 0;
    public int timeLimitSeconds = 400;

    public event Action coinAddEvent;
    public event Action timeDecreaseEvent;
    public event Action scoreAddEvent;
    public event Action liveCountUpdateEvent;

    public bool isNewScene = true;

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
    }

    private void Start()
    {
        Player.instance.onPlayerDeathEvent += OnPlayerDeath;
    }

    private void Update()
    {
        TimeLimitCountdown();
    }

    public void ConfigureNewSceneLoad()
    {
        ResetTimer();
        SavePlayerData_NewScene();
        Player.instance.ConfigureNewSceneLoad();
        UIManager.instance.UpdateUI();
    }

    public void ConfigureSavePointLoad()
    {
        ResetTimer();
        LoadPlayerData_NewScene();
    }

    public void AddCoin()
    {
        coinCount++;
        if(coinCount >= 100)
        {
            coinCount -= 100;
            liveCount++;
            liveCountUpdateEvent();
        }
        AddToScore(200);
        if (coinAddEvent != null) coinAddEvent();
    }

    public void AddToScore(int points)
    {
        score += points;
        if(scoreAddEvent != null)scoreAddEvent();
    }

    public void DecreaseTime(int seconds)
    {
        timeLimitSeconds -= seconds;
        timeDecreaseEvent();
    }

    public void SavePlayerData_NewScene()
    {
        data.score = score;
        data.coin = coinCount;
        data.live = liveCount;
        data.powerLevel = Player.instance.GetCurrentPower();
    }
    public void LoadPlayerData_NewScene()
    {
        score = data.score;
        coinCount = data.coin;
        liveCount = data.live;
        Player.instance.SetPowerLevel(data.powerLevel);
        UIManager.instance.UpdateUI();
    }

    public void ResertPlayerData()
    {
        score = 0;
        coinCount = 0;
        liveCount = 3;
        Player.instance.SetPowerLevel(1);
        SpawnPointManager.instance.ResetPlayerSavePoint();
    }

    public void ResetTimer()
    {
        timeLimitSeconds = 400;
    }

    public PlayerData_SO GetPlayerData()
    {
        return data;
    }

    public void StartConvertTime()
    {
        StartCoroutine(ConvertTimeToPoints());
    }

    private IEnumerator ConvertTimeToPoints()
    {
        while (timeLimitSeconds != 0)
        {
            DecreaseTime(1);
            AddToScore(100);
            yield return new WaitForSeconds(0.01f);
        }

        CameraScript.instance.CameraFadeOut();
        SceneLoader.instance.Invoke("LoadNextScene", 1f);
        SpawnPointManager.instance.ResetPlayerSavePoint();
        SavePlayerData_NewScene();
    }

    public void PauseGame()
    {
        if(pauseGameEvent != null) pauseGameEvent();
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        if(resumeGameEvent != null) resumeGameEvent();
        Time.timeScale = 1;
    }

    public int GetCurrentCoinCount()
    {
        return coinCount;
    }

    public int GetCurrentScore()
    {
        return score;
    }

    public int GetCurrentLiveCount()
    {
        return liveCount;
    }

    public int GetCurrentRemainingTime()
    {
        return timeLimitSeconds;
    }

    private float secondTimer;

    private void TimeLimitCountdown()
    {
        if (!Player.instance.goalReached)
        {
            secondTimer += Time.deltaTime;
            if (secondTimer >= 0.5f)
            {
                secondTimer = 0;
                timeLimitSeconds--;
                timeDecreaseEvent();
            }
        }
    }

    private void OnPlayerDeath()
    {
        liveCount--;
        if (liveCountUpdateEvent != null) liveCountUpdateEvent();
        if (liveCount == 0)
        {
            ResertPlayerData();
        }
        SavePlayerData_NewScene();
        StartCoroutine(PlayerDeathRoutine());
    }

    private IEnumerator PlayerDeathRoutine()
    {
        yield return new WaitForSeconds(2);
        SceneLoader.instance.ReloadScene();
    }
}
