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

            if (animatorStateInfo.IsTag("motion") && Player.Instance.yushanType == Yushan_Type.darkenType)
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
            comboPossible = true;
        }
        public void NxtAtk()
        {
            if (!inputSmash)
            {
                if (comboStep == 2)
                {
                    // moves
                }
                if (comboStep == 3)
                {
                    // moves
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
            Debug.Log("smash attack");
            if (comboPossible)
            {
                comboPossible = false;
                inputSmash = true;
            }
        }
    }


}

