using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove_Base : MonoBehaviour
{
    private Rigidbody2D rb;
    protected BoxCollider2D boxCol;
    private Vector2 moveDir;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private LayerMask rayAheadLayer;
    private bool isGrounded;
    private bool jumpEnable;
    private Vector2 localScaleReference;

    private bool friendlyFire;
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        localScaleReference = transform.localScale;
        FlipSprite();
    }

    private void Update()
    {
        UpdateCheckUpdate();
    }

    private void FixedUpdate()
    {
        FixedUpdateMovementUpdate();
    }

    private Vector2 jumpVelocity;

    public void EnableJump()
    {
        jumpEnable = true;
    }

    public virtual void FixedUpdateMovementUpdate()
    {
        if (!isDead) GroundCheck();
        ApplyGravity();
        if (isGrounded && jumpEnable)
        {
            jumpVelocity = Vector2.up * 3;
        }
        else if (!jumpEnable) jumpVelocity = Vector2.zero;
        rb.velocity = moveSpeed * Time.fixedDeltaTime * 40 * (moveDir + jumpVelocity);
    }

    public virtual void UpdateCheckUpdate()
    {
        CheckAhead();
    }

    public void SetFriendlyFire(bool active)
    {
        friendlyFire = active;
    }

    public bool IsFriendyFireActive()
    {
        return friendlyFire;
    }

    private void FlipSprite()
    {
        if (moveDir.x > 0)
        {
            transform.localScale = new Vector2(-localScaleReference.x, localScaleReference.y);
        }
        else if (moveDir.x < 0)
        {
            transform.localScale = localScaleReference;
        }
    }

    public void StopMovement()
    {
        rb.velocity = Vector2.zero;
    }

    public void SetBoxColliderActive(bool isActive)
    {
        boxCol.enabled = isActive;
    }

    public void SetInitialMoveDirection(Vector2 direction)
    {
        moveDir = direction;
    }

    public void SetMoveDirStrength(int strength)
    {
        moveDir.x = strength;
    }

    public void SetMoveDirDefault()
    {
        if (moveDir.x > 0)
        {
            moveDir.x = 1;
        }
        else
        {
            moveDir.x = -1;
        }
    }

    public void PopOffScreenConfigure(Vector2 direction)
    {
        moveDir = direction;
        isDead = true;
        isGrounded = false;
    }

    private Vector2 rayDir;
    private void CheckAhead()
    {
        if (isDead) return;
        if (moveDir.x != 0) rayDir.x = Mathf.Abs(moveDir.x) / moveDir.x;
        RaycastHit2D[] rays = Physics2D.RaycastAll(boxCol.bounds.center, rayDir, boxCol.bounds.extents.x + 0.02f, rayAheadLayer);
        Debug.DrawRay(boxCol.bounds.center, new Vector3(rayDir.x * (boxCol.bounds.extents.x + 0.02f), 0, 0));

        for (int i = 0; i < rays.Length; i++)
        {
            if (friendlyFire)
            {
                if (rays[i].collider.gameObject == gameObject || rays[i].collider.CompareTag("Enemy")) continue;
                else moveDir.x *= -1;
                FlipSprite();
            }
            else
            {
                if (rays[i].collider.gameObject == gameObject) continue;
                else moveDir.x *= -1;
                FlipSprite();
            }
        }
    }

    [SerializeField] private float gravityForce = 0.1f;
    [SerializeField] private float maxFallSpeed = 2.5f;
    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            moveDir.y -= gravityForce;
            if (jumpEnable)
            {
                jumpVelocity.y -= gravityForce;
            }
            if (moveDir.y <= -maxFallSpeed)
            {
                moveDir.y = -maxFallSpeed;
            }
        }
        else
        {
            moveDir.y = 0;
        }
    }

    [SerializeField] private LayerMask groundLayer;
    private Vector2 boxCastOrigin;
    private void GroundCheck()
    {
        boxCastOrigin = new Vector2(boxCol.bounds.center.x, boxCol.bounds.center.y - boxCol.bounds.extents.y);
        RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin, new Vector2(boxCol.size.x, 0.07f), 0, Vector2.zero, 0, groundLayer);
        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
