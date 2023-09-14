using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tortoise : Enemy_Base
{
    [SerializeField] private Animator anim;
    private bool canMove = true;
    private float defenceTimer;
    [SerializeField] private float defenceDuration;

    private enum State
    {
        Walk,
        Defence,
        Slide
    }
    private State currentState = State.Walk;

    public override void FixedUpdateMovementUpdate()
    {
        switch (currentState)
        {
            case State.Walk:
                break;
            case State.Defence:
                defenceTimer += Time.fixedDeltaTime;
                if(defenceTimer >= defenceDuration)
                {
                    anim.Play("DefenceToWalk");
                }
                break;
            case State.Slide:

                break;
        }
        BodyCheckBoxCast();
        if (!canMove) return;
        base.FixedUpdateMovementUpdate();
    }

    public override void OnHit(bool popOut)
    {
        switch (currentState)
        {
            case State.Walk:
                anim.Play("WalkToDefence");
                canMove = false;
                StopMovement();
                break;
            case State.Defence:
                canMove = true;
                defenceTimer = 0;
                if(Player.instance.transform.position.x >= transform.position.x)
                {
                    SetMoveDirStrength(-10);
                }
                else
                {
                    SetMoveDirStrength(10);
                }
                anim.Play("Defence");
                currentState = State.Slide;
                SetFriendlyFire(true);
                MainManager.instance.AddToScore(100);
                FloatingScorePool.instance.GetFromPool(transform.position, 100);
                break;
            case State.Slide:
                canMove = false;
                StopMovement();
                defenceTimer = 0;
                SetMoveDirDefault();
                currentState = State.Defence;
                break;
        }
    }

    private void BodyCheckBoxCast()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(boxCol.bounds.center, new Vector2(boxCol.size.x + 0.05f, boxCol.size.y - 0.2f), 0, Vector2.zero, 0);

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject == gameObject) continue;

            if (hit.collider.CompareTag("Player"))
            {
                if (currentState != State.Defence)
                {
                    Player.instance.OnHit(false);
                }
                else
                {
                    OnHit(false);
                }
            }
            if (IsFriendyFireActive() && hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<IDamageable>().OnHit(true);
            }

        }
    }

    public void ChangeStateToDefence()
    {
        anim.Play("Defence");
        currentState = State.Defence;
    }

    public void ChangeStateToWalk()
    {
        anim.Play("Walk");
        canMove = true;
        SetFriendlyFire(false);
        currentState = State.Walk;
    }

}
