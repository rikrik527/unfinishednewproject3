using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
//public class DemonActions : MonoBehaviour
//{

//    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
//    private CinemachineFollowZoom cinemachineFollowZoom;

//    private Animator animator;
//    private Animator demonAnimator;
//    private Rigidbody2D rigidbody2D;

//    public static DemonActions demonInstance;
//    private Transform playerTransform;
//    private Transform point;
//    [SerializeField] private GameObject followCamera;
//    [SerializeField] private GameObject closeCamera;

//    private AnimatorStateInfo animatorStateInfo;
//    private bool isDemonTransform;


//    private bool switchDemon;
//    private bool isDemon;


//    private float holdDownStartingTime;

//    //color
//    public Color colorWhite = Color.white;
//    public Color black = Color.black;
//    public float duration = 3.0f;

//    public Camera camera;

//    // game start
//    private bool gameStart;
//    private void Awake()
//    {
//        demonInstance = this;
//        playerTransform = GetComponent<Transform>();
//        point = GameObject.FindGameObjectWithTag(Tags.Point).GetComponent<Transform>();
//    }
//    void Start()
//    {
//        camera = GameObject.FindGameObjectWithTag(Tags.MainCamera).GetComponent<Camera>();
//        camera.clearFlags = CameraClearFlags.SolidColor;
//        if (!gameStart)
//        {
//            cinemachineVirtualCamera.Follow = point;
//            StartCoroutine(StartGameTransition());
//        }

//        demonAnimator = GetComponentInChildren<Animator>();

//        animator = GetComponentInChildren<Animator>();
//        rigidbody2D = GetComponent<Rigidbody2D>();
//        cinemachineFollowZoom = GameObject.FindGameObjectWithTag(Tags.FollowCamera).GetComponent<CinemachineFollowZoom>();
//        if (cinemachineFollowZoom == null)
//        {
//            Debug.Log("wrong");

//        }
//        else
//        {
//            Debug.Log("cine" + cinemachineFollowZoom);
//        }

//    }

//    // Update is called once per frame
//    void FixedUpdate()
//    {

//        Player.Instance.playerDirection = Direction.left;

//        if (Input.GetKeyDown(KeyCode.E) && gameStart)
//        {
//            DemonTransform();

//        }
//        if (Input.GetMouseButtonDown(0) && isDemon)
//        {

//            DemonAnimationController.demonControllerInstance.NormalAttack();
//        }

//        if (Input.GetMouseButtonDown(1) && isDemon)
//        {

//            DemonAnimationController.demonControllerInstance.SmashAttack();

//        }


//        EventHandler.CallDemonEvent(isDemonTransform, isDemon);
//        //DemonPunch();

//    }
//    private void LateUpdate()
//    {
//        if (animator == null)
//        {
//            Debug.Log("animator == null");
//        }
//        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
//        if (animatorStateInfo.IsTag("demon transform"))
//        {
//            Debug.Log(animatorStateInfo);
//        }
//        if (animatorStateInfo.IsName("demon transform") && animatorStateInfo.normalizedTime <= 0.1f)
//        {
//            Debug.Log("run");
//            ShakeCamera.Instance.Shake(5f, 1f);

//        }
//        if (animatorStateInfo.IsName("demon transform") && animatorStateInfo.normalizedTime >= 0.9f)
//        {

//            ShakeCamera.Instance.Shake(0f, 0f);

//        }
//    }

//    private void DemonTransform()
//    {

//        Debug.Log("demon transform");


//        animator.SetTrigger(Settings.isDemonTransform);





//    }


//    public void DemonYushanIdle()
//    {
//        isDemon = true;
//        animator.SetBool(Settings.isDemon, isDemon);
//        Debug.Log("isdemon" + isDemon);
//    }

//    IEnumerator StartGameTransition()
//    {
//        yield return new WaitForSeconds(1.0f);
//        cinemachineVirtualCamera.Follow = playerTransform;
//        cinemachineVirtualCamera.LookAt = playerTransform;
//        gameStart = true;
//    }


//}

//public void DemonPowerPunch()
//{
//    if (isDemonPunch == true && Input.GetMouseButtonDown(1))
//    {
//        isDemonPunch = false;
//        animator.SetTrigger(Settings.isDemonPowerPunch);
//    }
//}

