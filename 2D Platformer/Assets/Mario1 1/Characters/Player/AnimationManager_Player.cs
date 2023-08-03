using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager_Player : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Player player;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeAnimationToIdle()
    {
        switch (player.GetCurrentPower())
        {
            case 1:
                anim.Play("Idle_1");
                break;
            case 2:
                anim.Play("Idle_2");
                break;
            case 3:
                break;
        }
    }

    public void ChangeAnimationToRun()
    {
        switch (player.GetCurrentPower())
        {
            case 1:
                anim.Play("Run_1");
                break;
            case 2:
                anim.Play("Run_2");
                break;
            case 3:
                break;
        }
    }

    public void ChangeAnimationToJump()
    {
        switch (player.GetCurrentPower())
        {
            case 1:
                anim.Play("Jump_1");
                break;
            case 2:
                anim.Play("Jump_2");
                break;
            case 3:
                break;
        }
    }

    public void AnimatePowerUp()
    {
        switch (player.GetCurrentPower())
        {
            case 1:
                anim.Play("PowerUp_1");
                break;
            case 2:
                break;
        }
    }
}
