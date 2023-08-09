using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager_Player : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeAnimationToIdle()
    {
        switch (Player.instance.GetCurrentPower())
        {
            case 1:
                anim.Play("Idle_Small");
                break;
            case 2:
                anim.Play("Idle_Big");
                break;
            case 3:
                anim.Play("Idle_BigPowered");
                break;
        }
    }

    public void ChangeAnimationToRun()
    {
        switch (Player.instance.GetCurrentPower())
        {
            case 1:
                anim.Play("Run_Small");
                break;
            case 2:
                anim.Play("Run_Big");
                break;
            case 3:
                anim.Play("Run_BigPowered");
                break;
        }
    }

    public void ChangeAnimationToJump()
    {
        switch (Player.instance.GetCurrentPower())
        {
            case 1:
                anim.Play("Jump_Small");
                break;
            case 2:
                anim.Play("Jump_Big");
                break;
            case 3:
                anim.Play("Jump_BigPowered");
                break;
        }
    }

    public void ChangeAnimationToPoleGrab()
    {
        switch (Player.instance.GetCurrentPower())
        {
            case 1:
                anim.Play("PoleGrab_Small");
                break;
            case 2:
                anim.Play("PoleGrab_Big");
                break;
            case 3:
                anim.Play("PoleGrab_BigPowered");
                break;
        }
    }

    public void ChangeAnimationToDeath()
    {
        anim.Play("Death");
    }

    public void AnimatePowerUp()
    {
        switch (Player.instance.GetCurrentPower())
        {
            case 1:
                anim.Play("PowerUp_SmallToBig");
                break;
            case 2:
                anim.Play("PowerUp_BigToBigPowered");
                break;
        }
    }

    public void AnimatePowerDown()
    {
        anim.Play("PowerDown");
    }
}
