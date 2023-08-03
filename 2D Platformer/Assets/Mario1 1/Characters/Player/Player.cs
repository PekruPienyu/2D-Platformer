using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int powerLevel = 1;
    private int coinCount = 0;
    private AnimationManager_Player playerAnimator;
    [SerializeField] private GameObject deathLine;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private CameraFollow cam;

    public static Player instance;

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
    }
    void Start()
    {
        playerAnimator = GetComponent<AnimationManager_Player>();
        transform.position = spawnPoint.transform.position;
    }

    private void Update()
    {
        if(transform.position.y <= deathLine.transform.position.y)
        {
            PlayerDeath();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Coin"))
        {
            collision.gameObject.SetActive(false);
            Debug.Log("Coin Get");
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            Destroy(collision.gameObject);
            playerAnimator.AnimatePowerUp();
        }
    }

    public bool IsPoweredUp()
    {
        if(powerLevel > 1)
        {
            return true;
        }
        return false;
    }

    private void PlayerDeath()
    {
        transform.position = spawnPoint.transform.position;
        cam.ResetPositionToStart();
    }

    public void IncrementPower()
    {
        powerLevel++;
        if(powerLevel > 3)
        {
            powerLevel = 3;
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
}
