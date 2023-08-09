using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Flower : Misc_Base
{
    private Vector3 startPos;
    private Coroutine popUpRoutine;

    public override void Configure(Vector3 origin)
    {
        transform.position = origin;
        startPos = origin;
        SetBoxColliderActive(false);
        popUpRoutine = StartCoroutine(PopUp());
    }
    public override IEnumerator PopUp()
    {
        while (transform.position.y < startPos.y + boxCol.size.y)
        {
            transform.Translate(Vector3.up * Time.deltaTime);
            yield return null;
        }
        SetBoxColliderActive(true);
        if (popUpRoutine != null) popUpRoutine = null;
        yield return null;
    }

    public override void FixedUpdateMovementUpdate()
    {
        return;
    }
}
