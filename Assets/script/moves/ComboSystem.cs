using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yushan.DemonType;
using Yushan.Enums;
using Yushan.movement;
namespace Yushan.combo
{
    public class ComboSystem : MonoBehaviour
    {
        public static ComboSystem Instance;
        //combo fileds
        public Animator animator;
        public bool comboPossible;
        public int comboStep;
        public bool inputSmash;
        public AnimatorStateInfo animatorStateInfo;
        public Yushan_Type yushanType;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            animator = GetComponent<Animator>();
            yushanType = Yushan_Type.darkenType;
        }
        private void Update()
        {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (yushanType == Yushan_Type.darkenType)
            {
                Debug.Log("combosystem");
                if (Input.GetMouseButtonDown(0))
                {
                    NormalAttack();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    SmashAttack();
                }

            }

        }
        public void ComboPossible()
        {
            Debug.Log("composible");
            comboPossible = true;
        }
        public void NxtAtk()
        {
            Debug.Log("excuted");
            if (!inputSmash)
            {
                if (comboStep == 2)
                {
                    Debug.Log("combo2");
                    // moves
                    animator.Play(Tags.DarkWhirlWindKick);
                }
                if (comboStep == 3)
                {
                    // moves
                    animator.Play(Tags.DarkSweepKick);
                }
                if (comboStep == 4)
                {
                    animator.Play(Tags.DarkDoubleSweepKick);
                }
                if (comboStep == 5)
                {
                    animator.Play(Tags.DarkSpinHeadKick);
                }
            }
            if (inputSmash)
            {
                if (comboStep == 1)
                {
                    //moves
                }
                if (comboStep == 2)
                {
                    // moves
                }
                if (comboStep == 3)
                {
                    //moves
                }
            }
        }

        public void ResetCombo()
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
                //first normal move
                animator.Play("dark spin kick");
                comboStep = 1;
                Debug.Log("combostep" + comboStep);
                return;
            }
            if (comboStep != 0)
            {
                Debug.Log("combo!=0");
                if (comboPossible)
                {
                    Debug.Log("combopossible" + comboPossible);
                    comboPossible = false;
                    comboStep += 1;
                    Debug.Log("combo" + comboStep);

                }
            }
            Debug.Log("combo" + comboStep);
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
    }


}

