using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float m_MySliderValue;
    public float slowMotionTimeScale;
    private float startTimeScale;
    private float StartFixedDeltaTime;

    private float timer = 0;

    private SpriteRenderer spriteRenderer;

    //movement parameters
    private float inputX;
    private bool isIdle;
    private bool isWalking;
    private bool isRunning;
    private bool isDashing;


    private bool isDemonPunch;
    private bool isDemonPowerPunch;



    private Rigidbody2D rigidbody2D;

    private Direction playerDirection;
    [SerializeField] private GameObject player;

    private float movementSpeed;

    private bool _playerInputIsDisabled = false;

    public bool PlayerInputIsDisabled
    {
        get => _playerInputIsDisabled;
        set => _playerInputIsDisabled = value;

    }
    [SerializeField]
    private float jumpForce = 5.0f;



    [SerializeField]
    private LayerMask groundLayer;

    private bool resetJump = false;


    public Animator animator;
    public bool comboPossible;
    public int comboStep;
    public bool inputSmash;
    public bool isAttacking = false;
    public static Player Instance;
    public bool isPowerAttacking = false;
    public bool isNormalAttack = false;
    public bool normalAttack = false;
    public bool powerAttacking = false;
    public bool isFinishedAttack = false;
    public bool isFinishedPowerAttack = false;




    ToolEffect toolEffect = ToolEffect.none;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        startTimeScale = Time.timeScale;
        StartFixedDeltaTime = Time.fixedDeltaTime;
    }


    private void Update()
    {
        ResetAnimationTriggers();
        Movement();
        if (Input.GetMouseButtonDown(0))
        {
            DemonAnimationController.demonControllerInstance.NormalAttack();
        }
        if (Input.GetMouseButtonDown(1))
        {
            DemonAnimationController.demonControllerInstance.SmashAttack();
        }
        //Attack();
        //PowerAttack();
        // send event to any listeners for player movement input
        EventHandler.CallMovementEvent(inputX, isWalking, isRunning, isDashing, isIdle, toolEffect, isDemonPunch, isDemonPowerPunch, false, false);

    }

    //public void PowerAttack()
    //{
    //    if (Input.GetMouseButtonDown(1) && isAttacking && isPowerAttacking)
    //    {
    //        isPowerAttacking = true;
    //    }
    //}

    IEnumerator Charged()
    {
        Debug.Log("charging");
        yield return new WaitForSeconds(1f);
        animator.Play("demon power punch");
        animator.speed = 1f;
        isPowerAttacking = false;
    }
    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {

            Debug.Log("isattacking");
            isAttacking = true;
            //if (isAttacking)
            //{
            //    isPowerAttacking = true;
            //    isNormalAttack = true;
            //    if (Input.GetMouseButtonDown(0)&& isNormalAttack)
            //    {
            //        normalAttack = true;
            //        isPowerAttacking = false;
            //    }
            //}




        }
        if (Input.GetMouseButtonDown(1) && isAttacking && !isPowerAttacking)
        {
            Debug.Log("powerattacking");
            powerAttacking = true;
        }
    }
    private void ResetAnimationTriggers()
    {

        isWalking = false;
        isRunning = false;
        isDashing = false;


        isDemonPunch = false;
        isDemonPowerPunch = false;
        toolEffect = ToolEffect.none;
    }

    void Movement()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        if (inputX != 0)
        {
            inputX = inputX * 0.71f;
        }
        rigidbody2D.velocity = new Vector2(inputX * movementSpeed, rigidbody2D.velocity.y);
        if (inputX != 0)
        {
            isWalking = true;
            isRunning = false;
            isIdle = false;
            movementSpeed = Settings.walkingSpeed;

            // capture player direction for save game
            if (inputX < 0)
            {
                playerDirection = Direction.left;
                spriteRenderer.flipX = true;
            }
            else if (inputX > 0)
            {

                playerDirection = Direction.right;
                spriteRenderer.flipX = false;
            }
        }
        else if (inputX == 0)
        {
            isRunning = false;
            isWalking = false;
            isIdle = true;
        }
        // running
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            isRunning = false;
            isWalking = true;
            isIdle = false;
            movementSpeed = Settings.walkingSpeed;
        }
        else
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;
        }


        //jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() == true)
        {
            //jump
            Debug.Log("jump");
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);

            StartCoroutine(ResetJumpNeededRoutine());
        }
    }
    bool IsGrounded()
    {
        //2d raycast to the ground
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, groundLayer.value);



        if (hitInfo.collider != null)
        {
            if (resetJump == false)
            {
                return true;
            }


        }
        return false;
    }
    IEnumerator ResetJumpNeededRoutine()
    {
        resetJump = true;
        yield return new WaitForSeconds(0.1f);
        resetJump = false;
    }


}
