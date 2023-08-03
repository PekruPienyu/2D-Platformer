using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Misc_Base : AIMove_Base
{
    public abstract void Configure(Vector3 origin);
    public abstract IEnumerator PopUp();
}
