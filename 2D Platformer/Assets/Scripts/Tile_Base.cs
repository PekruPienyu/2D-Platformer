using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MyItem
{
    Coin,
    PowerUp,
    Star
}
public abstract class Tile_Base : MonoBehaviour, IDamageable
{
    private BoxCollider2D boxCol;
    private SpriteRenderer spriteRenderer;

    [HideInInspector] public bool isActive;
    [SerializeField] private LayerMask enemyLayer;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isActive = true;
    }

    public abstract void OnHit(bool popOut);

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
        CheckAbove();
        if (bounceRoutine == null)
        {
            bounceRoutine = StartCoroutine(StartBounce());
        }
    }

    public void CheckAbove()
    {
        Vector2 boxCastPos = new(boxCol.bounds.center.x, boxCol.bounds.center.y + boxCol.bounds.extents.y);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(boxCastPos, new Vector2(boxCol.size.x, 0.1f), 0, Vector2.zero, 0, enemyLayer);

        foreach (var enemy in hits)
        {
            enemy.collider.GetComponent<IDamageable>().OnHit(true);
        }
    }

    public abstract void SpawnItem();
    private IEnumerator StartBounce()
    {
        Vector3 startPos = transform.position;
        float timer = 0f;
        float duration = 0.05f;
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
