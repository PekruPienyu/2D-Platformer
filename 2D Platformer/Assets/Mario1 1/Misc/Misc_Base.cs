using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Misc_Base : AIMove_Base
{
    private void Start()
    {
        SetInitialMoveDirection(Vector2.right);
    }

    public abstract void Configure(Vector3 origin);
    public abstract IEnumerator PopUp();
}
