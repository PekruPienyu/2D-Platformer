using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Player player;
    [SerializeField] private LayerMask layerMask;
    public PlayerControllerKeys_SO playerControllerKeys;
    public bool pauseGame;

    [Header("Move")]
    [SerializeField] private float moveSpeed;
    private Vector2 moveDir;
    private float moveAcceleration;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float moveAccelerationValue;
    [SerializeField] private float moveDecelerationStrength;

    [Header("Sprint")]
    [SerializeField] private float maxSprintSpeed;
    [SerializeField] private float sprintAccelerationValue;
    [SerializeField] private float sprintDecelerationStrength;
    private float sprintAcceleration;

    [Header("Jump")]
    [SerializeField] private bool jumpBufferEnable = true;
    [SerializeField] private bool doubleJumpEnable = true;
    [SerializeField] private float jumpBufferDuration;
    [Space]
    [SerializeField] private AnimationCurve jumpForceCurve;
    [SerializeField] private float jumpForce;

    private bool jumpBuffer = false;
    private bool canDoubleJump = true;

    private Coroutine moveSpeedDecelerateRoutine;
    private Coroutine sprintSpeedDecelerateRoutine;
    private KeyCode prevKeyPressed;

    private enum PlayerState
    {
        Jump,
        OnAir,
        Fall,
        Grounded
    }
    private PlayerState currentPlayerState = PlayerState.Fall;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        pauseGame = false;
        playerControllerKeys.left = KeyCode.A;
        playerControllerKeys.right = KeyCode.D;
        playerControllerKeys.jump = KeyCode.Space;
        playerControllerKeys.sprint = KeyCode.LeftShift;
    }

    void Update()
    {
        if (pauseGame) return;
        //Sprint
        if (Input.GetKey(playerControllerKeys.sprint))
        {
            if (sprintSpeedDecelerateRoutine != null) StopCoroutine(sprintSpeedDecelerateRoutine);
            SprintAcceleration();
        }
        else if (Input.GetKeyUp(playerControllerKeys.sprint))
        {
            sprintSpeedDecelerateRoutine = StartCoroutine(DecelerateSprintSpeed());
        }
        //Jump
        if (Input.GetKeyDown(playerControllerKeys.jump))
        {
            if(CanJump())
            {
                Jump();
            }
            CheckJumpBuffer();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
        // Left Right
        if (Input.GetKey(playerControllerKeys.right))
        {
            Move(playerControllerKeys.right, 1);
            if (transform.localScale.x < 0) transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        if (Input.GetKey(playerControllerKeys.left))
        {
            Move(playerControllerKeys.left, -1);
            if (transform.localScale.x > 0) transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        if (Input.GetKeyUp(playerControllerKeys.right) || Input.GetKeyUp(playerControllerKeys.left))
        {
            moveSpeedDecelerateRoutine = StartCoroutine(DecelerateMoveSpeed());
            player.ChangeAnimationToIdle();
        }
        //Checks & Timers
        CheckCoyoteTime();
        JumpBufferTimer();
        GroundCheckBoxCast();
        HeadCheckBoxCast();
        BodyCheckBoxCast();

        moveDir.x = (moveAcceleration + sprintAcceleration);
    }

    [Space]
    [SerializeField] private float maxJumpHeight;
    private bool isJumping;
    private float heightCovered;

    [Header("Air Time")]
    [SerializeField] private bool airTimeEnable;
    [SerializeField] private float airTimeDuration;
    [SerializeField] private float airTimeMoveSpeedAdd;
    private float airTimeTimer;
    private bool onAir;

    [Header("Fall")]
    [SerializeField] private float maxFallSpeed = 5f;



    private void FixedUpdate()
    {
        if (pauseGame) return;
        switch (currentPlayerState)
        {
            case PlayerState.Jump:
                
                if(isJumping) //If Jump key Pressed or max jump height not reached
                {
                    moveDir.y = jumpForce * Time.fixedDeltaTime * 40;
                    heightCovered += moveDir.y;
                }
                else if(!isJumping)
                {
                    moveDir.y = (jumpForce * Time.fixedDeltaTime * 40) - JumpDecelerationGravity();
                }
                if(heightCovered > maxJumpHeight)
                {
                    isJumping = false;
                }
                if (moveDir.y <= 0)
                {
                    ResetJumpVariables();
                    if (airTimeEnable)
                    {
                        onAir = true;
                        currentPlayerState = PlayerState.OnAir;
                    }
                    else currentPlayerState = PlayerState.Fall;
                }
                
                break;
            case PlayerState.OnAir:
                airTimeTimer += Time.deltaTime;
                if (airTimeTimer >= airTimeDuration)
                {
                    onAir = false;
                    airTimeTimer = 0;
                    currentPlayerState = PlayerState.Fall;
                }
                break;
            case PlayerState.Fall:
                ApplyGravityForFall();
                if(isGrounded)
                {
                    player.ChangeAnimationToIdle();
                    ConfigureDoubleJumpVariables();
                    ResetCoyoteTimer();
                    if (jumpBuffer)
                    {
                        moveDir.y = 0;
                        Jump();
                    }
                    else currentPlayerState = PlayerState.Grounded;
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
        if(isCorrectingEdge)
        {
            if(IsTopEdge() || IsBottomEdge())
            {
                IsTopEdge();
                IsBottomEdge();
            }
            else
            {
                isCorrectingEdge = false;
                edgeCorrectionVelocity = Vector2.zero;
            }
        }
        rb.velocity = 50 * moveSpeed * Time.deltaTime * (moveDir + edgeCorrectionVelocity);
    }

    #region Move----------------

    private void Move(KeyCode keyPressed, float direction)
    {
        if ((prevKeyPressed == keyPressed) || (onAir && prevKeyPressed != keyPressed))
        {
            if (moveSpeedDecelerateRoutine != null) StopCoroutine(moveSpeedDecelerateRoutine);
        }
        if (onAir)
        {
            moveAcceleration += (airTimeMoveSpeedAdd * direction) * Time.deltaTime;
        }
        MoveAcceleration(direction);
        prevKeyPressed = keyPressed;
        if (isGrounded) player.ChangeAnimationToRun();
    }

    #endregion

    #region Gravity----------------

    [SerializeField] private float gravityJumpDeceleration;
    [SerializeField] private float gravityFallAcceleration;
    private int gravityJumpDecelerationMultiplier = 0;
    private void ApplyGravityForFall()
    {
        moveDir.y -= gravityFallAcceleration;
        if (moveDir.y < -maxFallSpeed)
        {
            moveDir.y = -maxFallSpeed;
        }
    }

    private float JumpDecelerationGravity()
    {
        gravityJumpDecelerationMultiplier++;
        return gravityJumpDeceleration * gravityJumpDecelerationMultiplier;
    }

    private void ResetJumpDecelerationMultiplier()
    {
        gravityJumpDecelerationMultiplier = 0;
    }
    #endregion

    #region Jump Check And Functions----------------
    private void Jump()
    {
        jumpBuffer = false;
        isJumping = true;
        currentPlayerState = PlayerState.Jump;
        player.ChangeAnimationToJump();
    }

    private bool CanJump()
    {
        if(isGrounded)
        {
            return true;
        }
        else
        {
            if(isCoyoteTime)
            {
                return true;
            }
            else if(canDoubleJump)
            {
                canDoubleJump = false;
                return true;
            }
        }
        return false;
    }

    private void ConfigureDoubleJumpVariables()
    {
        if (doubleJumpEnable) canDoubleJump = true;
        else canDoubleJump = false;
    }

    private void CheckJumpBuffer()
    {
        if (!jumpBufferEnable)
        {
            jumpBuffer = false;
            return;
        }
        if (currentPlayerState == PlayerState.Fall)
        {
            jumpBuffer = true;
        }
    }

    private float jumpBufferTimer;

    private void JumpBufferTimer()
    {
        if(jumpBuffer)
        {
            jumpBufferTimer += Time.deltaTime;
            if(jumpBufferTimer >= jumpBufferDuration)
            {
                jumpBuffer = false;
            }
        }
        else
        {
            jumpBufferTimer = 0;
        }
    }

    private void ResetJumpVariables()
    {
        heightCovered = 0;
        moveDir.y = 0;
        ResetJumpDecelerationMultiplier();
    }

    #endregion

    #region Player Key Change Functions--------------
    public void ChangePlayerKeyLeft(char keyCode)
    {
        playerControllerKeys.left = (KeyCode)keyCode;
    }

    public void ChangePlayerKeyRight(char keyCode)
    {
        playerControllerKeys.right = (KeyCode)keyCode;
    }

    public void ChangePlayerKeyJump(char keyCode)
    {
        playerControllerKeys.jump = (KeyCode)keyCode;
    }

    public void ChangePlayerKeySprint(char keyCode)
    {
        playerControllerKeys.sprint = (KeyCode)keyCode;
    }
    #endregion

    #region Accelerate, Decelerate------------
    private void MoveAcceleration(float dir)
    {
        moveAcceleration += (moveAccelerationValue * dir) * Time.deltaTime * 40;
        if(Mathf.Abs(moveAcceleration) >= maxMoveSpeed)
        {
            moveAcceleration = (maxMoveSpeed * dir);
        }
    }

    private void SprintAcceleration()
    {
        int dir = 0;

        if(moveDir.x > 0)
        {
            dir = 1;
        }
        else if(moveDir.x < 0)
        {
            dir = -1;
        }
        sprintAcceleration += (sprintAccelerationValue * dir) * Time.deltaTime * 40;
        if (Mathf.Abs(sprintAcceleration) > maxSprintSpeed)
        {
            sprintAcceleration = (maxSprintSpeed * dir);
        }
    }

    private IEnumerator DecelerateMoveSpeed()
    {
        while(Mathf.Abs(moveAcceleration) > moveAccelerationValue)
        {
            if(moveAcceleration > 0)
            {
                moveAcceleration -= (moveAccelerationValue * moveDecelerationStrength) * Time.deltaTime * 40;
            }
            else
            {
                moveAcceleration += (moveAccelerationValue * moveDecelerationStrength) * Time.deltaTime * 40;
            }
            yield return null;
        }
        moveAcceleration = 0;
    }


    private IEnumerator DecelerateSprintSpeed()
    {
        while (Mathf.Abs(sprintAcceleration) > sprintAccelerationValue)
        {
            if (sprintAcceleration > 0)
            {
                sprintAcceleration -= (sprintAccelerationValue * sprintDecelerationStrength) * Time.deltaTime * 40;
            }
            else
            {
                sprintAcceleration += (sprintAccelerationValue * sprintDecelerationStrength) * Time.deltaTime * 40;
            }
            yield return null;
        }
        sprintAcceleration = 0;
    }
    #endregion

    #region Checks--- Head, Body, Ground, Edge-----------------
    [Header("Checks")]
    [SerializeField] private bool edgeCorrectionEnable = true;
    [SerializeField] private Vector2 groundCheckSize;
    private bool isCorrectingEdge;
    private bool isGrounded;
    private void GroundCheckBoxCast()
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

    [SerializeField] private Vector2 headCheckSize;
    private void HeadCheckBoxCast()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, headCheckSize, 0, Vector2.zero, 0, layerMask);

        if(hits.Length == 0)
        {
            IsTopEdge();
        }

        for (int i = 0; i < hits.Length; i++)
        {
            if(hits[i].collider.CompareTag("Tile"))
            {
                ResetJumpVariables();
                if (airTimeEnable && currentPlayerState == PlayerState.Jump)
                {
                    currentPlayerState = PlayerState.OnAir;
                }
                else currentPlayerState = PlayerState.Fall;
                hits[i].collider.GetComponent<InteractableTile>().HandleInteraction(player.GetCurrentPower());
                return;
            }
        }
    }

    [SerializeField] private Vector2 bodyCheckSize;

    private void BodyCheckBoxCast()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z), bodyCheckSize, 0, Vector2.zero, 0, layerMask);

        if (hits.Length == 0)
        {
            IsBottomEdge();
        }
    }

    [Header("Top Edge")]
    [Range(0f, 0.175f)]
    [SerializeField] private float topEdgeCheckAreaSlider;
    [SerializeField] private float topEdgeCorrectionValue;
    private Vector2 edgeCorrectionVelocity = Vector2.zero;
    private Vector3 rayCastPosition;
    private bool IsTopEdge()
    {
        if(!edgeCorrectionEnable)
        {
            isCorrectingEdge = false;
        }
        else if (currentPlayerState == PlayerState.Jump && rb.velocity.y == 0)
        {
            rayCastPosition = new Vector3(transform.position.x + topEdgeCheckAreaSlider, transform.position.y, transform.position.z);
            RaycastHit2D rightTopRay = Physics2D.Raycast(rayCastPosition, new Vector3(0, 1, 0), 0.1f, layerMask);
            if(rightTopRay.collider != null)
            {
                edgeCorrectionVelocity = new Vector3(-topEdgeCorrectionValue, 0, 0);
                isCorrectingEdge = true;
                return true;
            }

            rayCastPosition = new Vector3(transform.position.x - topEdgeCheckAreaSlider, transform.position.y, transform.position.z);
            RaycastHit2D leftTopRay = Physics2D.Raycast(rayCastPosition, new Vector3(0, 1, 0), 0.1f, layerMask);
            if (leftTopRay.collider != null)
            {
                edgeCorrectionVelocity = new Vector3(topEdgeCorrectionValue, 0, 0);
                isCorrectingEdge = true;
                return true;
            }
        }
        return false;
    }
    [Header("Bottom Edge")]
    [Range(0, 0.17f)]
    [SerializeField] private float bottomEdgeCheckAreaSlider;
    [SerializeField] private float bottomEdgeCorrectionValue;
    [SerializeField] private float bottomRayYPosition;

    private bool IsBottomEdge()
    {
        if (!edgeCorrectionEnable)
        {
            isCorrectingEdge = false;
        }
        else if (currentPlayerState == PlayerState.Jump && rb.velocity.x == 0)
        {
            rayCastPosition = new Vector3(transform.position.x + bottomEdgeCheckAreaSlider, transform.position.y - bottomRayYPosition, transform.position.z);
            RaycastHit2D rightBottomRay = Physics2D.Raycast(rayCastPosition, new Vector2(1, 0), 0.1f, layerMask);
            if (rightBottomRay.collider != null)
            {
                edgeCorrectionVelocity = new Vector3(-bottomEdgeCheckAreaSlider, bottomEdgeCheckAreaSlider, 0);
                isCorrectingEdge = true;
                return true;
            }

            rayCastPosition = new Vector3(transform.position.x - bottomEdgeCheckAreaSlider, transform.position.y - bottomRayYPosition, transform.position.z);
            RaycastHit2D leftBottomRay = Physics2D.Raycast(rayCastPosition, new Vector2(-1, 0), 0.1f, layerMask);
            if (leftBottomRay.collider != null)
            {
                edgeCorrectionVelocity = new Vector3(bottomEdgeCheckAreaSlider, bottomEdgeCheckAreaSlider, 0);
                isCorrectingEdge = true;
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Coyote Check----------------
    [Header("Coyote")]
    [SerializeField] private bool coyoteTimeActive = true;
    [SerializeField] private float coyoteDuration;
    private float coyoteTimer;
    private bool isCoyoteTime = true;
    private void CheckCoyoteTime()
    {
        if(!coyoteTimeActive)
        {
            isCoyoteTime = false;
            return;
        }
        if(!isGrounded)
        {
            coyoteTimer += Time.deltaTime;
            if(coyoteTimer >= coyoteDuration || currentPlayerState == PlayerState.Jump)
            {
                isCoyoteTime = false;
            }
        }
        else
        {
            isCoyoteTime = true;
        }
    }

    private void ResetCoyoteTimer()
    {
        coyoteTimer = 0;
    }
    #endregion

    #region Gizmos Debugs------------
    private void OnDrawGizmos()
    {
        //Ground Check debug
        Gizmos.color = new(0, 1, 0, 0.6f);
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y -0.5f, transform.position.z), new Vector3(groundCheckSize.x, groundCheckSize.y, 0));
        //Head Check debug
        Gizmos.color = new(0, 1, 0, 0.6f);
        Gizmos.DrawCube(transform.position, new Vector3(headCheckSize.x, headCheckSize.y, 0));
        //Body Check debug
        Gizmos.color = new(0, 1, 0, 0.6f);
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z), new Vector3(bodyCheckSize.x, bodyCheckSize.y, 0));

        //Raycast Top Right Debug
        rayCastPosition = new Vector3(transform.position.x + topEdgeCheckAreaSlider, transform.position.y, transform.position.z);
        Gizmos.color = new(0, 1, 0, 1);
        Gizmos.DrawRay(rayCastPosition, new Vector3(0, 0.1f, 0));

        //Raycast Top Left Debug
        rayCastPosition = new Vector3(transform.position.x - topEdgeCheckAreaSlider, transform.position.y, transform.position.z);
        Gizmos.color = new(0, 1, 0, 1);
        Gizmos.DrawRay(rayCastPosition, new Vector3(0, 0.1f, 0));

        //Raycast Bottom Right Debug
        rayCastPosition = new Vector3(transform.position.x + bottomEdgeCheckAreaSlider, transform.position.y - bottomRayYPosition, transform.position.z);
        Gizmos.color = new(0, 1, 0, 1);
        Gizmos.DrawRay(rayCastPosition, new Vector3(0.1f, 0, 0));

        //Raycast Bottom Left Debug
        rayCastPosition = new Vector3(transform.position.x - bottomEdgeCheckAreaSlider, transform.position.y - bottomRayYPosition, transform.position.z);
        Gizmos.color = new(0, 1, 0, 1);
        Gizmos.DrawRay(rayCastPosition, new Vector3(-0.1f, 0, 0));
    }
    #endregion
}