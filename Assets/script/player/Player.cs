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


        public bool isJumping;
        public bool isAttacking = false;

        public PlayerState currentStat;
        private Vector3 change;
        public Transform dashPosition;
        public float runningSpeed = 10f;
        public float sprintingSpeed = 15f;
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
        public Camera mainCamera;

        public AnimationClip darkDoubleSpearKickClip;
        public const string attackingLayer = "Attacking";
        public const string animBaseLayer = "Base Layer Attacking";
        public int darkDoubleSpearHash = Animator.StringToHash(attackingLayer + ".dark-double-spear-kick");

        Dictionary<int, AnimationClip> hashToClip = new Dictionary<int, AnimationClip>();

        [SerializeField] private float leftOffsetX;
        [SerializeField] private float rightOffsetX;

        private bool funcExcuted;

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

        public float dashLerpSpeed;

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
        public bool dash;
        //movement parameters
        private float inputX;
        private bool isIdle;
        private bool isWalking;
        public bool isRunning;
        public bool isDashingRight;
        public bool isDashingLeft;
        public bool isSprinting;
        public bool isSlowingDown;
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


        [SerializeField] private GameObject followCameraLeft;

        [SerializeField]
        private LayerMask groundLayer;

        private bool resetJump = false;

        public Yushan_Move_type yushan_Move_Type;
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
            mainCamera = Camera.main;
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
                                if (isRunning)
                                {
                                    isSprinting = false;
                                }

                                if (IsGrounded() == true)
                                {
                                    animator.SetBool("isRunRight", true);
                                }
                                else if (IsGrounded() == false)
                                {
                                    animator.SetBool("isRunRight", false);
                                }

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

                                if (isRunning)
                                {
                                    isSprinting = false;
                                }
                                if (IsGrounded() == true)
                                {
                                    animator.SetBool("isRunLeft", true);
                                }
                                else if (IsGrounded() == false)
                                {
                                    animator.SetBool("isRunLeft", false);
                                }

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
                        if (movX > 0 && isRunning && Input.GetKey(KeyCode.M))
                        {
                            isSprinting = true;
                            if (isSprinting)
                            {
                                isRunning = false;
                                isRunningRight = false;
                                isRunningLeft = false;
                                Debug.Log("isspritnt");
                                if (Input.GetKey(KeyCode.D))
                                {
                                    animator.SetBool("isRunRight", false);
                                    animator.SetBool("isRunning", false);
                                    animator.SetBool("isSprintRight", true);
                                    playerTransform.Translate(sprintingSpeed * Time.deltaTime, 0, 0);
                                }
                                if (Input.GetKeyUp(KeyCode.D))
                                {
                                    Debug.Log("keyupD");
                                    isSprinting = false;
                                    isSlowingDown = true;
                                    animator.SetBool("isSprintRight", false);
                                    animator.SetTrigger("isSlowingDown");
                                }
                            }

                        }
                        if (movX < 0 && isRunning && Input.GetKey(KeyCode.M))
                        {
                            isSprinting = true;
                            if (isSprinting)
                            {

                                isRunning = false;
                                Debug.Log("issprintleft");
                                isRunningRight = false;
                                isRunningLeft = false;
                                if (Input.GetKey(KeyCode.A))
                                {
                                    animator.SetBool("isRunning", false);
                                    animator.SetBool("isRunLeft", false);
                                    animator.SetBool("isSprintLeft", true);
                                    playerTransform.Translate(-sprintingSpeed * Time.deltaTime, 0, 0);
                                }
                                if (Input.GetKeyUp(KeyCode.A))
                                {
                                    Debug.Log("keyup left sprint");
                                    isSprinting = false;
                                    isSlowingDown = true;
                                    animator.SetBool("isSprintingLeft", false);
                                    animator.SetBool("isSlowingDown", true);
                                }
                            }


                        }


                        dashDirection = (int)movX;






                        if (Input.GetAxis("Horizontal") == 0)
                        {
                            isRunningLeft = false;
                            isRunningRight = false;
                            isDashing = false;
                            isRunning = false;
                            isSprinting = false;
                            dash = false;
                            animator.SetBool("isSprinting", false);
                            animator.SetBool("isRunRight", false);
                            animator.SetBool("isRunLeft", false);
                            animator.SetBool("isSprintRight", false);
                            animator.SetBool("isSprintLeft", false);

                        }
                        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() == true)
                        {
                            //jump
                            Debug.Log("jump");
                            isJumping = true;
                            isSprinting = false;
                            isRunning = false;
                            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);

                            StartCoroutine(ResetJumpNeededRoutine());
                        }


                    }






                    // send event to any listeners for player movement input



                }
            }






            CameraPlayerPosition();





            //float time = GetCurrentAnimatorTime(animator, 0);
            //Debug.Log(time);

        }
        public void MoveYushanIsUsing()
        {
            if (isRunning)
            {
                yushan_Move_Type = Yushan_Move_type.runningType;
                Debug.Log("runningtype" + yushan_Move_Type);
            }
            if (isSprinting)
            {
                yushan_Move_Type = Yushan_Move_type.sprintType;
                Debug.Log("sprinttype" + Yushan_Move_type.sprintType);
            }
            if (isJumping)
            {
                yushan_Move_Type = Yushan_Move_type.jumpType;
                Debug.Log("jumptype" + yushan_Move_Type);
            }
            if (dash)
            {
                yushan_Move_Type = Yushan_Move_type.dashType;
            }
            if (ComboSystem.Instance.isDarkComboSystem)
            {
                yushan_Move_Type = Yushan_Move_type.darkComboType;
                Debug.Log("darkcombotype" + yushan_Move_Type);
            }
        }
        void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.J) && movX != 0 && (IsGrounded() == true || IsGrounded() == false))
            {
                Debug.Log("ready to attempdash is grounded or on air");
                if (Time.time >= (lastDash + dashCoolDown))
                {
                    StartCoroutine(AttemptToDash());


                }
            }

        }
        private void LateUpdate()
        {
            if (animator != null)
            {
                if (yushanType == Yushan_Type.darkenType)
                {
                    if (animatorStateInfo.IsName("dash") && animatorStateInfo.normalizedTime >= 0.99f)
                    {
                        Debug.Log("stop dash");
                        dash = false;
                        isDashing = false;
                        rigidbody2D.velocity = Vector2.zero;
                        Debug.Log("dash" + dash);
                    }
                    if (animatorStateInfo.IsName(Tags.isSlowingDown) && animatorStateInfo.normalizedTime >= 0.9f)
                    {
                        isSlowingDown = false;
                        animator.SetBool("isSlowingDown", false);
                    }
                }

            }


        }
        IEnumerator AttemptToDash()
        {
            Debug.Log("attemptodash");
            funcExcuted = false;
            isDashing = true;
            canMove = false;
            canFlip = false;
            dashTimeLeft = dashTime;
            lastDash = Time.time;

            PlayerAfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;
            funcExcuted = true;
            yield return new WaitForSeconds(0.1f);
            if (funcExcuted == true)
            {

                Debug.Log("attempdash is excuted");
                CheckDash();
            }
        }
        private void CheckDash()
        {
            Debug.Log("check dash is excute");
            if (isDashing)
            {
                Debug.Log("isdashing");
                Dash();
                if (dashTimeLeft > 0)
                {


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
        public void Dash()
        {
            Vector3 dashTargetPosition = dashPosition.position;
            Vector3 playerPosition = playerTransform.position;
            dash = true;

            animator.SetTrigger("isDash");
            playerTransform.position = Vector3.MoveTowards(playerPosition, Vector3.Lerp(playerPosition, dashTargetPosition, dashLerpSpeed), dashSpeed * Time.deltaTime);
            //rigidbody2D.velocity = new Vector2(dashSpeed * dashDirection, rigidbody2D.velocity.y);
            if (playerDirection == Direction.right)
            {
                dashPosition.position = new Vector3(13f, 0, 0);
            }

            if (playerDirection == Direction.left)
            {
                dashPosition.position = new Vector3(-13f, 0, 0);
            }
            Debug.Log("dash");


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
            //while (playerDirection == Direction.left && cinemachineTransposer.m_FollowOffset.x >= -10f)
            //{
            //    Debug.Log("left");
            //    cinemachineTransposer.m_FollowOffset.x -= leftOffsetX;
            //    break;


            //}
            //while (playerDirection == Direction.right && cinemachineTransposer.m_FollowOffset.x <= 11.8f)
            //{
            //    Debug.Log("right");
            //    cinemachineTransposer.m_FollowOffset.x += rightOffsetX;
            //    break;


            //}
            //while (playerDirection == Direction.left && isRunning == true)
            //{
            //    cinemachineTransposer.m_FollowOffset.x += 10f;
            //    break;
            //}

            if (isSprinting == true && orthoSize <= 14f)
            {
                followCamera.SetActive(false);
                followCameraLeft.SetActive(true);


            }

            else if (isSprinting == false && orthoSize >= 8.345f)
            {
                followCamera.SetActive(true);
                followCameraLeft.SetActive(false);
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
                    isJumping = false;
                    return true;

                }


            }
            isJumping = true;
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
        public Vector3 GetPlayerViewPortPosition()
        {
            //vector3 viewport position for player viewport
            return mainCamera.WorldToViewportPoint(transform.position);
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


