using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using Yushan.Enums;
using Yushan.abilities;
using Yushan.DemonType;
using Yushan.combo;
using Yushan.DarkType;
namespace Yushan.movement
{
    public class Player : SingletonMonobehaviour<Player>
    {
        public Yushan_Type yushan_Type = Yushan_Type.fatType;

        //type of yushan type
        private bool fatType;
        private bool darkenType;
        private bool demonType;
        public float slowMotionTimeScale;



        public bool isAttacking = false;

        public PlayerState currentStat;
        private Vector3 change;

        public float runningSpeed = 10f;
        public float dashForce = 3f;
        public float startDashTimer = 0.24f;
        public float currentDashTimer;
        public float dashDirection;
        public bool isDashing = false;

        public KeyCode tapLeft;
        public KeyCode tapRight;

        public int leftTotal = 0;
        public float leftTimeDelay = 0;
        public int rightTotal = 0;
        public float rightTimeDelay = 0;
        public float dashDuration = 2;
        public int xVel = 0;

        private AnimatorClipInfo[] animatorClipInfo;


        public AnimationClip darkDoubleSpearKickClip;
        public const string attackingLayer = "Attacking";
        public const string animBaseLayer = "Base Layer Attacking";
        public int darkDoubleSpearHash = Animator.StringToHash(attackingLayer + ".dark-double-spear-kick");

        Dictionary<int, AnimationClip> hashToClip = new Dictionary<int, AnimationClip>();

        [SerializeField] private float leftOffsetX;
        [SerializeField] private float rightOffsetX;



        private SpriteRenderer spriteRenderer;



        private CinemachineFollowZoom cinemachineFollowZoom;




        private Transform playerTransform;
        private Transform point;
        [SerializeField] private GameObject followCamera;
        [SerializeField] private GameObject closeCamera;
        [SerializeField] private GameObject closeCameraRight;


        //spriterender[]
        [SerializeField]
        private Sprite[] playerSprite;

        private AnimatorStateInfo animatorStateInfo;



        private bool switchDemon;


        //running bool
        public bool isRunningRight;
        public bool isRunningLeft;


        public float movX;
        public Camera camera;

        // game start
        private bool gameStart;

        private float dashTimeLeft;
        private float lastImageXpos;
        private float lastDash = -100f;
        public float dashTime;
        public float dashSpeed;
        public float distanceBetweenImages;
        public float dashCoolDown;
        public bool canMove;
        public bool canFlip;
        //movement parameters
        private float inputX;
        private bool isIdle;
        private bool isWalking;
        public bool isRunning;
        public bool isDashingRight;
        public bool isDashingLeft;
        private bool isSprinting;
        private bool isDemonTransform;
        public bool isDemonPowerCharge;
        private bool isDemonPunch;
        private bool isDemonPowerPunch;
        private bool isDemonIdle;
        private bool isDemon;
        private bool isDemonSecondPunch;
        private bool isDemonPowerPunch2;
        public bool isDemonPowerCharge2;
        private bool isDarkDoubleSpearKick;

        public Rigidbody2D rigidbody2D;
        public Yushan_Type yushanType;
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


        private Animator animator;



        public bool canRun = false;
        public bool canSprint = false;



        ToolEffect toolEffect = ToolEffect.none;
        public float orthoSize;

        //camera
        [SerializeField] public CinemachineVirtualCamera cinemachineVirtualCamera;
        public CinemachineTransposer cinemachineTransposer;


        protected override void Awake()
        {

            animator = GetComponentInChildren<Animator>();
            playerTransform = GetComponent<Transform>();
            point = GameObject.FindGameObjectWithTag(Tags.Point).GetComponent<Transform>();
            hashToClip.Add(darkDoubleSpearHash, darkDoubleSpearKickClip);
            base.Awake();

        }
        private void Start()
        {
            if (!gameStart)
            {
                cinemachineVirtualCamera.Follow = point;
                StartCoroutine(StartGameTransition());
                playerDirection = Direction.right;
            }

            yushanType = Yushan_Type.darkenType;
            rigidbody2D = GetComponent<Rigidbody2D>();
            cinemachineVirtualCamera = GameObject.FindGameObjectWithTag(Tags.FollowCamera).GetComponent<CinemachineVirtualCamera>();
            cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            orthoSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;

        }


