using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class DemonAnimationController : MonoBehaviour
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
    private Transform playerGameobject;
    public static DemonAnimationController Instance;

    //target
    private Transform target;
    private GameObject Player1;



    private void Awake()
    {
        Instance = this;
        animator = GetComponentInParent<Animator>();
    }
    private void Start()
    {





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

            animator.SetTrigger(Settings.isDemonSecondPunch);
        }
        if (isDemonPowerPunch2)
        {
            animator.SetTrigger(Settings.isDemonPowerPunch2);
        }
        if (isDemonPunch)
        {
            animator.SetTrigger(Settings.isDemonPunch);
        }
        if (isDemonTransform)
        {

            animator.SetTrigger(Settings.isDemonTransform);
        }
        if (isDemonPowerPunch)
        {

            animator.SetTrigger(Settings.isDemonPowerPunch);
        }
    }




    private void FixedUpdate()
    {

    }
    private void LateUpdate()
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

    }
    public void DemonPowerCharge2()
    {
        isDemonPowerCharge2 = true;
    }


    // effect on sprite

    public void IsSantanReturn()
    {
        Effect.Instance.IsSatanReturn();
    }
    public void IsFeatherEffect()
    {
        Effect.Instance.IsFeatherEffect();
    }
    public void IsElectric()
    {
        Effect.Instance.IsElectric();
    }
    public void IsNotElectric()
    {
        Effect.Instance.IsNotElectric();
    }
}




