using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Misc_Base
{
    private Vector3 startPos;
    private Coroutine popUpRoutine;
    private CapsuleCollider2D capsuleCol;

    public override void Configure(Vector3 origin)
    {
        capsuleCol = GetComponent<CapsuleCollider2D>();
        transform.position = origin;
        startPos = origin;
        popUpRoutine = StartCoroutine(PopUp());
    }

    public override IEnumerator PopUp()
    {
        while (transform.position.y < startPos.y + (capsuleCol.size.y * 2))
        {
            transform.Translate(3 * Time.deltaTime * Vector3.up);
            yield return null;
        }
        while(transform.position.y > startPos.y)
        {
            transform.Translate(3 * Time.deltaTime * Vector3.down);
            yield return null;
        }

        if (popUpRoutine != null) popUpRoutine = null;
        yield return null;
        Destroy(gameObject);
    }

    public override void UpdateCheckAndMovement()
    {
        return;
    }
}
