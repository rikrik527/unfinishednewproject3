using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Demon : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    public Animator animator;
    public AnimatorStateInfo animatorStateInfo;
    private bool isDemonTransform;
    private bool isDemon;
    private bool isDemonPowerCharge;
    private bool isDemonPunch;
    private bool isDemonIdle;
    private bool isDemonPowerPunch;
    private bool isDemonSecondPunch;
    private bool isDemonPowerCharge2;
    private bool isDemonPowerPunch2;
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineTransposer cinemachineTransposer;
    private Camera camera;
    private CinemachineBrain cinemachineBrain;
    [SerializeField] private GameObject followCamera;
    [SerializeField] private GameObject closeCam;
    [SerializeField] private Camera mainCamera;
    public static Demon Instance;


    public bool comboPossible;
    public int comboStep;
    public bool inputSmash;
    public float step = 2.0f;

    //player
    private Transform player;

    public bool firstClick;

    //timer
    public const float demonPowerChargeDuration = 1f;
    public float demonPowerChargeTimer = 0f;
    public float demonPowerChargeTimer2 = 0f;
    public float speedFactor = 2.3f;
    //demon
    //demon power punch
    public Vector2 targetPosition;//where you want the gameobject to move to
    private Vector2 currentVelocity = Vector2.zero;//this is used inside the function dont touch
    private float smoothing = 0.5f;
    private float maxSpeed = 10f;
    public float acceleration = 5.0f;
    public float maxSpeedLeft = 10f;
    public float curSpeed = 0.0f;
    //get current play clip info
    public AnimatorClipInfo[] animatorClipInfo;
    public string clipName;
    public float clipLength;
    private void Awake()
    {

        player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Transform>();
        if (camera == null)
        {
            camera = GameObject.FindGameObjectWithTag(Tags.MainCamera).GetComponent<Camera>();
            if (camera != null)
            {
                cinemachineBrain = camera.GetComponent<CinemachineBrain>();
            }

        }

        rigidbody2D = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            Debug.Log(animatorStateInfo.fullPathHash.ToString());

        }
        cinemachineVirtualCamera = GameObject.FindGameObjectWithTag(Tags.FollowCamera).GetComponent<CinemachineVirtualCamera>();


    }
    private void Start()
    {
        mainCamera.GetComponent<CinemachineBrain>();
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isDemon)
        {
            Debug.Log("clicked");
            NormalAttack();
        }
        EventHandler.CallDemonEvent(isDemonTransform, isDemon, isDemonPowerCharge, isDemonPowerPunch, isDemonPunch, isDemonSecondPunch, isDemonIdle, isDemonPowerPunch2, isDemonPowerCharge2);
    }

    private void LateUpdate()
    {
        if (animator != null)
        {
            animatorClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        }
    }
    //attack
    public void ComboPossible()
    {

        comboPossible = true;
    }




    public void ResetComBo()
    {
        Debug.Log("reset");

        firstClick = false;
        isDemonPowerPunch2 = false;
        isDemonPowerPunch = false;
        isDemonPunch = false;
        isDemonSecondPunch = false;
        comboPossible = false;
        inputSmash = false;
        comboStep = 0;
    }
    public void NextAtk()
    {

        Debug.Log("next attack" + comboStep + "firstclick" + firstClick);
        if (!firstClick)
        {
            firstClick = true;

            if (!inputSmash)
            {
                Debug.Log("!ifsmash" + comboStep);
                if (comboStep == 2)
                {

                    Debug.Log("combo step" + comboStep);
                    animator.Play(Tags.DemonSecondPunch);
                    //animator.Play(Tags.DemonSecondPunch);
                }
                //if (comboStep == 3)
                //{
                //    animator.Play("demon power punch2");
                //}
            }
            if (inputSmash)
            {
                if (comboStep == 1)
                {
                    float speed = step * Time.deltaTime;
                    Debug.Log("inputsmash" + comboStep);
                    Debug.Log("demon power punch");

                    //player.position = Vector3.MoveTowards(player.position, target.position, step);


                    DemonPowerPunch();

                    //rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x + 2f, rigidbody2D.velocity.y);
                    //StartCoroutine(DemonPowerPunch());

                }
                if (comboStep == 2)
                {
                    DemonPowerPunch2();
                }
            }


        }


    }
    public void NormalAttack()
    {
        Debug.Log("normal attack");
        if (comboStep == 0)
        {
            DemonPunch();

            comboStep = 1;
            return;
        }
        if (comboStep != 0)
        {
            if (comboPossible)
            {
                comboPossible = false;
                comboStep += 1;
            }
        }
    }
    public void SmashAttack()
    {


        if (comboPossible)
        {


            comboPossible = false;
            inputSmash = true;
        }
    }

    //demon punch
    public void DemonPowerPunch()
    {


        if (Input.GetMouseButtonDown(1) && isDemon && !isDemonPowerCharge)
        {


            demonPowerChargeTimer = 0;
            isDemonPowerCharge = true;
            Debug.Log("1" + clipName);
        }
        if (Input.GetMouseButton(1) && isDemon && isDemonPowerCharge)
        {
            Debug.Log("2" + clipName);
            demonPowerChargeTimer += Time.deltaTime;
            Effect.Instance.IsElectric();
            while (demonPowerChargeTimer >= demonPowerChargeDuration)
            {
                Debug.Log("3" + demonPowerChargeTimer);
                isDemonPowerCharge = false;
                if (isDemonPowerCharge == false)
                {
                    Effect.Instance.IsNotElectric();
                    acceleration = 10f;
                    maxSpeedLeft = 15f;
                    animator.SetTrigger(Settings.isDemonPowerPunch);
                    demonPowerChargeTimer = 0;
                    Debug.Log("demon power punch" + clipName);



                }
                break;




            }

        }
        if (Input.GetMouseButtonUp(1) && isDemon && isDemonPowerCharge)
        {
            Debug.Log("4");
            if (demonPowerChargeTimer < demonPowerChargeDuration)
            {

                Effect.Instance.IsNotElectric();
                Debug.Log("mouse up no power punch" + isDemonPowerCharge);
                acceleration = 2.5f;
                maxSpeedLeft = 5f;


                animator.SetTrigger(Settings.isDemonPowerPunch);
                demonPowerChargeTimer = 0;
                Debug.Log("demon power punch" + clipName);

            }


        }
        Effect.Instance.ElectricEffectAfterDemonPowerPunch();
        while (Player.Instance.playerDirection == Direction.left)
        {
            Debug.Log("left" + animatorStateInfo.IsName("demon power punch"));
            if (animatorStateInfo.IsName(Tags.DemonPowerCharge))
            {

            }
            if (animatorStateInfo.IsName("demon power punch transition") || animatorStateInfo.IsName(Tags.DemonPowerPunch2))
            {
                //targetPosition = Vector2.left * -10f * Time.deltaTime;


                //playerTransform.position = Vector2.SmoothDamp(playerTransform.position, targetPosition, ref currentVelocity, smoothing, maxSpeed);

                player.Translate(Vector2.left * curSpeed * Time.deltaTime);


                curSpeed += acceleration * Time.deltaTime;

                if (curSpeed > maxSpeedLeft)
                {

                    curSpeed = maxSpeedLeft;
                }
                //playerTransform.Translate(Vector2.left * 10f * Time.deltaTime);

            }
            break;
        }

        while (Player.Instance.playerDirection == Direction.right)
        {
            Debug.Log("right" + animatorStateInfo.IsName("demon power punch"));
            if (animatorStateInfo.IsName(Tags.DemonPowerCharge))
            {

            }
            if (animatorStateInfo.IsName(Tags.DemonPowerPunch) || animatorStateInfo.IsName(Tags.DemonPowerPunchTransition))
            {
                Debug.Log("demon power punch transition");

                player.Translate(Vector2.right * curSpeed * Time.deltaTime);

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


    public void DemonPowerPunch2()
    {
        Debug.Log("demonppunch2");
        animator.SetTrigger(Settings.isDemonPowerPunch2);
    }






    IEnumerator CheckAnimation(float seconds)
    {
        if (animator != null)
        {
            yield return new WaitForSeconds(seconds);
        }
    }

    public void DemonPunch()
    {

        Debug.Log("demon punch" + isDemonPunch);
        animator.SetTrigger(Settings.isDemonPunch);



    }
    public void DemonSecondPunch()
    {
        Debug.Log("demon second punch");

        animator.SetTrigger(Settings.isDemonSecondPunch);


    }
}
