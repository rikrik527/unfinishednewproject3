using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yushan.Enums;
using Yushan.DarkAbilites;
using Yushan.movement;
using System;

namespace Yushan.DarkType
{
    public class DarkTypeMoves : MonoBehaviour
    {
        public float darkDoubleSpearKickForce = 10f;
        public float darkKneeKickForce = 10f;
        public float kickDirection;


        [SerializeField] private float curSpeed = 0.0f;
        [SerializeField] private float maxSpeed = 10f;
        [SerializeField] private float accelertation = 5.0f;
        public float timeRemaining = 1.8f;
        public bool timerIsRunniing = false;
        public float startTime = 0f;
        private Animator animator;
        public static DarkTypeMoves Instance { get; private set; }



        private AnimatorStateInfo animatorStateInfo;
        public AnimatorStateInfo[] arrayStateInfo;
        private Player player;
        private Transform playerTransform;
        private BoxCollider2D boxCollider2D;//player boxcollider2d
        //get the animation state hash for each Animation state
        public const string baseLayer = "Base Layer";
        public int darkDoubleSpearKickAnimHash = Animator.StringToHash(baseLayer + ".dark double spear kick");
        public int darkKneeKickAnimHash = Animator.StringToHash(baseLayer + ".dark knee kick");
        public int darkCrossKickAnimHash = Animator.StringToHash(baseLayer + ".dark cross kick");
        // animation clip to calulate time
        public AnimationClip darkDoubleSpearKickClip;
        public AnimationClip darkKneeKickClip;
        public AnimationClip darkCrossKickClip;

        // Dictionary to link each animation state hash with their animation clip
        Dictionary<int, AnimationClip> hashToClip = new Dictionary<int, AnimationClip>();

        private bool actived;
        private GameObject playerObject;
        private void Awake()
        {
            Instance = this;
            animator = GetComponent<Animator>();
            playerTransform = GetComponentInParent<Transform>();

            hashToClip.Add(darkDoubleSpearKickAnimHash, darkDoubleSpearKickClip);
            hashToClip.Add(darkKneeKickAnimHash, darkKneeKickClip);
            hashToClip.Add(darkCrossKickAnimHash, darkCrossKickClip);
        }
        private void Update()
        {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            for (int i = 0; i < animatorStateInfo.length; i++)
            {
                arrayStateInfo[i] = animator.GetCurrentAnimatorStateInfo(0);
                Debug.Log(arrayStateInfo[i].length + "ddddkdkdkdk");
            }
        }
        private void FixedUpdate()
        {
            if (animatorStateInfo.IsTag("motion"))
            {
                Debug.Log("lateupdatemotion");
                if (actived)
                {
                    if (Input.GetMouseButtonDown(0) && Player.Instance.isDashing)
                    {
                        Debug.Log("mousedown" + Player.Instance.isDashing);

                        if (actived)
                        {


                            Player.Instance.rigidbody2D.velocity = Vector2.zero;
                            Player.Instance.dashDirection = (int)Player.Instance.movX;
                            StartCoroutine(DarkKneeKick());

                            Debug.Log(actived + "actived");
                        }
                    }
                }


                if (Input.GetMouseButtonDown(0) && Player.Instance.isRunning)
                {
                    Debug.Log("mousedown isrunning" + Player.Instance.isRunning);
                    if (Player.Instance.isRunningRight)
                    {

                        Debug.Log("player isruun right");
                        timerIsRunniing = true;
                        if (timerIsRunniing)
                        {
                            if (timeRemaining > 0f)
                            {
                                startTime += Time.deltaTime;
                                if (startTime >= timeRemaining)
                                {

                                    StartCoroutine(AttackCo());
                                    startTime = 0;


                                }
                            }


                        }
                    }


                }

                if (Input.GetMouseButtonDown(0) && Player.Instance.isRunning)
                {
                    Debug.Log("mousedown" + Player.Instance.isRunning);
                    if (Player.Instance.isRunningLeft)
                    {

                        timerIsRunniing = true;
                        if (timerIsRunniing)
                        {
                            if (timeRemaining > 0f)
                            {
                                startTime += Time.deltaTime;
                                if (startTime >= timeRemaining)
                                {

                                    StartCoroutine(AttackCo());
                                    startTime = 0;



                                }
                            }
                        }
                    }

                }



            }



        }



        private void LateUpdate()
        {
            if (animator != null)
            {
                Debug.Log("animator is not null");
                if (animatorStateInfo.IsName(Tags.DarkDoubleSpearKick) && animatorStateInfo.normalizedTime == 1f)
                {
                    Debug.Log("darkdoublespearkick finished stop");
                    Player.Instance.rigidbody2D.velocity = Vector2.zero;

                }
                Debug.Log("isrunning" + Player.Instance.isRunning + "isdashing" + Player.Instance.isDashing);
            }
        }
        public void ActivedAttack()
        {

            actived = true;
        }
        public void ActivedFalse()
        {
            actived = false;
        }
        private IEnumerator DarkKneeKick()
        {
            Debug.Log("darkkneekick");

            animator.SetTrigger("isDarkKneeKick");
            Player.Instance.rigidbody2D.velocity = transform.right * Player.Instance.dashDirection * darkKneeKickForce;
            yield return new WaitForSeconds(1f);







        }

        public void DarkDoubleSpearKick()
        {
            Debug.Log("darkkick");
            //animator.SetBool("isAttacking", true);
            animator.SetTrigger("isDarkDoubleSpearKick");
            //float time = GetCurrentAnimatorTime(animator, 1);
            //Debug.Log(time);
            kickDirection = (int)Player.Instance.movX;
            Player.Instance.rigidbody2D.velocity = transform.right *
            kickDirection * darkDoubleSpearKickForce;


        }



        private IEnumerator AttackCo()
        {

            Debug.Log("StartCoroutine");
            DarkDoubleSpearKick();

            yield return null;




            yield return new WaitForSeconds(1f);
            Player.Instance.rigidbody2D.velocity = Vector2.zero;

            Debug.Log("coruotine done" + animatorStateInfo.IsName(Tags.DarkDoubleSpearKick) + animatorStateInfo.normalizedTime);
        }
        public void DarkCrossKick()
        {
            animator.SetTrigger("isDarkCrossKick");
        }
        public float GetCurrentAnimatorTime(Animator targetAnim, int layer = 0)
        {
            AnimatorStateInfo animState = targetAnim.GetCurrentAnimatorStateInfo(layer);

            int currentAnimHash = animState.fullPathHash;

            AnimationClip clip = GetClipFromHash(currentAnimHash);
            float currentTime = animState.normalizedTime;
            return currentTime;
        }
        AnimationClip GetClipFromHash(int hash)
        {
            AnimationClip clip;
            if (hashToClip.TryGetValue(hash, out clip))
            {
                return clip;
            }
            else
            {
                return null;
            }
        }
    }
}

