using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private int powerLevel = 1;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeAnimationToJump()
    {
        anim.Play("Jump");
    }

    public void ChangeAnimationToIdle()
    {
        anim.Play("Idle");
    }

    public void ChangeAnimationToRun()
    {
        anim.Play("Run");
    }

    public void IncrementPower()
    {
        powerLevel++;
    }

    public void GetDamaged()
    {
        powerLevel = 1;
    }

    public int GetCurrentPower()
    {
        return powerLevel;
    }
}
