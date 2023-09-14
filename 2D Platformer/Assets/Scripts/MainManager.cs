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
    
    public int coinCount = 0;
    public int liveCount = 3;
    public int score = 0;
    public int timeLimitSeconds = 400;

    public event Action coinAddEvent;
    public event Action timeDecreaseEvent;
    public event Action scoreAddEvent;
    public event Action liveCountUpdateEvent;

    private SpawnPointManager spawnPointManager;

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
        data.live = liveCount;
    }

    private void Start()
    {
        Player.instance.onPlayerDeathEvent += OnPlayerDeath;
    }

    private void Update()
    {
        TimeLimitCountdown();
    }

    public void ConfigureNewSceneLoad(SpawnPointManager spawnManager)
    {
        spawnPointManager = spawnManager;
        ResetTimer();
        if(isNewScene)
        {
            SavePlayerData_NewScene();
        }
        else LoadPlayerData_NewScene();
        spawnPointManager.SceneLoadConfigure(isNewScene);
    }

    public void AddCoin(bool floatingScore)
    {
        coinCount++;
        if(coinCount >= 100)
        {
            coinCount -= 100;
            liveCount++;
            liveCountUpdateEvent();
        }
        AddToScore(200);
        if (floatingScore) FloatingScorePool.instance.GetFromPool(Player.instance.transform.position, 200);
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
        data.spawnPos = spawnPointManager.playerSavePoint;
    }
    public void LoadPlayerData_NewScene()
    {
        score = data.score;
        coinCount = data.coin;
        liveCount = data.live;
        Player.instance.SetNewSpawnPos(data.spawnPos);
        UIManager.instance.UpdateUI();
    }

    public void ResertPlayerData()
    {
        score = 0;
        coinCount = 0;
        liveCount = 3;
        spawnPointManager.ResetPlayerSavePoint();
    }

    public void ResetTimer()
    {
        timeLimitSeconds = 400;
    }

    public void SetPlayerSavePoint(Vector3 newPos)
    {
        spawnPointManager.SetPlayerSavePoint(newPos);
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
        while (timeLimitSeconds != 0)
        {
            DecreaseTime(1);
            AddToScore(100);
            yield return new WaitForSeconds(0.01f);
        }

        CameraScript.instance.CameraFadeOut();
        isNewScene = true;
        SceneLoader.instance.Invoke("LoadNextScene", 1f);
        SavePlayerData_NewScene();
    }

    public void PauseGame()
    {
        if(pauseGame != null)pauseGame();
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        if(resumeGame != null)resumeGame();
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
            isNewScene = true;
        }
        else
        {
            isNewScene = false;
        }
        SavePlayerData_NewScene();
        StartCoroutine(PlayerDeathRoutine());
    }

    private IEnumerator PlayerDeathRoutine()
    {
        yield return new WaitForSeconds(2);
        isNewScene = false;
        SceneLoader.instance.ReloadScene();
    }
}
