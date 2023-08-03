using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MyItem
{
    Coin,
    PowerUp,
    Star
}
public abstract class Tile_Base : MonoBehaviour
{
    private BoxCollider2D boxCol;
    private SpriteRenderer spriteRenderer;

    [HideInInspector] public bool isActive;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isActive = true;
    }

    public abstract void OnHit();

    public Vector2 GetBoxColliderSize()
    {
        return boxCol.size;
    }

    public void DisableTile(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        isActive = false;
    }

    private Coroutine bounceRoutine;

    public void Bounce()
    {
        if (bounceRoutine == null)
        {
            bounceRoutine = StartCoroutine(StartBounce());
        }
    }

    public abstract void SpawnItem();
    private IEnumerator StartBounce()
    {
        Vector3 startPos = transform.position;
        float timer = 0f;
        float duration = 0.07f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y + (5f * Time.deltaTime), transform.position.z);
            yield return null;
        }
        timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y - (5f * Time.deltaTime), transform.position.z);
            yield return null;
        }
        transform.position = startPos;
        yield return null;
        bounceRoutine = null;
        SpawnItem();
    }

}
