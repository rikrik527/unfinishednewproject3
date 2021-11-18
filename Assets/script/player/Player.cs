using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
public class Player : MonoBehaviour
{

    public float slowMotionTimeScale;










    private SpriteRenderer spriteRenderer;



    private CinemachineFollowZoom cinemachineFollowZoom;




    private Transform playerTransform;
    private Transform point;
    [SerializeField] private GameObject followCamera;
    [SerializeField] private GameObject closeCamera;
    [SerializeField] private GameObject closeCameraRight;




    public AnimatorStateInfo animatorStateInfo;
    private bool isDemonTransform;


    private bool switchDemon;
    private bool isDemon;





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
    public bool isDemonPowerCharge;
    private bool isDemonPunch;
    private bool isDemonPowerPunch;
    private bool isDemonIdle;
    private bool isDemonSecondPunch;
    private bool isDemonPowerPunch2;
    public bool isDemonPowerCharge2;

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



        //if (Input.GetMouseButtonDown(0) && isDemon && !isDemonPowerCharge && !isDemonPowerCharge2)
        //{

        //    Debug.Log(isDemonPowerCharge + "first click");
        //    DemonAnimationController.Instance.NormalAttack();
        //    canMove = true;


        //}


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









        // charge input mouse up
        //ChargeAttackAndInputMouseUp();





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

            Debug.Log(animatorStateInfo.IsName(Tags.DemonPunch) + "demon punch");
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

