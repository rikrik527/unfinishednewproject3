using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
public class Player : MonoBehaviour
{

    public float slowMotionTimeScale;
    public const float demonPowerChargeDuration = 1f;
    public float demonPowerChargeTimer = 0f;
    public float demonPowerChargeTimer2 = 0f;
    public float speedFactor = 2.3f;


    public bool isStanding;



    public bool canMove = false;

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
    public float curSpeed = 0.0f;


    private Transform playerTransform;
    private Transform point;
    [SerializeField] private GameObject followCamera;
    [SerializeField] private GameObject closeCamera;
    [SerializeField] private GameObject closeCameraRight;

    public AnimatorStateInfo animatorStateInfo;
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
    private bool isDemonPowerCharge;
    private bool isDemonPunch;
    private bool isDemonPowerPunch;
    private bool isDemonIdle;
    private bool isDemonSecondPunch;
    private bool isDemonPowerPunch2;
    private bool isDemonPowerCharge2;

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
        if (Input.GetMouseButtonDown(0) && isDemon && !isDemonPowerCharge && !isDemonPowerCharge2)
        {

            Debug.Log(isDemonPowerCharge + "first click");
            DemonAnimationController.Instance.NormalAttack();
            canMove = true;


        }
        ChargeAttackAndInputGetMouse();