        private void Update()
        {
            animatorClipInfo = animator.GetCurrentAnimatorClipInfo(0);




            switch (yushanType)
            {
                case Yushan_Type.fatType:
                    Debug.Log("fat");
                    break;
                case Yushan_Type.darkenType:
                    darkenType = true;
                    if (darkenType)
                    {
                        spriteRenderer.sprite = playerSprite[1];
                    }
                    break;
                case Yushan_Type.demonType:
                    demonType = true;
                    if (demonType)
                    {
                        spriteRenderer.sprite = playerSprite[2];
                    }
                    break;
                default:
                    Debug.Log("default");
                    break;
            }

            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (yushanType == Yushan_Type.darkenType)
            {
                if (!PlayerInputIsDisabled)
                {

                    Debug.Log("!playerinputiddisabled");

                    if (animatorStateInfo.IsTag("motion"))
                    {
                        movX = Input.GetAxis("Horizontal");
                        Debug.Log("motion");
                        if (movX > 0)
                        {
                            canMove = true;
                            if (canMove)
                            {
                                isRunning = true;
                                isRunningRight = true;
                                isRunningLeft = false;



                                animator.SetBool("isRunRight", true);
                                playerTransform.Translate(runningSpeed * Time.deltaTime, 0, 0);
                                Debug.Log("isrunningright" + isRunning);
                                playerDirection = Direction.right;
                                if (playerDirection == Direction.right)
                                {
                                    canFlip = true;
                                    if (canFlip)
                                    {
                                        spriteRenderer.flipX = false;
                                    }

                                }
                            }

                        }
                        else if (movX < 0)
                        {
                            canMove = true;
                            if (canMove)
                            {
                                isRunning = true;
                                isRunningRight = false;
                                isRunningLeft = true;



                                animator.SetBool("isRunLeft", true);
                                playerTransform.Translate(-runningSpeed * Time.deltaTime, 0, 0);
                                Debug.Log("isrunningright" + isRunning);
                                playerDirection = Direction.left;
                                if (playerDirection == Direction.left)
                                {
                                    canFlip = true;
                                    if (canFlip)
                                    {
                                        spriteRenderer.flipX = true;
                                    }

                                }
                            }

                        }
                        //if (movX > 0)
                        //{
                        //    if (Input.GetKey(tapRight))
                        //    {
                        //        Debug.Log("get");
                        //        //rigidbody2D.velocity = new Vector3(2 + xVel, 0, 0);
                        //        isRunning = true;
                        //        isRunningRight = true;
                        //        isRunningLeft = false;
                        //        isDashing = false;


                        //        animator.SetBool("isRunRight", true);
                        //        playerTransform.Translate(runningSpeed * Time.deltaTime, 0, 0);
                        //        Debug.Log("isrunningright" + isRunning);
                        //    }
                        //    if (Input.GetKeyDown(tapRight))
                        //    {

                        //        rightTotal += 1;
                        //    }
                        //    if (Input.GetKeyUp(tapRight))
                        //    {
                        //        Debug.Log("get up");
                        //        //xVel = 0;
                        //        //rigidbody2D.velocity = new Vector3(0, 0, 0);

                        //    }
                        //    if ((rightTotal == 1) && (rightTimeDelay < .5))
                        //    {
                        //        rightTimeDelay += Time.deltaTime;
                        //    }
                        //    if ((rightTotal == 1) && (rightTimeDelay >= .5))
                        //    {
                        //        rightTimeDelay = 0;
                        //        rightTotal = 0;
                        //    }
                        //    if ((rightTotal == 2) && (rightTimeDelay < .5))
                        //    {
                        //        Debug.Log("isdashing");
                        //        xVel = 2;
                        //        rightTotal = 0;
                        //        animator.SetTrigger("isDash");
                        //        isDashing = true;

                        //    }
                        //    if ((rightTotal == 2) && (rightTimeDelay >= .5))
                        //    {
                        //        xVel = 0;
                        //        rightTotal = 0;
                        //        rightTimeDelay = 0;
                        //    }
                        //    if (xVel == 2)
                        //    {
                        //        dashDuration += Time.deltaTime;
                        //    }
                        //    if (dashDuration > 1)
                        //    {
                        //        xVel = 0;
                        //        dashDuration = 0;
                        //        rightTotal = 0;
                        //        rightTimeDelay = 0;
                        //    }
                        //}


                        //if (movX < 0)
                        //{
                        //    if (Input.GetKey(tapLeft))
                        //    {
                        //        Debug.Log("get");
                        //        //rigidbody2D.velocity = new Vector3(2 + xVel, 0, 0);
                        //        isRunning = true;
                        //        isRunningRight = false;
                        //        isRunningLeft = true;
                        //        isDashing = false;


                        //        animator.SetBool("isRunLeft", true);
                        //        playerTransform.Translate(-runningSpeed * Time.deltaTime, 0, 0);
                        //        Debug.Log("isrunningright" + isRunning);
                        //    }

                        //    if (Input.GetKeyDown(tapLeft))
                        //    {

                        //        leftTotal += 1;
                        //    }
                        //    if (Input.GetKeyUp(tapLeft))
                        //    {
                        //        Debug.Log("get up");
                        //        //xVel = 0;
                        //        //rigidbody2D.velocity = new Vector3(0, 0, 0);

                        //    }
                        //    if ((leftTotal == 1) && (leftTimeDelay < .5))
                        //    {
                        //        leftTimeDelay += Time.deltaTime;
                        //    }
                        //    if ((leftTotal == 1) && (leftTimeDelay >= .5))
                        //    {
                        //        leftTimeDelay = 0;
                        //        leftTotal = 0;
                        //    }
                        //    if ((leftTotal == 2) && (leftTimeDelay < .5))
                        //    {
                        //        Debug.Log("isdashing");
                        //        xVel = 2;
                        //        rightTotal = 0;
                        //        animator.SetTrigger("isDash");
                        //        isDashing = true;

                        //    }
                        //    if ((leftTotal == 2) && (leftTimeDelay >= .5))
                        //    {
                        //        xVel = 0;
                        //        leftTotal = 0;
                        //        leftTimeDelay = 0;
                        //    }
                        //    if (xVel == 2)
                        //    {
                        //        dashDuration += Time.deltaTime;
                        //    }
                        //    if (dashDuration > 1)
                        //    {
                        //        xVel = 0;
                        //        dashDuration = 0;
                        //        leftTotal = 0;
                        //        leftTimeDelay = 0;
                        //    }

                        //}

                        //if (Input.GetMouseButtonDown(1) && movX != 0 && !isDashing)
                        //{
                        //    Debug.Log("mousedown ready to dash");
                        //    isDashing = true;

                        //    //playerTransform.Translate((Vector3.right * 10) * Time.deltaTime * dashingSpeed);
                        //    currentDashTimer = startDashTimer;
                        //    rigidbody2D.velocity = Vector2.zero;
                        dashDirection = (int)movX;


                        //}
                        //if (isDashing)
                        //{
                        //    Debug.Log("dashing");
                        //    animator.SetTrigger("isDash");
                        //    rigidbody2D.velocity = transform.right * dashDirection * dashForce;
                        //    currentDashTimer -= Time.deltaTime;
                        //    if (currentDashTimer <= 0)
                        //    {
                        //        Debug.Log("isdashing false");
                        //        isDashing = false;
                        //    }
                        //}

                        if (Input.GetMouseButtonDown(1) && !isDashing)
                        {
                            if (Time.time >= (lastDash + dashCoolDown))
                            {
                                AttemptToDash();
                            }
                        }
                        if (Input.GetAxis("Horizontal") == 0)
                        {
                            isRunningLeft = false;
                            isRunningRight = false;
                            isDashing = false;
                            isRunning = false;
                            animator.SetBool("isRunRight", false);
                            animator.SetBool("isRunLeft", false);

                        }
                        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() == true)
                        {
                            //jump
                            Debug.Log("jump");
                            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);

                            StartCoroutine(ResetJumpNeededRoutine());
                        }

                        CheckDash();
                    }






                    // send event to any listeners for player movement input



                }
            }






