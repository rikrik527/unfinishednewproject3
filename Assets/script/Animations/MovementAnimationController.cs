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





    private void SetAnimationParameter(float inputX, bool isWalking, bool isRunning, bool isSprinting, bool isDashing, bool isIdle, ToolEffect toolEffect, bool idleLeft, bool idleRight)
    {
        animator.SetBool(Settings.isSprinting, isSprinting);
        animator.SetFloat(Settings.inputX, inputX);
        animator.SetBool(Settings.isWalking, isWalking);
        animator.SetBool(Settings.isRunning, isRunning);
        animator.SetBool(Settings.isDashing, isDashing);
        animator.SetBool(Settings.isIdle, isIdle);


        animator.SetInteger(Settings.toolEffect, (int)toolEffect);






    }


}
