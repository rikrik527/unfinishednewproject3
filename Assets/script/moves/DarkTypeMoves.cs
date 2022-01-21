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
        public float darkDoubleSpearKickForce = 20f;
        public float darkKneeKickForce = 20f;
        public float kickDirection;
        public bool readyToKick;

        [SerializeField] private float curSpeed = 0.0f;
        [SerializeField] private float maxSpeed = 10f;
        [SerializeField] private float accelertation = 5.0f;
        public float timeRemaining = 1.8f;
        public bool timerIsRunniing = false;
        public float startTime = 0f;
        private Animator animator;
        public static DarkTypeMoves Instance { get; private set; }

        public Yushan_Type yushan_Type;

        private AnimatorStateInfo animatorStateInfo;
        public AnimatorStateInfo[] arrayStateInfo;

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
            yushan_Type = Yushan_Type.darkenType;
        }

        private void Update()
        {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.IsTag("motion"))
            {
                if (yushan_Type == Yushan_Type.darkenType)
                {
                    Debug.Log("lateupdatemotion");

                    if (Input.GetKeyDown(KeyCode.H) && Player.Instance.dash)
                    {






                        //Player.Instance.rigidbody2D.velocity = Vector2.zero;
                        //Player.Instance.dashDirection = (int)Player.Instance.movX;
                        DarkKneeKick();




                    }


                }



                if (Input.GetKeyDown(KeyCode.J) && Player.Instance.isSprinting && !Player.Instance.dash)
                {
                    Debug.Log("mousedown isrunning" + Player.Instance.isRunning);


                    Debug.Log("player isruun right");
                    timerIsRunniing = true;
                    while (timerIsRunniing)
                    {
                        if (timeRemaining > 0f)
                        {
                            startTime += Time.deltaTime;

                            if (startTime >= timeRemaining)
                            {
                                readyToKick = true;

                                if (readyToKick)
                                {
                                    StartCoroutine(AttackCo());
                                    startTime = 0;
                                    timerIsRunniing = false;
                                    readyToKick = false;
                                    break;
                                }


                            }
                        }



                    }


                }

                if (Input.GetKeyDown(KeyCode.J) && Player.Instance.isSprinting && !Player.Instance.dash)
                {
                    Debug.Log("mousedown" + Player.Instance.isRunning);
                    timerIsRunniing = true;

                    Debug.Log("playerrunningleft");

                    while (timerIsRunniing)
                    {
                        if (timeRemaining > 0f)
                        {
                            startTime += Time.deltaTime;

                            if (startTime >= timeRemaining)
                            {

                                readyToKick = true;
                                if (readyToKick)
                                {
                                    StartCoroutine(AttackCo());
                                    startTime = 0;
                                    timerIsRunniing = false;
                                    readyToKick = false;
                                    break;
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
                if (animatorStateInfo.IsName(Tags.DarkDoubleSpearKick) && animatorStateInfo.normalizedTime >= 0.9f)
                {
                    Debug.Log("darkdoublespearkick finished stop");
                    Stop();

                }



                if (animatorStateInfo.IsName(Tags.DarkKneeKick) && animatorStateInfo.normalizedTime >= 0.9f)
                {
                    Debug.Log("stop darkknee kick");
                    Stop();
                }
            }
            if (animatorStateInfo.IsName(Tags.Dash) && animatorStateInfo.normalizedTime >= 0.9f)
            {
                Debug.Log("dash stops");
                Stop();
            }



        }

        public void Stop()
        {
            Debug.Log("stop");
            Player.Instance.rigidbody2D.velocity = Vector2.zero;
        }
        public void ActivedAttack()
        {

            actived = true;
        }
        public void ActivedFalse()
        {
            actived = false;
        }
        private void DarkKneeKick()
        {
            Debug.Log("darkkneekick");


            animator.SetTrigger("isDarkKneeKick");
            Player.Instance.rigidbody2D.velocity = transform.right * Player.Instance.dashDirection * darkKneeKickForce;









        }

        public void DarkDoubleSpearKick()
        {
            if (Player.Instance.isSprinting)
            {
                Debug.Log("darkdoublespearkick");
                //animator.SetBool("isAttacking", true);
                animator.SetTrigger("isDarkDoubleSpearKick");
                //float time = GetCurrentAnimatorTime(animator, 1);
                //Debug.Log(time);
                kickDirection = (int)Player.Instance.movX;
                Player.Instance.rigidbody2D.velocity = transform.right *
                kickDirection * darkDoubleSpearKickForce;
            }
            else
            {
                Debug.Log("跑不夠多");
            }



        }



        private IEnumerator AttackCo()
        {

            Debug.Log("StartCoroutine");
            DarkDoubleSpearKick();

            yield return null;




            yield return new WaitForSeconds(1f);
            Player.Instance.rigidbody2D.velocity = Vector2.zero;
            readyToKick = false;

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

