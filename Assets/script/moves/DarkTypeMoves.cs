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
        public bool canKneeKick;


        [Header("tmp pro text")]
        public TMP_Text readyText;

        [Header("DarkDoublespearkick")]
            public float darkDoubleSpearKickForce = 20f;
        public float darkDoubleSpearKickLength = .3f;
        public bool canDSKick;



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
                
            if (Player.Instance.yushan_Type == Yushan_Type.darkenType)
            {
              


                    Debug.Log("dashtype");
                    if (isDashMovesReady == true)
                    {
                        Debug.Log("isDarkmoveReady");
                        if (Input.GetKeyDown(KeyCode.K))
                        {






                        //Player.Instance.rigidbody2D.velocity = Vector2.zero;
                        //Player.Instance.dashDirection = (int)Player.Instance.movX;
                        canKneeKick = true;



                            
                        }
                    }



                


               

                    if (Input.GetKeyDown(KeyCode.K))
                    {
                    if (Settings.readyToPerformRunningMoves == true)
                    {
                        Debug.Log("animator is tag running" + animatorStateInfo.IsTag("running"));
                        canDSKick = true;

                       

                        

                    }










                }




            }


            }



        private void FixedUpdate()
        {
            if(Player.Instance.yushanType == Yushan_Type.darkenType)
            {
                if (canKneeKick)
                {
                    DarkKneeKick();
                }
                if (canDSKick)
                {
                    DarkDoubleSpearKick();
                }
                EventHandler.CallDarkCombatEvent(isDarkDoubleSpearKick, isDarkKneeKick, isDarkCrossKick);

            }

        }






       

          
        public void IsDashMovesReady()
        {
            Debug.Log("isdashmoveready");
            isDashMovesReady = true;
        }
        public void IsDashMovesNotReady()
        {
            Debug.Log("isdashmoves false");
            isDashMovesReady = false;
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
            
            public void DarkDoubleSpearKick()
            {
            float kickStartTime = Time.time;
            Debug.Log("darkdoublespearkick");
                //animator.SetBool("isAttacking", true);
                isDarkDoubleSpearKick = true;
            //float time = GetCurrentAnimatorTime(animator, 1);
            //Debug.Log(time);

            Player.Instance.rigidbody2D.velocity = Vector2.zero;
            Player.Instance.rigidbody2D.gravityScale = 0f;
            Player.Instance.rigidbody2D.drag = 0f;
            Vector2 dir = new Vector2(kickDirection, 0f);
            while (Time.time < kickStartTime + darkDoubleSpearKickLength)
            {
                Player.Instance.rigidbody2D.velocity = dir.normalized * darkDoubleSpearKickForce;
                break;
            }
            isDarkKneeKick = false;

      



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

