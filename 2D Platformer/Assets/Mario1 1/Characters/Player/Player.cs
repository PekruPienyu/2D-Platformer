using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour, IDamageable
{
    private int powerLevel = 1;
    private int coinCount = 0;
    private int liveCount = 3;
    private int score = 0;
    [SerializeField] private int timeLimitSeconds = 400;
    private bool isDamageable = false;
    private AnimationManager_Player playerAnimator;
    [SerializeField] private Player_Controller playerController;
    [SerializeField] private GameObject deathLine;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private CameraFollow cam;

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
        powerLevel = 1;
    }
    void Start()
    {
        playerAnimator = GetComponent<AnimationManager_Player>();
        transform.position = spawnPoint.transform.position;
    }

    private void Update()
    {
        if (playerController.pauseGame) return;
        if(transform.position.y <= deathLine.transform.position.y && !isDeathFall)
        {
            PlayerDeath();
        }

        if(!isDamageable)
        {
            immuneTimer += Time.deltaTime;
            if(immuneTimer >= 1f)
            {
                immuneTimer = 0;
                isDamageable = true;
            }
        }
        DeathFallTimer();
        KillComboTimer();
        TimeLimitCountdown();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Coin"))
        {
            collision.gameObject.SetActive(false);
            AddCoin();
        }
        if (collision.gameObject.CompareTag("Pole"))
        {
            goalReached = true;
            playerController.GoalReachedConfigure();
            AddToScore(FindObjectOfType<EndPole>().GetHeightPoints(transform.position.y));
        }
        if (collision.gameObject.CompareTag("Door"))
        {
            playerController.castleReached = true;
            playerController.StopPlayerMovement();
            StartCoroutine(ConvertTimeToPoints());
        }
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

    private IEnumerator ConvertTimeToPoints()
    {
        while(timeLimitSeconds != 0)
        {
            timeLimitSeconds--;
            timeDecreaseEvent();
            AddToScore(100);
            yield return null;
        }
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
                ResetPlayerPosition();
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

    public void SetDamageable(bool _isDamageable)
    {
        isDamageable = _isDamageable;
    }

    public void OnHit(bool popOut)
    {
        if (!isDamageable) return;

        if(powerLevel > 1)
        {
            powerLevel = 1;
            isDamageable = false;
            playerController.SetCurrentBoxCollider();
            playerAnimator.AnimatePowerDown();
        }
        else
        {
            PlayerDeath();
        }
    }

    public void PlayerDeath()
    {
        playerController.PlayerDeathJump();
        deathFallTimer = 0;
        isDeathFall = true;
    }

    public void ResetPlayerPosition()
    {
        liveCount--;
        liveCountUpdateEvent();
        playerAnimator.ChangeAnimationToIdle();
        transform.position = spawnPoint.transform.position;
        playerController.SetIsActive(true);
        playerController.EnableBoxCollider();
        cam.ResetPositionToStart();
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
}
