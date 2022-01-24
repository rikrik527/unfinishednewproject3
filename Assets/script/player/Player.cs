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
using System;

namespace Yushan.movement
{
    public class Player : SingletonMonobehaviour<Player>
    {
        public Yushan_Type yushan_Type;
        [Header("Components")]
        public Rigidbody2D rigidbody2D;
        public Animator animator;

        [Header("Layer Masks")]
        [SerializeField] private LayerMask cornerCorrectLayer;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask wallLayer;
        [Header("Movement Variables")]
        [SerializeField] private float movementAcceleration = 70f;
        [SerializeField] private float maxMoveSpeed = 6.66f;
        [SerializeField] private float groundLinearDrag = 7f;
        public float movY;
        public float movX;
        public bool changeDirection => (rigidbody2D.velocity.x > 0f && movX < 0f) || (rigidbody2D.velocity.x < 0f && movX > 0f);
        public bool facingRight = true;
        public bool canMove => !wallGrab;

        [Header("jump variable")]
        [SerializeField] private float jumpForce = 12f;
        [SerializeField] private float airLinearDrag = 2.5f;
        [SerializeField] private float fallMultiplier = 8f;
        [SerializeField] private float lowJumpFallMultiplier = 5f;
        [SerializeField] private float downMultiplier = 12f;
        [SerializeField] private int extraJumps = 1;
        [SerializeField] private float hangTime = .1f;
        [SerializeField] private float jumpBufferLength;
        private int extraJumpValue;
        private float hangTimeCounter;
        private float jumpBufferCounter;
        private bool canJump => jumpBufferCounter > 0f && (hangTimeCounter > 0f || extraJumpValue > 0 || onWall);
        private bool isJumping = false;

        [Header("wall movement variables")]
        [SerializeField] private float wallSliderModifier = 0.5f;
        [SerializeField] private float wallRunModifier = 0.85f;
        [SerializeField] private float wallJumpXVelocityHaltDelay = 0.2f;
        private bool wallGrab => onWall && !onGround && Input.GetKey(KeyCode.H) && !wallRun;
        private bool wallSlide => onWall && !onGround && !Input.GetKey(KeyCode.H) && rigidbody2D.velocity.y < 0f && !wallRun;
        private bool wallRun => onWall && movY > 0f;


        [Header("Ground collision Variables")]
        [SerializeField] private float groundRaycastLength;
        [SerializeField] private Vector3 groundRaycastOffset;
        private bool onGround;

        [Header("wall collision variables")]
        [SerializeField] private float wallRaycastLength;
        private bool onWall;
        private bool onRightWall;

        [Header("corner correction variables")]
        [SerializeField] private float topRaycastLength;
        [SerializeField] private Vector3 edgeRaycastOffset;
        [SerializeField] private Vector3 innerRaycastOffset;
        private bool canCornerCorrect;

        //type of yushan type
        private bool fatType;
        private bool darkenType;
        private bool demonType;




        public bool isAttacking = false;

        public PlayerState currentStat;
        private Vector3 change;

        public float runningSpeed = 10f;
        public float sprintingSpeed = 15f;
        public float dashForce = 3f;
        public float startDashTimer = 0.24f;
        public float currentDashTimer;
        public float dashDirection;


        [Header("Dash Variables")]
        public float dashSpeed = 15f;
        public float dashLength = .3f;
        public float dashBufferLength = .1f;
        public float dashBufferCounter;
        public bool isDashing;
        public bool hasDashed;
        public bool canDash => dashBufferCounter > 0f && !hasDashed;




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




        public GameObject playerGameObject;
        private Transform point;
        [SerializeField] private GameObject followCamera;
        [SerializeField] private GameObject closeCamera;
        [SerializeField] private GameObject closeCameraRight;


        //spriterender[]
        [SerializeField]
        private Sprite[] playerSprite;

        private AnimatorStateInfo animatorStateInfo;






        //running bool
        public bool isRunningRight;
        public bool isRunningLeft;


        public Camera camera;

        // game start
        private bool gameStart;

        private float dashTimeLeft;
        private float lastImageXpos;
        private float lastDash = -100f;
        public float dashTime;

        public float distanceBetweenImages;
        public float dashCoolDown;

        public bool canFlip;
        public bool dash;
        //movement parameters

        public bool isRunning;
        public bool isSprinting;
        public bool isSlowingDown;



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



        [SerializeField] private GameObject followCameraLeft;

        [SerializeField]


        private bool resetJump = false;

        public Yushan_Move_type yushan_Move_Type;




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
            Debug.Log("candash" + canDash);
        }


        private void Update()
        {


            animatorClipInfo = animator.GetCurrentAnimatorClipInfo(0);

            movX = GetInput().x;
            movY = GetInput().y;


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


                        Debug.Log("motion");
                        if (GetInput().x > 0 && Input.GetKey(KeyCode.D))
                        {

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

                                playerGameObject.transform.Translate(runningSpeed * Time.deltaTime, 0, 0);
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
                        else if (GetInput().x < 0 && Input.GetKey(KeyCode.A))
                        {

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

                                playerGameObject.transform.Translate(-runningSpeed * Time.deltaTime, 0, 0);
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
                        if (GetInput().x > 0 && isRunning && Input.GetKey(KeyCode.M))
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
                                    playerGameObject.transform.Translate(sprintingSpeed * Time.deltaTime, 0, 0);
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
                        if (GetInput().x < 0 && isRunning && Input.GetKey(KeyCode.M))
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
                                    playerGameObject.transform.Translate(-sprintingSpeed * Time.deltaTime, 0, 0);
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






                        if (GetInput().x == 0)
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
                        if (Input.GetButtonDown("Jump"))
                        {
                            jumpBufferCounter = jumpBufferLength;
                        }
                        else
                        {
                            jumpBufferCounter -= Time.deltaTime;
                        }
                        if (Input.GetButtonDown("Dash"))
                        {

                            dashBufferCounter = dashBufferLength;
                            Debug.Log("j press" + canDash);
                        }
                        else
                        {
                            dashBufferCounter -= Time.deltaTime;
                            Debug.Log("else" + canDash);
                        }
                        Animation();
                    }

                    //if (animatorStateInfo.IsTag("dash"))
                    //{

                    //}




                    // send event to any listeners for player movement input



                }
            }






