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
    private bool isJumping;
    private bool onAir;
    public float jumpForce;

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
        if (Input.GetKeyDown(KeyCode.Space) && !onAir)
        {
            anim.Play("Jump");
            isJumping = true;
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
                if (!onAir) anim.Play("Run");
            }
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            {
                if (!onAir) anim.Play("Idle");
            }
        }
        
    }

    float timer = 0f;
    float duration = 1f;

    private void FixedUpdate()
    {
        if (isJumping)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / duration;
            float strength = jumpForceCurve.Evaluate(normalizedTime);
            moveDir.y = jumpForce * strength;
            rb.velocity  = moveDir * Time.deltaTime;
            if (moveDir.y == 0)
            {
                timer = 0;
                isJumping = false;
            }
            onAir = true;
        }

        rb.velocity = moveSpeed * Time.deltaTime * moveDir;
    }

    private void GroundCheck()
    {
        RaycastHit2D ray = Physics2D.Raycast(boxCol.bounds.center, Vector2.down, boxCol.bounds.extents.y + 0.1f, layerMask);

        if (ray.collider == null || !ray.collider.CompareTag("Ground"))
        {
            onAir = true;
        }
        else
        {
            if(onAir)anim.Play("Idle");
            onAir = false;
        }  
    }
}