            CameraPlayerPosition();





            //float time = GetCurrentAnimatorTime(animator, 0);
            //Debug.Log(time);

        }
        private void AttemptToDash()
        {
            isDashing = true;
            canMove = false;
            canFlip = false;
            dashTimeLeft = dashTime;
            lastDash = Time.time;

            PlayerAfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;
        }
        private void CheckDash()
        {
            if (isDashing)
            {
                if (dashTimeLeft > 0)
                {
                    animator.SetTrigger("isDash");
                    rigidbody2D.velocity = new Vector2(dashSpeed * dashDirection, rigidbody2D.velocity.y);

                    dashTimeLeft -= Time.deltaTime;
                    if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                    {
                        PlayerAfterImagePool.Instance.GetFromPool();
                        lastImageXpos = transform.position.x;
                    }
                }
                if (dashTimeLeft <= 0)
                {
                    // || isTouchingWall
                    isDashing = false;
                    canMove = true;
                    canFlip = true;
                }
            }
        }
        void FixedUpdate()
        {


        }
        private void LateUpdate()
        {
            if (animatorStateInfo.IsName("dash") && animatorStateInfo.normalizedTime == 1f)
            {
                Debug.Log("stop dash");
                rigidbody2D.velocity = Vector2.zero;
            }
        }
        // Update is called once per frame


        public void InputDashLeft()
        {

        }
        public void InputDashRight()
        {

        }
        private void ResetMovement()
        {
            // reset movement
            inputX = 0f;
            isRunning = false;
            isWalking = false;
            isIdle = true;
        }
        public void DisablePlayerInputAndResetMovement()
        {
            DisablePlayerInput();
            //ResetMovement();
            //// send event to any listeners for player movement input
            //EventHandler.CallMovementEvent(inputX, isWalking, isRunning, isSprinting, isDashing, isIdle, toolEffect, isDemonTransform, isDarkDoubleSpearKick);

        }
        public void DisablePlayerInput()
        {
            PlayerInputIsDisabled = true;
        }
        public void EnablePlayerInput()
        {
            PlayerInputIsDisabled = false;
        }




        public void DemonTransform()
        {
            animator.SetTrigger("isDemonTransform");
        }
        public void DemonPunch()
        {
            animator.Play("demon punch");
        }
        public void DemonSecondPunch()
        {
            Debug.Log("demon second punch");
            animator.Play("demon second punch");
        }

        public void DemonPowerPunch()
        {
            Debug.Log("demon power punch");
            if (Input.GetMouseButtonDown(1) && isDemon && animatorStateInfo.IsName(Tags.DemonPunch) && animatorStateInfo.normalizedTime <= 0.6f)
            {
                isDemonPowerCharge = true;

            }
            if (Input.GetMouseButton(1) && isDemon && isDemonPowerCharge)
            {
                Debug.Log(animatorStateInfo.IsName(Tags.DemonPowerCharge) + "isdemonpowercharge");
                isDemonPowerPunch = true;
            }

        }
        //dark yushan



        private void ChangeSprite()
        {
            //if (fatType)
            //{
            //    spriteRenderer = playerSprite[0];
            //}
            //if (darkType)
            //{
            //    spriteRenderer = playerSprite[1];
            //}
            //if (demonType)
            //{
            //    spriteRenderer = playerSprite[2];
            //}

        }


        void InputMouseAttack()
        {
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
            //if (Input.GetMouseButtonDown(0) && isDemon)
            //{
            //    Debug.Log("moudedown");
            //    NormalAttack();
            //}
            //if (Input.GetMouseButtonDown(1) && isDemon)
            //{
            //    Debug.Log("mouse smash clicked");
            //    SmashAttack();
            //}

            //if (Input.GetMouseButtonDown(0))
            //{
            //    Debug.Log("attack next");
            //    StartCoroutine(AttackCo());
            //    Debug.Log("currentstat" + currentStat);
            //    InputMouseAttack();
            //    EventHandler.CallMovementEvent(inputX, isWalking, isRunning, isSprinting, isDashing, isIdle, toolEffect, isDemonTransform, isDarkDoubleSpearKick);
            //    EventHandler.CallDemonEvent(isDemon, isDemonIdle, isDemonPunch, isDemonPowerCharge, isDemonPowerPunch, isDemonSecondPunch, isDemonPowerCharge2, isDemonPowerPunch2);
            //}

        }


        private void ResetAnimationTriggers()
        {
            isDemonPunch = false;
            isDemonSecondPunch = false;
            isWalking = false;
            isRunning = false;

            isSprinting = false;
            toolEffect = ToolEffect.none;
        }




        public void CameraPlayerPosition()
        {

            //cinemachineTransposer = cinemachineVirtualCamera.AddCinemachineComponent<CinemachineTransposer>();
            while (playerDirection == Direction.left && cinemachineTransposer.m_FollowOffset.x >= -10f)
            {
                Debug.Log("left");
                cinemachineTransposer.m_FollowOffset.x -= leftOffsetX;
                break;


            }
            while (playerDirection == Direction.right && cinemachineTransposer.m_FollowOffset.x <= 11.8f)
            {
                Debug.Log("right");
                cinemachineTransposer.m_FollowOffset.x += rightOffsetX;
                break;


            }
            if (isRunning)
            {

                Debug.Log("if dashing");
                while (orthoSize <= 14f)
                {

                    orthoSize += 0.1f;
                    if (orthoSize >= 13f)
                    {

                        cinemachineVirtualCamera.m_Lens.OrthographicSize = 13f;
                        break;
                    }
                }
            }
            if (!isRunning)
            {

                cinemachineVirtualCamera.m_Lens.OrthographicSize = 8.345f;

            }
        }




        public bool IsGrounded()
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












        IEnumerator StartGameTransition()
        {
            yield return new WaitForSeconds(1.0f);
            cinemachineVirtualCamera.Follow = playerTransform;
            cinemachineVirtualCamera.LookAt = playerTransform;
            gameStart = true;
        }

        //combo
        //public void ComboPossible()
        //{
        //    Debug.Log("combopossible trigger");
        //    comboPossible = true;

        //    Debug.Log("combopossible should be true" + comboPossible);
        //}
        //public void ResetComBo()
        //{
        //    Debug.Log("reset");
        //    comboPossible = false;
        //    inputSmash = false;
        //    comboStep = 0;
        //}
        //public void NextAtk()
        //{



        //    if (!inputSmash)
        //    {
        //        Debug.Log("!ifsmash" + comboStep);
        //        if (comboStep == 1)
        //        {

        //            Debug.Log("combo step" + comboStep);
        //            //Player.Instance.DemonSecondPunch();
        //            animator.Play(Tags.DemonSecondPunch);
        //        }
        //        //if (comboStep == 3)
        //        //{
        //        //    animator.Play("demon power punch2");
        //        //}
        //    }
        //    if (inputSmash)
        //    {
        //        if (comboStep == 1)
        //        {

        //            Debug.Log("inputsmash" + comboStep);
        //            Debug.Log("demon power punch");

        //            //player.position = Vector3.MoveTowards(player.position, target.position, step);




        //            //rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x + 2f, rigidbody2D.velocity.y);
        //            //StartCoroutine(DemonPowerPunch());

        //        }
        //        if (comboStep == 2)
        //        {

        //        }



        //    }


        //}
        //public void NormalAttack()
        //{
        //    Debug.Log("normal attack" + comboStep + comboPossible);
        //    if (comboStep == 0)
        //    {
        //        Player.Instance.DemonPunch();

        //        comboStep = 1;
        //        return;
        //    }
        //    if (comboStep != 0)
        //    {
        //        if (comboPossible)
        //        {
        //            comboPossible = false;
        //            comboStep += 1;
        //        }
        //    }
        //}
        //public void SmashAttack()
        //{


        //    if (comboPossible)
        //    {


        //        comboPossible = false;
        //        inputSmash = true;
        //    }
        //}



    }
}


