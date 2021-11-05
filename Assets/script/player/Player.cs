using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    float m_MySliderValue;
    public float slowMotionTimeScale;
    private float startTimeScale;
    private float StartFixedDeltaTime;

    private float timer = 0;

    private SpriteRenderer spriteRenderer;


    private CinemachineFollowZoom cinemachineFollowZoom;





    private Transform playerTransform;
    private Transform point;
    [SerializeField] private GameObject followCamera;
    [SerializeField] private GameObject closeCamera;

    private AnimatorStateInfo animatorStateInfo;
    private bool isDemonTransform;


    private bool switchDemon;
    private bool isDemon;


    private float holdDownStartingTime;

    //color
    public Color colorWhite = Color.white;
    public Color black = Color.black;
    public float duration = 3.0f;

    public Camera camera;

    // game start
    private bool gameStart;


    //movement parameters
    private float inputX;
    private bool isIdle;
    private bool isWalking;
    private bool isRunning;
    private bool isDashing;


    private bool isDemonPunch;
    private bool isDemonPowerPunch;



    private Rigidbody2D rigidbody2D;

    public Direction playerDirection;
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

    public static Player Instance;




    ToolEffect toolEffect = ToolEffect.none;


    //camera
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineTransposer cinemachineTransposer;

    private void Awake()
    {
        Instance = this;
        playerTransform = GetComponent<Transform>();
        point = GameObject.FindGameObjectWithTag(Tags.Point).GetComponent<Transform>();
    }
    private void Start()
    {
        if (!gameStart)
        {
            cinemachineVirtualCamera.Follow = point;
            StartCoroutine(StartGameTransition());
            playerDirection = Direction.right;
        }
        animator = GetComponentInChildren<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        cinemachineVirtualCamera = GameObject.FindGameObjectWithTag(Tags.FollowCamera).GetComponent<CinemachineVirtualCamera>();
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        startTimeScale = Time.timeScale;
        StartFixedDeltaTime = Time.fixedDeltaTime;
    }


    private void Update()
    {
        ResetAnimationTriggers();

        Movement();
        if (Input.GetKeyDown(KeyCode.E) && gameStart)
        {
            DemonTransform();

        }
        if (Input.GetMouseButtonDown(0) && isDemon)
        {

            DemonAnimationController.Instance.NormalAttack();
        }

        if (Input.GetMouseButtonDown(1) && isDemon)
        {

            DemonAnimationController.Instance.SmashAttack();

        }


        EventHandler.CallDemonEvent(isDemonTransform, isDemon);
        //DemonPunch();
        //Attack();
        //PowerAttack();
        // send event to any listeners for player movement input
        EventHandler.CallMovementEvent(inputX, isWalking, isRunning, isDashing, isIdle, toolEffect, isDemonPunch, isDemonPowerPunch, false, false);

    }
    void FixedUpdate()
    {






    }
    // Update is called once per frame

    private void LateUpdate()
    {
        if (animator == null)
        {
            Debug.Log("animator == null");
        }
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.IsTag("demon transform"))
        {
            Debug.Log(animatorStateInfo);
        }
        if (animatorStateInfo.IsName("demon transform") && animatorStateInfo.normalizedTime <= 0.1f)
        {
            Debug.Log("run");
            ShakeCamera.Instance.Shake(10f, 2f);

        }
        if (animatorStateInfo.IsName("demon transform") && animatorStateInfo.normalizedTime >= 0.9f)
        {

            ShakeCamera.Instance.Shake(0f, 0f);

        }
    }

    //public void PowerAttack()
    //{
    //    if (Input.GetMouseButtonDown(1) && isAttacking && isPowerAttacking)
    //    {
    //        isPowerAttacking = true;
    //    }
    //}



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
        //inputX = Input.GetAxisRaw("Horizontal");
        if (inputX != 0)
        {
            inputX = inputX * 0.71f;
        }
        rigidbody2D.velocity = new Vector2(inputX * movementSpeed, rigidbody2D.velocity.y);
        if (inputX != 0)
        {
            isWalking = false;
            isRunning = true;
            isIdle = false;
            movementSpeed = Settings.walkingSpeed;

            // capture player direction for save game
            if (inputX < 0)
            {

                playerDirection = Direction.left;
                spriteRenderer.flipX = true;






            }
            if (inputX > 0)
            {

                playerDirection = Direction.right;
                spriteRenderer.flipX = false;


            }

        }
        else if (inputX == 0)
        {
            Debug.Log("isidle" + isIdle);
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
        while (playerDirection == Direction.left && cinemachineTransposer.m_FollowOffset.x >= -10f)
        {
            Debug.Log("left");
            cinemachineTransposer.m_FollowOffset.x -= 0.1f;
            break;


        }
        while (playerDirection == Direction.right && cinemachineTransposer.m_FollowOffset.x <= 11.8f)
        {
            Debug.Log("right");
            cinemachineTransposer.m_FollowOffset.x += 0.1f;
            break;


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







    private void DemonTransform()
    {









        animator.SetTrigger(Settings.isDemonTransform);

    }


    public void DemonYushanIdle()
    {
        if (inputX < 0)
        {
            Debug.Log("if" + playerDirection);
            playerDirection = Direction.left;
            spriteRenderer.flipX = true;
        }
        else if (inputX > 0)
        {
            Debug.Log("else if" + playerDirection);
            playerDirection = Direction.right;
            spriteRenderer.flipX = false;
        }
        isDemon = true;
        animator.SetBool(Settings.isDemon, isDemon);
        Debug.Log("isdemon" + isDemon);
        Debug.Log("isdemon idle" + playerDirection);
    }

    IEnumerator StartGameTransition()
    {
        yield return new WaitForSeconds(1.0f);
        cinemachineVirtualCamera.Follow = playerTransform;
        cinemachineVirtualCamera.LookAt = playerTransform;
        gameStart = true;
    }


}

