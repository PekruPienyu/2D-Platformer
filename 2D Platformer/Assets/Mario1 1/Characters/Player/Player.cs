using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Player : MonoBehaviour, IDamageable
{
    private string FILE_PATH;

    private int powerLevel = 1;
    private int coinCount = 0;
    private int liveCount = 3;
    private int score = 0;
    [SerializeField] private GameObject bulletPrefab;
    public int timeLimitSeconds = 400;
    private bool isDamageable = false;
    private AnimationManager_Player playerAnimator;
    [SerializeField] private LayerMask secretEntranceScanRay;
    [SerializeField] private Player_Controller playerController;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private LayerMask enemyLayer;


    public event Action coinAddEvent;
    public event Action timeDecreaseEvent;
    public event Action scoreAddEvent;
    public event Action liveCountUpdateEvent;

    public static Player instance;
    private float immuneTimer;

    private float killComboTimer;
    private bool isKillComboTime;
    private int killComboPointMultiplier = 1;

    private bool isDeathFall;
    private float deathFallTimer;

    private bool goalReached;
    [HideInInspector] public Vector2 facingDir;

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
        FILE_PATH = Path.Combine(Application.dataPath, "PlayerData.dat");
        powerLevel = 1;
    }
    void Start()
    {
        SavePlayerData_NewScene();
        playerAnimator = GetComponent<AnimationManager_Player>();
    }

    private void Update()
    {
        if (playerController.pauseGame) return;

        if(!isDamageable)
        {
            immuneTimer += Time.deltaTime;
            if(immuneTimer >= 1f)
            {
                isDamageable = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            if (!playerController.isActive) return;
            if(powerLevel == 3)
            {
                SpawnBullet();
            }
        }

        if(Input.GetKeyDown(playerController.playerControllerKeys.down))
        {
            CheckForSecretEntrance();
        }
        DeathFallTimer();
        KillComboTimer();
        TimeLimitCountdown();
    }

    public void SavePlayerData_NewScene()
    {
        MainManager.instance.SavePlayerData_NextScene(score, coinCount, liveCount, spawnPoint);
    }

    public void LoadPlayerData_NewScene()
    {
        PlayerData_SceneLoad data = MainManager.instance.GetPlayerData();
        score = data.score;
        coinCount = data.coin;
        liveCount = data.live;
        spawnPoint = data.spawnPos;
        timeLimitSeconds = 400;
        UIManager.instance.UpdateUI();
    }

    private void SpawnBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.GetComponent<BulletScript>().Initialize(transform.position, facingDir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Coin"))
        {
            collision.gameObject.SetActive(false);
            AddCoin();
        }
        if (collision.CompareTag("Pole"))
        {
            goalReached = true;
            playerController.GoalReachedConfigure(collision.gameObject);
            AddToScore(FindObjectOfType<EndPole>().GetHeightPoints(transform.position.y));
        }
        if (collision.CompareTag("Castle"))
        {
            playerController.castleReached = true;
            playerController.StopPlayerMovement();
            playerController.movingToCastleAI = false;
            playerController.SetIsActive(false);
            GetComponent<SpriteRenderer>().enabled = false;
            MainManager.instance.StartConvertTime();
        }
        if(collision.CompareTag("DeathLine"))
        {
            PlayerDeath();
        }
        if(collision.CompareTag("SavePoint"))
        {
            spawnPoint = collision.gameObject.transform.position;
        }
        if(collision.CompareTag("SecretRoomExit"))
        {
            playerController.isWalkingRightAI = true;
            playerController.StopPlayerMovement();
            playerAnimator.ChangeAnimationToRun();
            playerController.SetIsActive(false);
            playerController.DisableBoxCollider();
        }
    }

    public void ConfigureNewSceneLoad()
    {
        goalReached = false;
        playerController.castleReached = false;
        if (SceneLoader.instance.GetCurrentSceneIndex() > 1)
        {
            playerController.endPole = FindObjectOfType<EndPole>().gameObject;
            playerController.SetIsActive(true);
        }
        GetComponent<SpriteRenderer>().enabled = true;
        timeLimitSeconds = 400;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            Destroy(collision.gameObject);
            playerAnimator.AnimatePowerUp();
            AddToScore(1000);
            FloatingScorePool.instance.GetFromPool(transform.position, 1000);
        }
        if (collision.gameObject.CompareTag("Star"))
        {
            Destroy(collision.gameObject);
            AddToScore(1000);
            FloatingScorePool.instance.GetFromPool(transform.position, 1000);
        }
    }

    private void CheckForSecretEntrance()
    {
        BoxCollider2D boxCol = GetComponentInChildren<BoxCollider2D>();
        RaycastHit2D ray = Physics2D.Raycast(boxCol.bounds.center, Vector2.down, boxCol.bounds.extents.y + 0.1f, secretEntranceScanRay);

        if(ray.collider != null)
        {
            playerAnimator.ChangeAnimationToIdle();
            playerController.isElevatingDown = true;
            playerController.StopPlayerMovement();
            playerController.SetIsActive(false);
            playerController.DisableBoxCollider();
            ray.collider.gameObject.GetComponent<SecretRoom_Helper>().SecretRoomConfigure();
        }
    }

    public void EnterSecretRoomConfigure()
    {
        playerAnimator.ChangeAnimationToJump();
        playerController.EnableBoxCollider();
    }

    public void ExitSecretRoomConfigure()
    {
        playerAnimator.ChangeAnimationToIdle();
        playerController.isElevatingUp = true;
    }

    public void SetPosition(Vector3 origin)
    {
        transform.position = origin;
    }

    public void AddCoin()
    {
        coinCount++;
        AddToScore(200);
        if(coinAddEvent != null)coinAddEvent();
    }

    public void AddToScore(int points)
    {
        score += points;
        scoreAddEvent();
    }

    private void DeathFallTimer()
    {
        if(isDeathFall)
        {
            deathFallTimer += Time.deltaTime;
            if(deathFallTimer >= 2f)
            {
                isDeathFall = false;
                ReloadSceneOnDeath();
            }
        }
    }

    public void ActivateKillComboTimer()
    {
        isKillComboTime = true;
        killComboTimer = 0;
        killComboPointMultiplier++;
    }

    public void KillComboTimer()
    {
        if(isKillComboTime)
        {
            killComboTimer += Time.deltaTime;
            if(killComboTimer >= 1f)
            {
                isKillComboTime = false;
                killComboPointMultiplier = 1;
            }
        }
    }

    public int GetKillComboPointMultiplier()
    {
        return killComboPointMultiplier;
    }

    public bool IsPoweredUp()
    {
        if(powerLevel > 1)
        {
            return true;
        }
        return false;
    }

    public void SetDamageableFalse()
    {
        immuneTimer = 0;
        isDamageable = false;
    }

    public void OnHit(bool popOut)
    {
        if (!isDamageable) return;

        if(powerLevel > 1)
        {
            powerLevel = 1;
            isDamageable = false;
            playerController.SetCurrentBoxCollider();
            MainManager.instance.PauseGame();
            playerAnimator.AnimatePowerDown();
        }
        else
        {
            PlayerDeath();
        }
    }

    public void PlayerDeath()
    {
        SceneLoader.instance.isNewScene = false;
        playerController.PlayerDeathJump();
        powerLevel = 1;
        deathFallTimer = 0;
        isDeathFall = true;
        liveCount--;
        liveCountUpdateEvent();
        if(liveCount <= 0)
        {
            MainManager.instance.ResertPlayerData();
        }
        else SavePlayerData_NewScene();
    }

    public void ResetPlayerPosition()
    {
        playerAnimator.ChangeAnimationToIdle();
        transform.position = spawnPoint;
        playerController.SetIsActive(true);
        playerController.SetCurrentBoxCollider();
        playerController.EnableBoxCollider();
    }

    public void ReloadSceneOnDeath()
    {
        SceneLoader.instance.ReloadScene();
    }

    public void IncrementPower()
    {
        powerLevel++;
        if(powerLevel > 3)
        {
            powerLevel = 3;
        }
        playerController.SetCurrentBoxCollider();
    }

    private float secondTimer;

    private void TimeLimitCountdown()
    {
        if (!goalReached)
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

    public void SetNewSpawnPos(Vector3 _spawnPoint)
    {
        spawnPoint = _spawnPoint;
    }

    public void GetDamaged()
    {
        powerLevel = 1;
    }

    public int GetCurrentPower()
    {
        return powerLevel;
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

    public void DecreaseTime(int seconds)
    {
        timeLimitSeconds -= seconds;
        timeDecreaseEvent();
    }
}
