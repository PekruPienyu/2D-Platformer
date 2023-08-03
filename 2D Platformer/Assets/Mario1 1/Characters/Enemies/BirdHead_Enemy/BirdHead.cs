using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdHead : AIMove_Base
{
    [SerializeField] private Animator anim;
    private bool isActive = true;

    public override void UpdateCheckAndMovement()
    {
        if (isActive)
        {
            base.UpdateCheckAndMovement();
        }
        else
        {
            StopMovement();
        }
    }

    public void OnHit()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        isActive = false;
        anim.Play("Death");
    }

    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
