using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdHead : Enemy_Base
{
    [SerializeField] private Animator anim;
    private bool isActive = true;

    public override void FixedUpdateMovementUpdate()
    {
        if (isActive)
        {
            base.FixedUpdateMovementUpdate();
        }
        else
        {
            StopMovement();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.instance.OnHit(false);
        }
    }

    public override void OnHit(bool popOut)
    {
        GetComponent<BoxCollider2D>().enabled = false;

        if (popOut)
        {
            int ranIndex = Random.Range(0, 2);
            if (ranIndex == 0)
            {
                PopOffScreenConfigure(new Vector2(1, 5));
            }
            else
            {
                PopOffScreenConfigure(new Vector2(-1, 5));
            }
            GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            isActive = false;
            anim.Play("Death");
        }

        int score = 100 * Player.instance.GetKillComboPointMultiplier();
        Player.instance.ActivateKillComboTimer();
        MainManager.instance.AddToScore(score);
        FloatingScorePool.instance.GetFromPool(transform.position, score);
    }

    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
