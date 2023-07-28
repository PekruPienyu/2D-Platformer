using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 finalVelocity;
    private Vector3 currentMoveDirection;

    private enum State
    {
        Idle,
        Jump,
        Fall
    }
    private State currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentMoveDirection = Vector3.zero;
        currentState = State.Idle;
    }

    void Update()
    {
       
       
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentMoveDirection.x = -1;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentMoveDirection.x = 1;
        }

        switch (currentState)
        {
            case State.Idle:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }
                GroundCheckBoxCast();
                break;
            case State.Jump:
                if (Input.GetKey(KeyCode.Space) && currentState == State.Jump)
                {
                    Test();
                }
                JumpUpdates();
              
                break;
            case State.Fall:
                GroundCheckBoxCast();
                if (isGrounded) currentState = State.Idle;
                break;
            default:
                break;
        }
        
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Idle:
                break;
            case State.Jump:
                finalVelocity = CalculateFinalJumpDirection();
                break;
            case State.Fall:
                finalVelocity.x = currentMoveDirection.x;
                finalVelocity.y = -1 * gravityForce;
                break;
            default:
                break;
        }
        rb.velocity =  finalVelocity +  GravityCheck();
    }


    [Header("Jump")]
    [SerializeField] private float jumpHeight = 10;
    [SerializeField] private AnimationCurve jumpCurve_UpDirection;
    [SerializeField] private AnimationCurve bonusHeightCurve;
    private float currentJumpHeight;
    private float currentMaxJumpHeight;
    private float startYHeight;
    private float bonusHeightTimer;
    private const float BONUS_HEIGHT_MAXTIME = 1.5f;
   
    
    [SerializeField] private float jumpSpeed = 10;
    private float currentJumpSpeed;

    private Vector2 currentJumpDirection;
    private Vector2 finalJumpDirection;

    private void Jump()
    {
        currentState = State.Jump;
        currentJumpSpeed = jumpSpeed;
        currentJumpDirection.x = currentMoveDirection.x;
        currentJumpDirection.y = 1;
        currentJumpHeight = 0;
        currentMaxJumpHeight = jumpHeight;
        startYHeight = transform.position.y;
        bonusHeightTimer = 0;
    }

    private void Test()
    {
        bonusHeightTimer += Time.deltaTime;
        if(bonusHeightTimer>BONUS_HEIGHT_MAXTIME)
        {
            bonusHeightTimer = BONUS_HEIGHT_MAXTIME;
        }
    }

    private void JumpUpdates()
    {
        if (currentJumpHeight > jumpHeight)
        {
            currentState = State.Fall;

        }
        else
        {
            float bonusHeight = bonusHeightCurve.Evaluate(bonusHeightTimer/BONUS_HEIGHT_MAXTIME);
            currentJumpHeight = transform.position.y - startYHeight;
            currentMaxJumpHeight += bonusHeight;
            float _normalizedJumpHeight = currentJumpHeight / currentMaxJumpHeight;
            currentJumpSpeed = jumpSpeed - (jumpCurve_UpDirection.Evaluate(_normalizedJumpHeight) * jumpSpeed) ;
        }
    }

    private Vector3 CalculateFinalJumpDirection()
    {
        currentJumpDirection = currentJumpDirection.normalized;
        finalJumpDirection.x = currentJumpDirection.x;
        finalJumpDirection.y = currentJumpSpeed * currentJumpDirection.y;
        return finalJumpDirection;
    }




    #region GroundCheck
    [SerializeField] private float gravityForce = 8.5f;
    private bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckSize;
    private void GroundCheckBoxCast()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), groundCheckSize, 0, Vector2.zero, 0, groundLayer);

        if (hits.Length == 0)
        {
            isGrounded = false;
        }
        else
        {         
            isGrounded = true;
        }
    }

    private Vector3 GravityCheck()
    {
        if(!isGrounded && currentState!=State.Jump)
        {
            return Vector3.down* gravityForce;
        }
        else
        {
            return Vector3.zero;
        }
    }

    #endregion
}
