using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class DemonAnimationController : MonoBehaviour
{
    [SerializeField] private Animator effectAnimator;
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
    private Transform playerGameobject;
    public static DemonAnimationController Instance;
    public bool comboPossible;
    public int comboStep;
    public bool inputSmash;
    public float step = 2.0f;
    //target
    private Transform target;
    private Player Player1;
    //player
    private Transform player;


    private void Awake()
    {
        Instance = this;


        player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Transform>();
        if (camera == null)
        {
            camera = GameObject.FindGameObjectWithTag(Tags.MainCamera).GetComponent<Camera>();
            if (camera != null)
            {
                cinemachineBrain = camera.GetComponent<CinemachineBrain>();
            }

        }
        Player1 = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Player>();
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

        playerGameobject = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Transform>();
        mainCamera.GetComponent<CinemachineBrain>();
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();


    }
    private void OnEnable()
    {
        EventHandler.DemonEvent += SetDemonParameter;
    }
    private void OnDisable()
    {
        EventHandler.DemonEvent -= SetDemonParameter;


    }
    private void SetDemonParameter(bool isDemonTransform, bool isDemon, bool isDemonPowerCharge, bool isDemonPowerPunch, bool isDemonPunch, bool isDemonSecondPunch, bool isDemonIdle, bool isDemonPowerPunch2, bool isDemonPowerCharge2)
    {

        animator.SetBool(Settings.isDemon, isDemon);
        animator.SetBool(Settings.isDemonPowerCharge, isDemonPowerCharge);
        animator.SetBool(Settings.isDemonPowerCharge2, isDemonPowerCharge2);
        animator.SetBool(Settings.isDemonIdle, isDemonIdle);
        if (isDemonSecondPunch)
        {
            Player.Instance.canMove = false;
            animator.SetTrigger(Settings.isDemonSecondPunch);
        }
        if (isDemonPowerPunch2)
        {
            animator.SetTrigger(Settings.isDemonPowerPunch2);
        }
        if (isDemonPunch)
        {
            Player.Instance.canMove = false;
            animator.SetTrigger(Settings.isDemonPunch);
        }
        if (isDemonTransform)
        {
            Player.Instance.canMove = false;
            animator.SetTrigger(Settings.isDemonTransform);
        }
        if (isDemonPowerPunch)
        {
            Player.Instance.canMove = false;
            animator.SetTrigger(Settings.isDemonPowerPunch);
        }
    }
    //effect
    public void IsSatanReturn()
    {
        effectAnimator.SetTrigger("isSatanReturn");
    }
    public void IsFeatherEffect()
    {
        effectAnimator.SetTrigger("isFeatherEffect");
    }
    public void IsElectric()
    {
        effectAnimator.SetBool("isElectricEffect", true);
    }
    public void IsNotElectric()
    {
        effectAnimator.SetBool("isElectricEffect", false);
    }


    private void Update()
    {

    }
    private void FixedUpdate()
    {

    }

    //idle
    public void DemonIdle()
    {

        Player.Instance.DemonYushanIdle();
        isDemonIdle = true;

    }
    public void DemonPowerCharge()
    {
        isDemonPowerCharge = true;
        Player.Instance.canMove = false;
    }
    public void DemonPowerCharge2()
    {
        isDemonPowerCharge2 = true;
    }

    //attack
    public void ComboPossible()
    {
        Debug.Log("combo -possible");
        comboPossible = true;
    }

    private void LateUpdate()
    {

    }
    public void DemonPowerPunch2()
    {
        Debug.Log("demonppunch2");
        animator.SetTrigger(Settings.isDemonPowerPunch2);
    }
    public void DemonPowerPunch()
    {
        Debug.Log("demonpowerpunch" + isDemonPowerCharge);



        animator.SetTrigger(Settings.isDemonPowerPunch);




    }
    public void ResetComBo()
    {
        Debug.Log("reset");
        Player.Instance.canMove = true;
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

        Debug.Log("next attack" + comboStep);

        if (!inputSmash)
        {
            Debug.Log("!ifsmash" + comboStep);
            if (comboStep == 2)
            {

                Debug.Log("combo step" + comboStep);
                DemonSecondPunch();
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

        Debug.Log("smash attack" + isDemonPowerCharge);
        if (comboPossible)
        {
            Debug.Log("combopossible");

            comboPossible = false;
            inputSmash = true;
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

