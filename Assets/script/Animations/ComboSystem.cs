using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Yushan.comboSystem
{
    public class ComboSystem : MonoBehaviour
    {
        private Animator animator;
        public int comboStep;
        public bool comboPossible;
        public bool inputSmash;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void ComboPossible()
        {
            comboPossible = true;
        }
        public void ResetCombo()
        {
            Debug.Log("resetcombo");
            comboPossible = false;
            inputSmash = false;
            comboStep = 0;
        }
        public void NxtAtk()
        {
            if (!inputSmash)
            {
                if (comboStep == 2)
                {
                    animator.Play(Tags.DemonSecondPunch);
                }
                if (comboStep == 3)
                {
                    Debug.Log("nothing");
                }
            }
            if (inputSmash)
            {
                if (comboStep == 1)
                {
                    animator.Play(Tags.DemonPowerPunch);
                }
                if (comboStep == 2)
                {
                    animator.Play(Tags.DemonPowerPunch2);
                }
            }
        }
        public void NormalAtk()
        {
            if (comboStep == 0)
            {
                animator.Play(Tags.DemonPunch);
                comboStep = 1;
                return;
            }
            if (comboStep != 0)
            {
                comboPossible = false;
                comboStep += 1;
            }
        }
        public void SmashAtk()
        {
            if (comboPossible)
            {
                comboPossible = false;
                inputSmash = true;
            }
        }

        void Update()
        {

        }
    }
}

