using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class MovementAnimationController : MonoBehaviour
{
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        EventHandler.MovementEvent += SetAnimationParameter;
    }
    private void OnDisable()
    {
        EventHandler.MovementEvent -= SetAnimationParameter;
    }





    private void SetAnimationParameter(float inputX, bool isWalking, bool isRunning, bool isSprinting, bool isDashing, bool isIdle, ToolEffect toolEffect, bool isDemonPunch, bool isDemonPowerPunch, bool idleLeft, bool idleRight)
    {
        animator.SetBool(Settings.isSprinting, isSprinting);
        animator.SetFloat(Settings.inputX, inputX);
        animator.SetBool(Settings.isWalking, isWalking);
        animator.SetBool(Settings.isRunning, isRunning);
        animator.SetBool(Settings.isDashing, isDashing);
        animator.SetBool(Settings.isIdle, isIdle);


        animator.SetInteger(Settings.toolEffect, (int)toolEffect);


        if (isDemonPunch)
        {
            animator.SetTrigger(Settings.isDemonPunch);
        }


        if (isDemonPowerPunch)
        {
            animator.SetTrigger(Settings.isDemonPowerPunch);
        }
    }


}
