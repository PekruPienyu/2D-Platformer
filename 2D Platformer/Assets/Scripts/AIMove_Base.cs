using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove_Base : MonoBehaviour
{
    private Rigidbody2D rb;
    [HideInInspector] public BoxCollider2D boxCol;
    private Vector2 moveDir;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private LayerMask layerMask;
    private bool isGrounded;
    private bool jumpEnable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        moveDir = Vector2.right;
    }

    private void FixedUpdate()
    {
        UpdateCheckAndMovement();
    }

    private Vector2 jumpVelocity;

    public void EnableJump()
    {
        jumpEnable = true;
    }

    public virtual void UpdateCheckAndMovement()
    {
        CheckAhead();
        GroundCheck();
        ApplyGravity();
        if (isGrounded && jumpEnable)
        {
            jumpVelocity = Vector2.up * 4;
        }
        else if (!jumpEnable) jumpVelocity = Vector2.zero;
        rb.velocity = moveSpeed * Time.fixedDeltaTime * 40 * (moveDir + jumpVelocity);
    }

    public void StopMovement()
    {
        rb.velocity = Vector2.zero;
    }

    public void SetBoxColliderActive(bool isActive)
    {
        boxCol.enabled = isActive;
    }

    private Vector2 rayDir;
    private void CheckAhead()
    {
        rayDir.x = moveDir.x;
        RaycastHit2D ray = Physics2D.Raycast(boxCol.bounds.center, rayDir, boxCol.bounds.extents.x + 0.02f, layerMask);
        Debug.DrawRay(boxCol.bounds.center, new Vector3(moveDir.x * (boxCol.bounds.extents.x + 0.02f), 0,0));

        if (ray.collider != null)
        {
            moveDir *= -1;
        }
    }

    [SerializeField] private float gravityForce = 0.1f;
    [SerializeField] private float maxFallSpeed = 2.5f;
    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            moveDir.y -= gravityForce;
            if(jumpEnable)
            {
                jumpVelocity.y -= gravityForce;
            }
            if(moveDir.y <= -maxFallSpeed)
            {
                moveDir.y = -maxFallSpeed;
            }
        }
        else
        {
            moveDir.y = 0;
        }
    }

    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.07f);
    [SerializeField] private float groundCheckYPos = 0.25f;
    private Vector2 boxCastOrigin;
    private void GroundCheck()
    {
        boxCastOrigin = new Vector2(transform.position.x, transform.position.y - groundCheckYPos);
        RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin, groundCheckSize, 0, Vector2.zero, 0, layerMask);
        if(hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        boxCastOrigin = new Vector2(transform.position.x, transform.position.y - groundCheckYPos);
        Gizmos.color = new(0, 1, 0, 0.6f);
        Gizmos.DrawCube(boxCastOrigin, groundCheckSize);
    }
}
