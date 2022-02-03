    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Yushan.Enums;
    using Yushan.DarkAbilites;
    using Yushan.movement;
    using System;
    using Yushan.combo;
using UnityEngine.UI;
using TMPro;
    namespace Yushan.DarkType
    {
        public class DarkTypeMoves : MonoBehaviour
        {
            [Header("dark combat variables")]
            public bool isDarkDoubleSpearKick;
            public bool isDarkKneeKick;
            public bool isDarkCrossKick;

            [Header("dark knee kick variables")]
            public float darkKneeKickForce = 20f;
            public float darkKneeKickLength = .3f;


        [Header("tmp pro text")]
        public TMP_Text readyText;

            public float darkDoubleSpearKickForce = 20f;

            public float kickDirection;
            public bool readyToKick;
            public bool isDashMovesReady;


            private Animator animator;
            public static DarkTypeMoves Instance { get; private set; }



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

            }

            private void Update()
            {
                animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                kickDirection = (int)Player.Instance.movX;
            if (Player.Instance.yushan_Type == Yushan_Type.darkenType)
            {
                if (animatorStateInfo.IsTag("dash"))
                {


                    Debug.Log("dashtype");
                    if (isDashMovesReady == true)
                    {
                        Debug.Log("isDarkmoveReady");
                        if (Input.GetKeyDown(KeyCode.K))
                        {



                            Debug.Log("k press is dash knee kick" + "animator is tag dash"+ animatorStateInfo.IsTag("dash"));


                            //Player.Instance.rigidbody2D.velocity = Vector2.zero;
                            //Player.Instance.dashDirection = (int)Player.Instance.movX;
                            DarkKneeKick();



                            EventHandler.CallDarkCombatEvent(isDarkDoubleSpearKick, isDarkKneeKick, isDarkCrossKick);
                        }
                    }



                }


                if (animatorStateInfo.IsTag("running") && Settings.readyToPerformRunningMoves == true)
                {
                    Debug.Log("animator is tag running"+animatorStateInfo.IsTag("running"));

                    if (Input.GetKeyDown(KeyCode.K))
                    {
                        StartCoroutine(AttackCo());

                        Player.Instance.startTime = 0;
                        Player.Instance.timerIsRunning = false;
                      

                        EventHandler.CallDarkCombatEvent(isDarkDoubleSpearKick, isDarkKneeKick, isDarkCrossKick);

                    }










                }




            }


            }










            private void LateUpdate()
            {
                //if (animator != null)
                //{
                //    Debug.Log("animator is not null");
                //    if (animatorStateInfo.IsName(Tags.DarkDoubleSpearKick) && animatorStateInfo.normalizedTime >= 0.9f)
                //    {
                //        Debug.Log("darkdoublespearkick finished stop");
                //        Stop();

                //    }



                //    if (animatorStateInfo.IsName(Tags.DarkKneeKick) && animatorStateInfo.normalizedTime >= 0.9f)
                //    {
                //        Debug.Log("stop darkknee kick");
                //        Stop();
                //    }
                //}



            }

            public void Stop()
            {
                Debug.Log("stop");
                Player.Instance.rigidbody2D.velocity = Vector2.zero;
            }

            private void DarkKneeKick()
            {
                Debug.Log("darkkneekick");

                isDarkKneeKick = true;










            }
            public void DarkKneeKickForce()
            {
                float kickStartTime = Time.time;
                Debug.Log("trigger dark knee kick force");

                Player.Instance.rigidbody2D.velocity = Vector2.zero;
                Player.Instance.rigidbody2D.gravityScale = 0f;
                Player.Instance.rigidbody2D.drag = 0f;
                Vector2 dir = new Vector2(kickDirection, 0f);
                while (Time.time < kickStartTime + darkKneeKickLength)
                {
                    Player.Instance.rigidbody2D.velocity = dir.normalized * darkKneeKickForce;
                    break;
                }
                isDarkKneeKick = false;
                Debug.Log("isdarkkneekick" + isDarkKneeKick);
            }
            public void IsDashMovesReady()
            {
                isDashMovesReady = true;
            }
            public void IsDashMovesNotReady()
            {
                isDashMovesReady = false;
            }
            public void DarkDoubleSpearKick()
            {

                Debug.Log("darkdoublespearkick");
                //animator.SetBool("isAttacking", true);
                isDarkDoubleSpearKick = true;
                //float time = GetCurrentAnimatorTime(animator, 1);
                //Debug.Log(time);

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
                readyToKick = false;
                Settings.readyToPerformRunningMoves = false;

            }
            public void DarkCrossKick()
            {
                isDarkCrossKick = true;
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

