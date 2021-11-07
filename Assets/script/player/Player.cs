using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{

    public float slowMotionTimeScale;
    public float demonPowerChargeTimer = 0f;




    private SpriteRenderer spriteRenderer;
    // demonanimationcontroller
    public DemonAnimationController demonAnimationController;

    private CinemachineFollowZoom cinemachineFollowZoom;

    //demon power punch
    public Vector2 targetPosition;//where you want the gameobject to move to
    private Vector2 currentVelocity = Vector2.zero;//this is used inside the function dont touch
    private float smoothing = 0.5f;
    private float maxSpeed = 10f;
    public float acceleration = 5.0f;
    public float maxSpeedLeft = 10f;
    private float curSpeed = 0.0f;


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
    private bool isSprinting;

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

    public bool canRun = false;
    public bool canSprint = false;



    ToolEffect toolEffect = ToolEffect.none;


    //camera
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineTransposer cinemachineTransposer;

    private void Awake()
    {
        Instance = this;
        playerTransform = GetComponent<Transform>();
        point = GameObject.FindGameObjectWithTag(Tags.Point).GetComponent<Transform>();
        demonAnimationController = GetComponentInChildren<DemonAnimationController>();
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

    }


    private void Update()
    {
        ResetAnimationTriggers();

        Movement();
        if (Input.GetKeyDown(KeyCode.E) && gameStart)
        {
            DemonTransform();

        }
        if (Input.GetKeyDown(KeyCode.R) && gameStart)
        {


        }



        EventHandler.CallDemonEvent(isDemonTransform, isDemon);
        //DemonPunch();
        //Attack();
        //PowerAttack();
        // send event to any listeners for player movement input
        EventHandler.CallMovementEvent(inputX, isWalking, isRunning, isSprinting, isDashing, isIdle, toolEffect, isDemonPunch, isDemonPowerPunch, false, false);

    }
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && isDemon)
        {

            DemonAnimationController.Instance.NormalAttack();
        }
        if (Input.GetMouseButton(1) && isDemon)
        {
            Debug.Log("mouse down");
            demonPowerChargeTimer += Time.deltaTime;
            if (demonPowerChargeTimer > 2)
            {
                acceleration = 5f;
                maxSpeedLeft = 10f;
                Debug.Log("demonpowercjargetimer>2");
                DemonAnimationController.Instance.SmashAttack();


                if (Time.timeScale == 1f)
                {
                    Time.timeScale = 0.5f;
                }
                else if (animatorStateInfo.IsName(Tags.DemonPowerPunch) && animatorStateInfo.normalizedTime >= 0.6f)
                {
                    Time.timeScale = 1f;
                    Debug.Log("animatorstateinfo demon power punch");
                }


                demonPowerChargeTimer = 0;
            }
        }
        if (Input.GetMouseButtonUp(1) && isDemon && (demonPowerChargeTimer > 2))
        {
            Debug.Log("mouse up power");
            acceleration = 5f;
            maxSpeedLeft = 10f;

            if (Time.timeScale == 1f)
            {
                Time.timeScale = 0.5f;
            }
            else if (animatorStateInfo.IsName(Tags.DemonPowerPunch) && animatorStateInfo.normalizedTime >= 0.6f)
            {
                Time.timeScale = 1f;
                Debug.Log("animatorstateinfo demon power punch");
            }


            demonPowerChargeTimer = 0;
        }
        if (Input.GetMouseButtonUp(1) && isDemon && (demonPowerChargeTimer < 2))
        {
            Debug.Log("mouse up no power");
            acceleration = 2.5f;
            maxSpeedLeft = 5f;
            DemonAnimationController.Instance.SmashAttack();
            demonPowerChargeTimer = 0;
        }








    }
    // Update is called once per frame

    private void LateUpdate()
    {


        if (animator == null)
        {
            Debug.Log("animator == null");
        }
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //annimatorstateinfo to get demon transform and trigger shake func
        if (animatorStateInfo.IsName("demon transform") && animatorStateInfo.normalizedTime <= 0.1f)
        {
            Debug.Log("run");
            ShakeCamera.Instance.Shake(10f, 2f);

        }
        if (animatorStateInfo.IsName("demon transform") && animatorStateInfo.normalizedTime >= 0.9f)
        {

            ShakeCamera.Instance.Shake(0f, 0f);

        }
        //demon power punch animatorstateinfo
        while (playerDirection == Direction.left)
        {
            Debug.Log("left" + animatorStateInfo.IsName("demon power punch"));
            if (animatorStateInfo.IsName(Tags.DemonPowerPunch))
            {
                //targetPosition = Vector2.left * -10f * Time.deltaTime;

                Debug.Log("move left" + animatorStateInfo.IsName("demon power punch"));
                //playerTransform.position = Vector2.SmoothDamp(playerTransform.position, targetPosition, ref currentVelocity, smoothing, maxSpeed);
                playerTransform.Translate(Vector2.left * curSpeed * Time.deltaTime);
                curSpeed += acceleration * Time.deltaTime;
                Debug.Log("curspeed" + curSpeed + "" + acceleration);
                if (curSpeed > maxSpeedLeft)
                {
                    Debug.Log("curspeed > maxspeedleft" + curSpeed);
                    curSpeed = maxSpeedLeft;
                }
                //playerTransform.Translate(Vector2.left * 10f * Time.deltaTime);

            }
            break;
        }

        while (playerDirection == Direction.right)
        {
            Debug.Log("right" + animatorStateInfo.IsName("demon power punch"));
            if (animatorStateInfo.IsName(Tags.DemonPowerPunch))
            {
                Debug.Log("move right" + animatorStateInfo.IsName(Tags.DemonPowerPunch));
                playerTransform.Translate(Vector2.right * curSpeed * Time.deltaTime);
                curSpeed += acceleration * Time.deltaTime;
                Debug.Log("curspeed" + curSpeed + "" + acceleration);
                if (curSpeed > maxSpeedLeft)
                {
                    Debug.Log("curspeed > maxspeedleft" + curSpeed);
                    curSpeed = maxSpeedLeft;
                }
                //playerTransform.Translate(Vector2.right * 10f * Time.deltaTime);
            }
            break;
        }
        if (animatorStateInfo.IsName(Tags.DemonPowerPunch) && animatorStateInfo.normalizedTime <= 0.5f)
        {
            demonAnimationController.IsElectric();
            Debug.Log("stateinfo" + animatorStateInfo.IsName("demon power punch"));
        }
        else if (animatorStateInfo.IsName(Tags.DemonPowerPunch) && animatorStateInfo.normalizedTime >= 0.6f)
        {

            demonAnimationController.IsNotElectric();
        }

    }





    private void ResetAnimationTriggers()
    {

        isWalking = false;
        isRunning = false;
        isDashing = false;
        isSprinting = false;

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
            Debug.Log("if isrunning" + isRunning);
            isSprinting = false;
            isWalking = false;
            isRunning = true;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;
            StartCoroutine(SprintingTime());

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
            isSprinting = false;
            isIdle = true;


        }
        // running
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            isSprinting = false;
            isRunning = false;
            isWalking = true;
            isIdle = false;
            movementSpeed = Settings.walkingSpeed;
        }
        else
        {
            Debug.Log("else runinnig");
            isSprinting = false;
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;
            StartCoroutine(SprintingTime());
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
    private void Run()
    {
        canRun = true;
        isRunning = true;
        if (canRun)
        {
            isSprinting = false;
            movementSpeed = Settings.runningSpeed;
            StartCoroutine(SprintingTime());
        }


    }
    private void Sprint()
    {
        canSprint = true;
        isSprinting = true;
        if (canSprint)
        {
            Debug.Log("cansprint in void sprint");
            isRunning = false;
            movementSpeed = Settings.sprintSpeed;
        }
        else if (canSprint == false)
        {
            Debug.Log("sprint can sprint === false is run");
            isSprinting = false;

            Run();
        }

    }
    IEnumerator SprintingTime()
    {
        yield return new WaitForSeconds(2.5f);
        Sprint();
        yield return new WaitForSeconds(2.5f);
        canSprint = false;
        Debug.Log("sprinting" + isSprinting);
        if (inputX == 0)
        {
            Debug.Log("resetmovement");
            ResetMovement();
        }
    }
    private void ResetMovement()
    {
        isRunning = false;
        isSprinting = false;
        isWalking = false;
        isIdle = true;
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

