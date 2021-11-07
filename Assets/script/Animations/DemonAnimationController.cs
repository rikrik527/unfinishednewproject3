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
    private void SetDemonParameter(bool isDemonTransform, bool isDemon)
    {

        animator.SetBool(Settings.isDemon, isDemon);

        if (isDemonTransform)
        {
            animator.SetTrigger(Settings.isDemonTransform);
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

    //idle
    public void DemonIdle()
    {

        Player.Instance.DemonYushanIdle();

    }

    //attack
    public void ComboPossible()
    {
        Debug.Log("combo -possible");
        comboPossible = true;
    }
    public void NextAtk()
    {
        Debug.Log("next attack" + Player1.playerDirection);
        if (!inputSmash)
        {
            if (comboStep == 2)
            {
                Debug.Log("combo step" + comboStep);
                animator.Play(Tags.DemonSecondPunch);
            }
            //if (comboStep == 3)
            //{
            //    animator.Play("demon second punch");
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
        }
    }
    private void LateUpdate()
    {

    }
    public void DemonPowerPunch()
    {



        //Debug.Log("left" + rigidbody2D.name);

        //rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
        //rigidbody2D.AddForce(new Vector2(10f * -5f, 0f), ForceMode2D.Impulse);
        //float gravity = rigidbody2D.gravityScale;
        //rigidbody2D.gravityScale = 0;

        animator.Play(Tags.DemonPowerPunch);


        //rigidbody2D.gravityScale = gravity;

        //while (Player1.playerDirection == Direction.right)
        //{

        //    Debug.Log(player.name);
        //    //player.Translate(Vector2.right * 10f * Time.deltaTime);

        //    //Debug.Log("right" + rigidbody2D.name);

        //    //rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
        //    //rigidbody2D.AddForce(new Vector2(10f * 5f, 0f), ForceMode2D.Impulse);
        //    //float gravity = rigidbody2D.gravityScale;
        //    //rigidbody2D.gravityScale = 0;

        //    animator.Play(Tags.DemonPowerPunch);
        //    if (animatorStateInfo.IsName(Tags.DemonPowerPunch) != null && animatorStateInfo.normalizedTime >= 0.3f)
        //    {
        //        Debug.Log("moveright" + animatorStateInfo.IsName(Tags.DemonPowerPunch));
        //        player.Translate(Vector2.right * 10f * Time.fixedDeltaTime);
        //    }
        //    //rigidbody2D.gravityScale = gravity;
        //    break;
        //}
    }
    //IEnumerator DemonPowerPunch()
    //{
    //    yield return new WaitForSeconds(0.2f);
    //    animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
    //    if (animatorStateInfo.IsName("demon power punch") && animatorStateInfo.normalizedTime <= 0.1f)
    //    {
    //        Debug.Log("animator power punch is not null");
    //        animator.speed = 0.1f;
    //        Debug.Log("start count down");
    //        yield return new WaitForSeconds(0.5f);
    //        animator.speed = 1f;
    //    }
    //}
    public void ResetComBo()
    {
        Debug.Log("resetcombo");
        comboPossible = false;
        inputSmash = false;
        comboStep = 0;
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
        Debug.Log("smash attack");
        if (comboPossible)
        {
            comboPossible = false;
            inputSmash = true;
        }
    }
    public void DemonPunch()
    {
        Debug.Log("demon punch");
        animator.Play(Tags.DemonPunch);
    }
}

