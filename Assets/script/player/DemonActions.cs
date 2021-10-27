using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class DemonActions : MonoBehaviour
{
    private const string TransformDemon = "transform demon";
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineFollowZoom cinemachineFollowZoom;

    private Animator animator;
    private Animator demonAnimator;
    private Rigidbody2D rigidbody2D;

    public static DemonActions demonInstance;
    private Transform playerTransform;
    private Transform point;
    [SerializeField] private GameObject followCamera;
    [SerializeField] private GameObject closeCamera;

    private AnimatorStateInfo animatorStateInfo;
    private bool isDemonTransform;


    private bool switchDemon;
    private bool isDemon;


    // game start
    private bool gameStart;
    private void Awake()
    {
        demonInstance = this;
        playerTransform = GetComponent<Transform>();
        point = GameObject.FindGameObjectWithTag(Tags.Point).GetComponent<Transform>();
    }
    void Start()
    {
        if (!gameStart)
        {
            cinemachineVirtualCamera.Follow = point;
            StartCoroutine(StartGameTransition());
        }

        demonAnimator = GetComponentInChildren<Animator>();

        animator = GetComponentInChildren<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        cinemachineFollowZoom = GameObject.FindGameObjectWithTag(Tags.FollowCamera).GetComponent<CinemachineFollowZoom>();
        if (cinemachineFollowZoom == null)
        {
            Debug.Log("wrong");

        }
        else
        {
            Debug.Log("cine" + cinemachineFollowZoom);
        }

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.E) && gameStart)
        {
            DemonTransform();

        }

        EventHandler.CallDemonEvent(isDemonTransform, isDemon);
        //DemonPunch();

    }
    private void LateUpdate()
    {
        if (animator == null)
        {
            Debug.Log("animator == null");
        }
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.IsTag("demon transform"))
        {
            Debug.Log(animatorStateInfo);
        }
        if (animatorStateInfo.IsName("demon transform") && animatorStateInfo.normalizedTime <= 0.1f)
        {
            Debug.Log("run");
            closeCamera.SetActive(true);
            followCamera.SetActive(false);

        }
        if (animatorStateInfo.IsName("demon transform") && animatorStateInfo.normalizedTime >= 0.6f)
        {
            Debug.Log("runrun");

            closeCamera.SetActive(false);
            followCamera.SetActive(true);
        }
    }
    private void DemonTransform()
    {

        Debug.Log("demon transform");


        animator.SetTrigger(Settings.isDemonTransform);





    }


    public void DemonYushanIdle()
    {
        isDemon = true;
        animator.SetBool(Settings.isDemon, isDemon);
        Debug.Log("isdemon" + isDemon);
    }

    IEnumerator StartGameTransition()
    {
        yield return new WaitForSeconds(1.0f);
        cinemachineVirtualCamera.Follow = playerTransform;
        cinemachineVirtualCamera.LookAt = playerTransform;
        gameStart = true;
    }
}

//public void DemonPowerPunch()
//{
//    if (isDemonPunch == true && Input.GetMouseButtonDown(1))
//    {
//        isDemonPunch = false;
//        animator.SetTrigger(Settings.isDemonPowerPunch);
//    }
//}

