using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Mushroom : Misc_Base
{
    private bool isActive;
    private Vector3 startPos;

    public override void Configure(Vector3 origin)
    {
        transform.position = origin;
        startPos = origin;
        SetBoxColliderActive(false);
        popUpRoutine = StartCoroutine(PopUp());
    }

    private Coroutine popUpRoutine;

    public override IEnumerator PopUp()
    {
        while(transform.position.y < startPos.y + boxCol.size.y + 0.05f)
        {
           transform.Translate(Vector3.up * Time.deltaTime);
           yield return null;
        }
        SetBoxColliderActive(true);
        isActive = true;
        if (popUpRoutine != null) popUpRoutine = null;
        yield return null;
    }
    public override void FixedUpdateMovementUpdate()
    {
        if (!isActive) return;

        base.FixedUpdateMovementUpdate();
    }

    public override void UpdateCheckUpdate()
    {
        if (!isActive) return;

        base.UpdateCheckUpdate();
    }
}
