using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D boxCol;
    public LayerMask layerMask;
    public AnimationCurve jumpForceCurve;

    public float moveSpeed;
    private Vector3 moveDir;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
    }


    void Update()
    {
        moveDir.x = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveDir.x *= 1.5f;
        }
        else
        {
            moveDir.x = Input.GetAxis("Horizontal");
        }
        GroundCheck();
        if (Input.GetKeyDown(KeyCode.Space) && currentJumpState==PlayerState.Grounded)
        {
            anim.Play("Jump");
            timer = 0;
            currentJumpState = PlayerState.Jump;
        }
        else
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                if (moveDir.x > 0)
                {
                    transform.localScale = new Vector2(1, 1);
                }
                else
                {
                    transform.localScale = new Vector2(-1, 1);

                }
               // if (!onAir) anim.Play("Run");
            }
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            {
                //if (!onAir) anim.Play("Idle");
            }
        }
        
    }

    float timer = 0f;
    float duration = 1f;

    public float jumpSpeed = 5;
    public float fallAcceleration =1;
    public float maxFallSpeed = 5;
    public float jumpDuration = 0.5f;

    private enum PlayerState
    {
        Jump,
        Fall,
        Grounded
    }
    private PlayerState currentJumpState;

    private void FixedUpdate()
    {
        switch (currentJumpState)
        {
            case PlayerState.Jump:
                timer += Time.deltaTime;
                float normalizedTime = timer / jumpDuration;
                float strength = jumpForceCurve.Evaluate(normalizedTime);
                moveDir.y = jumpSpeed * strength;
                if (timer > jumpDuration)
                {
                    timer = 0;
                    moveDir.y = 0;
                    currentJumpState = PlayerState.Fall;
                }
                break;
            case PlayerState.Fall:
                moveDir.y -= fallAcceleration;
                if(moveDir.y < maxFallSpeed)
                {
                    moveDir.y = -maxFallSpeed;
                }
                if(isGrounded)
                {
                    currentJumpState = PlayerState.Grounded;
                }
                break;
            case PlayerState.Grounded:      
                moveDir.y = 0;
                if (!isGrounded)
                {
                    currentJumpState = PlayerState.Fall;
                }
                break;
            default:
                break;
        }

        //moveDir.y = rb.velocity.y;
        rb.velocity = moveSpeed * moveDir;
    }

    private void GroundCheck()
    {
        RaycastHit2D ray = Physics2D.Raycast(boxCol.bounds.center, Vector2.down, boxCol.bounds.extents.y + 0.1f, layerMask);
        if (ray.collider == null || !ray.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }  
    }
}