            CameraPlayerPosition();





            //float time = GetCurrentAnimatorTime(animator, 0);
            //Debug.Log(time);

        }






        void FixedUpdate()
        {
            if (canDash) StartCoroutine(Dash(movX, movY));
            if (!isDashing)
            {
                if (canMove) MoveCharacter();
                else
                {
                    rigidbody2D.velocity = Vector2.Lerp(rigidbody2D.velocity, (new Vector2(movX * maxMoveSpeed, rigidbody2D.velocity.y)), .5f * Time.deltaTime);
                }
                if (onGround)
                {
                    ApplyGroundLinearDrag();
                    extraJumpValue = extraJumps;
                    hangTimeCounter = hangTime;
                    hasDashed = false;
                }
                else
                {
                    ApplyAirLinearDrag();
                    FallMultiplier();
                    hangTimeCounter -= Time.fixedDeltaTime;
                    if (!onWall || rigidbody2D.velocity.y < 0f || wallRun)
                    {
                        isJumping = false;
                    }
                }
                if (canJump)
                {
                    if (onwall && !onGround)
                    {
                        if (!wallRun && (onRightWall) && movX > 0f || !onRightWall && movX < 0f)
                        {
                            StartCoroutine(NeutralWallJump());
                        }
                        else
                        {
                            WallJump();
                        }
                        Flip();
                    }
                    else
                    {
                        Jump(Vector2.up);
                    }
                }
                if (!isJumping)
                {
                    if (wallSlide) WallSlide();
                    if (wallGrab) WallGrab();
                    if (wallRun) WallRun();
                    if (onWall) StickToWall();
                }
            }
            if (canCornerCorrect) cornerCorrectLayer(rigidbody2D.velocity.y);
        }
        private Vector2 GetInput()
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        private void MoveCharacter()
        {
            rigidbody2D.AddForce(new Vector2(movX, 0f) * movementAcceleration);
            if (Mathf.Abs(rigidbody2D.velocity.x) > maxMoveSpeed)
            {
                rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxMoveSpeed, rigidbody2D.velocity.y);
            }
        }
        private void ApplyGroundLinearDrag()
        {
            if (Mathf.Abs(movX) < 0.4f || changeDirection)
            {
                Debug.Log("applygroubdlineardrag" + changeDirection);
                rigidbody2D.drag = groundLinearDrag;
            }
            else
            {
                rigidbody2D.drag = 0f;
            }
        }
        private void ApplyAirLinearDrag()
        {
            Debug.Log("applyairlineardrag");
            rigidbody2D.drag = airLinearDrag;
        }
        private void Jump(Vector2 direction)
        {
            if (!onGround && !onWall)
            {
                extraJumpValue--;
            }
            ApplyAirLinearDrag();
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
            rigidbody2D.AddForce(direction * jumpForce, ForceMode2D.Impulse);
            hangTimeCounter = 0f;
            jumpBufferCounter = 0f;
            isJumping = false;
        }
        private void WallJump()
        {

            Vector2 jumpDirection = onRightWall ? Vector2.left : Vector2.right;
            Jump(Vector2.up + jumpDirection);
            Debug.Log("walljump" + jumpDirection);
        }
        IEnumerator Dash(float x, float y)
        {
            float dashStartTime = Time.time;
            hasDashed = true;
            isDashing = true;
            isJumping = false;
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.gravityScale = 0f;
            rigidbody2D.drag = 0f;

            Vector2 dir;
            if (x != 0f || y != 0f) dir = new Vector2(x, y);
            else
            {
                if (playerDirection == Direction.right)
                {
                    dir = new Vector2(1f, 0f);
                }
                else
                {
                    dir = new Vector2(-1f, 0f);
                }
            }
            while (Time.time < dashStartTime + dashLength)
            {
                rigidbody2D.velocity = dir.normalized * dashSpeed;
                yield return null;
            }
            isDashing = false;
        }
        void Animation()
        {
            if (isDashing)
            {
                animator.SetBool("isDashing", true);

            }
            else
            {
                animator.SetBool("isDashing", false);
            }
        }
        private void LateUpdate()
        {
            if (animator != null)
            {
                if (yushanType == Yushan_Type.darkenType)
                {

                }

            }


        }
        IEnumerator AttemptToDash()
        {
            Debug.Log("attemptodash");
            funcExcuted = false;
            isDashing = true;

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

                    canFlip = true;
                }
            }
        }
        public void Dash()
        {
            dash = true;

            animator.SetTrigger("Dash");

            playerGameObject.transform.Translate(dashSpeed * dashDirection, 0, 0);
            //rigidbody2D.velocity = new Vector2(dashSpeed * dashDirection, rigidbody2D.velocity.y);

            Debug.Log("dash");


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

            if (isSprinting == true)
            {
                followCamera.SetActive(false);
                followCameraLeft.SetActive(true);


            }

            else if (isSprinting == false)
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
            cinemachineVirtualCamera.Follow = playerGameObject.transform;
            cinemachineVirtualCamera.LookAt = playerGameObject.transform;
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