        EventHandler.CallDemonEvent(isDemonTransform, isDemon, isDemonPowerCharge, isDemonPowerPunch, isDemonPunch, isDemonSecondPunch, isDemonIdle, isDemonPowerPunch2, isDemonPowerCharge2);
        //DemonPunch();
        //Attack();
        //PowerAttack();
        // send event to any listeners for player movement input
        EventHandler.CallMovementEvent(inputX, isWalking, isRunning, isSprinting, isDashing, isIdle, toolEffect, false, false);

    }
    void FixedUpdate()
    {
        //ChargeAttackAndInputGetMouse2();







        //annimatorstateinfo to get demon transform and trigger shake func
        DemonGroundShake();

        // charge input mouse up
        //ChargeAttackAndInputMouseUp();

        //demon power punch animatorstateinfo
        DemonPowerPunchMovePower();

        ElectricEffectAfterDemonPowerPunch();

        if (animatorStateInfo.IsName("demon power punch"))
        {
            Debug.Log("isname demon power punch");

        }





    }
    // Update is called once per frame

    private void LateUpdate()
    {
        if (animator != null)
        {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            Debug.Log(animatorStateInfo + "animatorstateinfo");
        }
    }

    public void ChargeAttackAndInputGetMouse()
    {

        if (Input.GetMouseButtonDown(1) && isDemon && animatorStateInfo.IsName(Tags.DemonPunch) && animatorStateInfo.normalizedTime <= 0.666 && !isDemonPowerCharge)
        {
            Debug.Log("moudedown first click");
            demonPowerChargeTimer = 0;

            isDemonPowerCharge = true;
        }

        if (Input.GetMouseButton(1) && isDemon && animatorStateInfo.IsName(Tags.DemonPowerCharge) && isDemonPowerCharge)
        {

            Debug.Log("buttondown(1)");
            demonPowerChargeTimer += Time.deltaTime;



            demonAnimationController.IsElectric();


            if (demonPowerChargeTimer >= demonPowerChargeDuration)
            {
                isDemonPowerCharge = false;
                if (isDemonPowerCharge == false)
                {
                    demonAnimationController.IsNotElectric();
                    Debug.Log("button if" + isDemonPowerCharge);
                    acceleration = 5f;
                    maxSpeedLeft = 10f;


                    DemonAnimationController.Instance.SmashAttack();

                    demonPowerChargeTimer = 0;
                    Debug.Log(isDemonPowerCharge + "isdemoncharge");
                }

            }

        }
        if (Input.GetMouseButtonUp(1) && isDemon && (demonPowerChargeTimer < demonPowerChargeDuration) && isDemonPowerCharge)
        {
            isDemonPowerCharge = false;

            DemonAnimationController.Instance.IsNotElectric();
            Debug.Log("mouse up no power" + isDemonPowerCharge);
            acceleration = 2.5f;
            maxSpeedLeft = 5f;


            DemonAnimationController.Instance.SmashAttack();
            demonPowerChargeTimer = 0;
            Debug.Log("mouseup called" + isDemonPowerCharge);

        }

        //charge attack 2
        if (Input.GetMouseButtonDown(1) && isDemon && animatorStateInfo.IsName(Tags.DemonSecondPunch) && animatorStateInfo.normalizedTime <= 0.666 && !isDemonPowerCharge2)
        {

            demonPowerChargeTimer2 = 0;

            isDemonPowerCharge2 = true;
            Debug.Log("moudedown power2 first click" + isDemonPowerCharge2);
        }

        if (Input.GetMouseButton(1) && isDemon && animatorStateInfo.IsName(Tags.DemonPowerCharge2) && isDemonPowerCharge2)
        {

            Debug.Log("buttondown(1)power2");
            demonPowerChargeTimer2 += Time.deltaTime;



            demonAnimationController.IsElectric();


            if (demonPowerChargeTimer2 >= demonPowerChargeDuration)
            {
                isDemonPowerCharge2 = false;
                if (isDemonPowerCharge2 == false)
                {
                    demonAnimationController.IsNotElectric();
                    Debug.Log("button if p2" + isDemonPowerCharge2);
                    acceleration = 10f;
                    maxSpeedLeft = 15f;


                    DemonAnimationController.Instance.SmashAttack();

                    demonPowerChargeTimer2 = 0;
                    Debug.Log(isDemonPowerCharge2 + "isdemoncharge2");
                }

            }
        }
        if (Input.GetMouseButtonUp(1) && isDemon && (demonPowerChargeTimer2 < demonPowerChargeDuration) && isDemonPowerCharge2)
        {
            isDemonPowerCharge2 = false;

            DemonAnimationController.Instance.IsNotElectric();
            Debug.Log("mouse up no power2" + isDemonPowerCharge2);
            acceleration = 2.5f;
            maxSpeedLeft = 5f;


            DemonAnimationController.Instance.SmashAttack();
            demonPowerChargeTimer2 = 0;
            Debug.Log("mouseup called power2" + isDemonPowerCharge2);

        }
    }

    private void ChargeAttackAndInputMouseUp()
    {

        //if (Input.GetMouseButtonUp(1) && isDemon && (demonPowerChargeTimer >= demonPowerChargeDuration) && animatorStateInfo.IsName(Tags.DemonPowerCharge) && isDemonPowerCharge)
        //{

        //    isDemonPowerCharge = false;

        //    DemonAnimationController.Instance.IsNotElectric();
        //    Debug.Log("mouse up no power" + isDemonPowerCharge);
        //    acceleration = 5f;
        //    maxSpeedLeft = 10f;


        //    DemonAnimationController.Instance.SmashAttack();

        //    Debug.Log("charge full up called" + isDemonPowerCharge);
        //    demonPowerChargeTimer = 0;

        //}






        //    if (Input.GetMouseButton(1) && isDemon && animatorStateInfo.IsName("demon idle") && isStanding && !isDemonPowerCharge)
        //    {
        //        isDemonPowerCharge = true;

        //        if (isDemonPowerCharge)
        //        {
        //            demonAnimationController.IsElectric();
        //            Debug.Log("charging" + demonPowerChargeTimer + isDemonPowerCharge);
        //            demonPowerChargeTimer += Time.deltaTime * speedFactor;
        //            if (demonPowerChargeTimer > 1f)
        //            {
        //                Debug.Log("smash2" + isDemonPowerCharge);
        //                isDemonPowerCharge = false;
        //                demonAnimationController.IsNotElectric();
        //                acceleration = 5f;
        //                maxSpeedLeft = 10f;
        //                DemonAnimationController.Instance.SmashAttack();
        //                demonPowerChargeTimer = 0;
        //            }
        //            if (demonPowerChargeTimer < 1)
        //            {
        //                Debug.Log("smash" + isDemonPowerCharge);
        //                isDemonPowerCharge = false;
        //                demonAnimationController.IsNotElectric();
        //                acceleration = 2.5f;
        //                maxSpeedLeft = 5f;
        //                DemonAnimationController.Instance.SmashAttack();
        //                demonPowerChargeTimer = 0;
        //                StartCoroutine(IsStanding());
        //            }
        //        }


        //    }
        //    if (Input.GetMouseButtonUp(1) && isDemon && animatorStateInfo.IsName("demon idle") && demonPowerChargeTimer > 1 && isStanding && isDemonPowerCharge)
        //    {
        //        Debug.Log("smash" + isDemonPowerCharge);
        //        isDemonPowerCharge = false;
        //        demonAnimationController.IsNotElectric();
        //        acceleration = 5f;
        //        maxSpeedLeft = 10f;
        //        DemonAnimationController.Instance.SmashAttack();
        //        demonPowerChargeTimer = 0;
        //        StartCoroutine(IsStanding());
        //    }
        //    if (Input.GetMouseButtonUp(1) && isDemon && animatorStateInfo.IsName("demon idle") && demonPowerChargeTimer < 1 && isStanding && isDemonPowerCharge)
        //    {
        //        Debug.Log("smash" + isDemonPowerCharge);
        //        isDemonPowerCharge = false;
        //        demonAnimationController.IsNotElectric();
        //        acceleration = 2.5f;
        //        maxSpeedLeft = 5f;
        //        DemonAnimationController.Instance.SmashAttack();
        //        demonPowerChargeTimer = 0;
        //        StartCoroutine(IsStanding());
        //    }
    }

    IEnumerator IsStanding()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("isStanding count down");

    }

    private void ElectricEffectAfterDemonPowerPunch()
    {
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
    private void DemonPowerPunchMovePower()
    {
        while (playerDirection == Direction.left)
        {
            Debug.Log("left" + animatorStateInfo.IsName("demon power punch"));
            if (animatorStateInfo.IsName(Tags.DemonPowerCharge))
            {

            }
            if (animatorStateInfo.IsName(Tags.DemonPowerPunch) || animatorStateInfo.IsName(Tags.DemonPowerPunch2))
            {
                //targetPosition = Vector2.left * -10f * Time.deltaTime;


                //playerTransform.position = Vector2.SmoothDamp(playerTransform.position, targetPosition, ref currentVelocity, smoothing, maxSpeed);

                playerTransform.Translate(Vector2.left * curSpeed * Time.deltaTime);


                curSpeed += acceleration * Time.deltaTime;

                if (curSpeed > maxSpeedLeft)
                {

                    curSpeed = maxSpeedLeft;
                }
                //playerTransform.Translate(Vector2.left * 10f * Time.deltaTime);

            }
            break;
        }

        while (playerDirection == Direction.right)
        {
            Debug.Log("right" + animatorStateInfo.IsName("demon power punch"));
            if (animatorStateInfo.IsName(Tags.DemonPowerCharge))
            {

            }
            if (animatorStateInfo.IsName(Tags.DemonPowerPunch) || animatorStateInfo.IsName(Tags.DemonPowerPunch2))
            {


                playerTransform.Translate(Vector2.right * curSpeed * Time.deltaTime);

                curSpeed += acceleration * Time.deltaTime;

                if (curSpeed > maxSpeedLeft)
                {

                    curSpeed = maxSpeedLeft;
                }
                //playerTransform.Translate(Vector2.right * 10f * Time.deltaTime);
            }
            break;
        }
    }
    private void DemonGroundShake()
    {
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

    private void ResetAnimationTriggers()
    {

        isWalking = false;
        isRunning = false;
        isDashing = false;
        isSprinting = false;
        isDemonSecondPunch = false;
        isDemonPowerPunch2 = false;
        isDemonPunch = false;
        isDemonPowerPunch = false;
        toolEffect = ToolEffect.none;
    }

    void Movement()
    {
        canMove = true;
        if (canMove)
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

        CameraPlayerPosition();

    }
    public void CameraPlayerPosition()
    {
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







        canMove = false;

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
        Debug.Log("isstanding" + isStanding);
        isDemon = true;
        isStanding = true;
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

