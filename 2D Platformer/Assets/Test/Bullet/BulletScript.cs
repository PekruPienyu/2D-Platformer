using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private BoxCollider2D boxCol;
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 moveDir;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float bounceDuration;
    [SerializeField] private float activeDuration;

    [SerializeField] private float gravityForce;
    [SerializeField] private float maxGravityForce;

    private Vector2 bounceVelocity;
    private float activeTimer;
    private bool isActive = false;

    private bool isGrounded;
    private bool isExploding;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    public void Initialize(Vector3 playerPos, Vector2 _moveDir)
    {
        if(_moveDir.x != 0)
            moveDir.x *= _moveDir.x;
        moveDir.y = 0;
        if(moveDir.x > 0)
        {
            bounceVelocity = Vector2.right;
        }
        else
        {
            bounceVelocity = Vector2.left;
        }
        transform.position = playerPos;
        isActive = true;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        if (isExploding) return;
        if (isActive)
            rb.velocity = moveSpeed * (moveDir + bounceVelocity);
    }

    private void Update()
    {
        CheckAhead();
        if (isActive)
        {
            activeTimer += Time.deltaTime;
            if(activeTimer >= activeDuration)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void GroundCheck()
    {
        ApplyGravity();
        Vector2 origin = new(boxCol.bounds.center.x, boxCol.bounds.center.y - boxCol.bounds.extents.y);
        RaycastHit2D hit = Physics2D.BoxCast(origin, new Vector2(boxCol.size.x, 0.3f), 0, Vector2.zero, 0, groundLayer);

        if(hit.collider != null)
        {
            isGrounded = true;
            Bounce();
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            anim.Play("BulletHit");
            moveDir = Vector2.zero;
            rb.velocity = Vector2.zero;
            collision.GetComponentInParent<IDamageable>().OnHit(true);
        }
    }

    private void CheckAhead()
    {
        RaycastHit2D ray = Physics2D.Raycast(boxCol.bounds.center, moveDir.normalized, 0.2f, groundLayer);
        Debug.DrawRay(boxCol.bounds.center, new Vector3(moveDir.normalized.x * 0.2f, 0, 0));

        if (ray.collider == null) return;
        if (ray.collider.CompareTag("Obstacle"))
        {
            anim.Play("BulletHit");
            moveDir = Vector2.zero;
            rb.velocity = Vector2.zero;
        }
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            moveDir.y -= gravityForce;
            if(moveDir.y <= -maxGravityForce)
            {
                moveDir.y = -maxGravityForce;
            }
        }
        else
        {
            moveDir.y = 0;
        }
    }

    private void Bounce()
    {
        bounceVelocity = Vector2.up;
    }

    public void DisableBoxCollider()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void Explode()
    {
        isExploding = true;
        rb.velocity = Vector2.zero;
    }
}