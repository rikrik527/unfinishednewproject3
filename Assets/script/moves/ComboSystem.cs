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

        public bool isDarkComboSystem;

        //combo system parameters
        private bool isDarkSpinKick;
        private bool isDarkWhirlWindKick;
        private bool isDarkSweepKick;
        private bool isDarkDoubleSweepKick;
        private bool isDarkSpinHeadKick;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            animator = GetComponent<Animator>();


        }
        private void Update()
        {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (Player.Instance.yushan_Move_Type == Yushan_Move_type.darkComboType)
            {
                Debug.Log("isdarkcombosystem is true");
                isDarkComboSystem = true;
            }
            else
            {
                Debug.Log("isdarkcombosystem" + isDarkComboSystem);
                isDarkComboSystem = false;
            }

            if (Player.Instance.yushanType == Yushan_Type.darkenType)
            {

                Debug.Log("combosystem");
                if (Input.GetKeyDown(KeyCode.K) && isDarkComboSystem)
                {
                    NormalAttack();
                }
                if (Input.GetKeyDown(KeyCode.L) && isDarkComboSystem)
                {
                    SmashAttack();
                }
                EventHandler.CallDarkComboEvent(isDarkSpinKick, isDarkWhirlWindKick, isDarkSweepKick, isDarkDoubleSweepKick, isDarkSpinHeadKick);
            }

        }
        public void ComboPossible()
        {
            Debug.Log("composible");
            comboPossible = true;
        }
        public void NxtAtk()
        {

            if (!inputSmash)
            {
                if (comboStep == 2)
                {
                    Debug.Log("combo2");
                    // moves
                    isDarkWhirlWindKick = true;
                }
                if (comboStep == 3)
                {
                    // moves
                    isDarkSweepKick = true;
                }
                if (comboStep == 4)
                {
                    isDarkDoubleSweepKick = true;
                }
                if (comboStep == 5)
                {
                    isDarkSpinHeadKick = true;
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
            //moves reset
            isDarkSpinKick = false;
            isDarkWhirlWindKick = false;
            isDarkSweepKick = false;
            isDarkDoubleSweepKick = false;
            isDarkSpinHeadKick = false;
        }

        public void NormalAttack()
        {

            Debug.Log("normal attack");
            if (comboStep == 0)
            {
                //first normal move
                isDarkSpinKick = true;
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

