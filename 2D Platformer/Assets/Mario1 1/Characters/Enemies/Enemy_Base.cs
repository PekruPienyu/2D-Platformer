using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Base : AIMove_Base, IDamageable
{
    private void Start()
    {
        SetInitialMoveDirection(Vector2.left);
    }

    public abstract void OnHit(bool popOut);

}
