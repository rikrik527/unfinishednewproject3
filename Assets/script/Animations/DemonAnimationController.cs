using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class DemonAnimationController : MonoBehaviour
{
    [SerializeField] private Animator effectAnimator;

    private Animator animator;
    private bool isDemonTransform;
    private bool isDemon;
    private DemonActions demonActions;
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineTransposer cinemachineTransposer;
    private Camera camera;
    private CinemachineBrain cinemachineBrain;
    [SerializeField] private GameObject followCamera;
    [SerializeField] private GameObject closeCam;
    [SerializeField] private Camera mainCamera;
    private Transform playerGameobject;
    public static DemonAnimationController demonControllerInstance;
    public bool comboPossible;
    public int comboStep;
    public bool inputSmash;

    private void Awake()
    {
        demonControllerInstance = this;

        if (camera == null)
        {
            camera = GameObject.FindGameObjectWithTag(Tags.MainCamera).GetComponent<Camera>();
            if (camera != null)
            {
                cinemachineBrain = camera.GetComponent<CinemachineBrain>();
            }

        }

        animator = GetComponent<Animator>();
        cinemachineVirtualCamera = GameObject.FindGameObjectWithTag(Tags.FollowCamera).GetComponent<CinemachineVirtualCamera>();
    }
    private void Start()
    {

        playerGameobject = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Transform>();
        mainCamera.GetComponent<CinemachineBrain>();
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        demonActions = GetComponentInParent<DemonActions>();
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
    public void DemonIdle()
    {
        Debug.Log("demonidle");
        DemonActions.demonInstance.DemonYushanIdle();
    }
    public void ComboPossible()
    {
        comboPossible = true;
    }
    public void NextAtk()
    {
        if (!inputSmash)
        {
            if (comboStep == 2)
            {
                animator.Play("demon punch");
            }
            if (comboStep == 3)
            {
                animator.Play("demon second punch");
            }
        }
        if (inputSmash)
        {
            if (comboStep == 1)
            {
                animator.Play("demon power punch");
            }
        }
    }
    public void ResetComBo()
    {
        comboPossible = false;
        inputSmash = false;
        comboStep = 0;
    }
    public void NormalAttack()
    {
        if (comboStep == 0)
        {
            animator.Play("demon punch");
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
}
