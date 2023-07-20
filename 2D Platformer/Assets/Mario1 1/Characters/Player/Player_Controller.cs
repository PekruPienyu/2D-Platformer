using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    public Player player;
    public LayerMask layerMask;
    public AnimationCurve jumpForceCurve;

    private bool isGrounded;
    private bool isCoyoteTime = true;

    public float moveSpeed;
    private Vector3 moveDir;
    private float moveAcceleration;
    public float maxMoveAcceleration;
    public float addToMoveAcceleration;

    //-------- Min And Max Jump Variables-----------
    public float minJumpMultiplier;
    public float maxJumpMultiplier;
    private float jumpMultiplier;
    public float addToJumpMultiplier;

    private Coroutine decelerateRoutine;

    private enum PlayerState
    {
        Jump,
        Fall,
        Grounded
    }
    private PlayerState currentPlayerState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //Application.targetFrameRate = 20;
    }

    private void Start()
    {
        jumpMultiplier = minJumpMultiplier;
    }

    void Update()
    {
        CheckCoyoteTime();
        moveDir.x = Input.GetAxis("Horizontal") + moveAcceleration;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(decelerateRoutine!=null)StopCoroutine(decelerateRoutine);
            AccelerateMoveSpeed(moveDir.x);
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            decelerateRoutine = StartCoroutine(DecelerateMoveSpeed());
        }
        GroundCheck();
        HeadCheck();
        if (Input.GetKeyDown(KeyCode.Space) && isCoyoteTime)
        {
            jumpTimer = 0;
            currentPlayerState = PlayerState.Jump;
        }
        if(Input.GetKey(KeyCode.Space))
        {
            jumpMultiplier += addToJumpMultiplier;

            if(jumpMultiplier > maxJumpMultiplier)
            {
                jumpMultiplier = maxJumpMultiplier;
            }
        }
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
            if(isGrounded)player.ChangeAnimationToRun();
        }
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            player.ChangeAnimationToIdle();
        }
    }

    float jumpTimer = 0f;

    public float jumpSpeed = 5;
    public float fallAcceleration = 1;
    public float maxFallSpeed = 5;
    public float jumpDuration = 0.5f;

    private void FixedUpdate()
    {
        switch (currentPlayerState)
        {
            case PlayerState.Jump:
                player.ChangeAnimationToJump();

                jumpTimer += Time.deltaTime;
                float normalizedTime = jumpTimer / jumpDuration;
                float strength = jumpForceCurve.Evaluate(normalizedTime);

                moveDir.y = (jumpSpeed * jumpMultiplier) * strength;
                if (jumpTimer > jumpDuration)
                {
                    jumpTimer = 0;
                    moveDir.y = 0;
                    currentPlayerState = PlayerState.Fall;
                }
                break;
            case PlayerState.Fall:
                moveDir.y -= fallAcceleration;
                if(moveDir.y < -maxFallSpeed)
                {
                    moveDir.y = -maxFallSpeed;
                }
                if(isGrounded)
                {
                    player.ChangeAnimationToIdle();
                    isCoyoteTime = true;
                    jumpMultiplier = minJumpMultiplier;
                    currentPlayerState = PlayerState.Grounded;
                }
                break;
            case PlayerState.Grounded:
                moveDir.y = 0;
                if (!isGrounded)
                {
                    currentPlayerState = PlayerState.Fall;
                }
                break;
            default:
                break;
        }

        rb.velocity = moveSpeed * moveDir * Time.deltaTime * 50;
    }

    private void AccelerateMoveSpeed(float dir)
    {
        moveAcceleration += (addToMoveAcceleration * dir);
        if(Mathf.Abs(moveAcceleration) >= maxMoveAcceleration)
        {
            if(moveAcceleration > 0)
            {
                moveAcceleration = maxMoveAcceleration;
            }
            else
            {
                moveAcceleration = -maxMoveAcceleration;
            }
        }
    }

    private IEnumerator DecelerateMoveSpeed()
    {
        while(Mathf.Abs(moveAcceleration) > addToMoveAcceleration)
        {
            if(moveAcceleration > 0)
            {
                moveAcceleration -= addToMoveAcceleration;
            }
            else
            {
                moveAcceleration += addToMoveAcceleration;
            }
            yield return null;
        }
        moveAcceleration = 0;
    }

    public Vector2 groundCheckSize;
    private void GroundCheck()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), groundCheckSize, 0, Vector2.zero, 0, layerMask);

        if(hits.Length == 0)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
    }

    public Vector2 headCheckSize;
    private void HeadCheck()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, headCheckSize, 0, Vector2.zero, 0);

        for (int i = 0; i < hits.Length; i++)
        {
            if(hits[i].collider.CompareTag("Tile"))
            {
                jumpTimer = jumpDuration;
                hits[i].collider.GetComponent<InteractableTile>().HandleInteraction(player.GetCurrentPower());
                Debug.Log("Tile");
                return;
            }
        }
    }

    private float coyoteTimer;
    private void CheckCoyoteTime()
    {
        if(!isGrounded)
        {
            coyoteTimer += Time.deltaTime;
            if(coyoteTimer >= 0.3f)
            {
                coyoteTimer = 0;
                isCoyoteTime = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        //Ground Check debug
        Gizmos.color = new(0, 1, 0, 0.6f);
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y -0.5f, transform.position.z), new Vector3(groundCheckSize.x, groundCheckSize.y, 0));
        //Head Check debug
        Gizmos.color = new(0, 1, 0, 0.6f);
        Gizmos.DrawCube(transform.position, new Vector3(headCheckSize.x, headCheckSize.y, 0));
    }
}