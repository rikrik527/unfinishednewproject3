using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void IsDemonTransform()
    {
        Debug.Log("isdemontransform");
        animator.SetTrigger(Settings.isDemonTransform);


    }

    public void DemonPowerPunch()
    {
        if (Input.GetMouseButtonDown(1))
        {

            Debug.Log("fired");
            animator.SetTrigger(Settings.isDemonPowerPunch);
        }

    }

    private void SetAnimationParameter(float inputX, bool isWalking, bool isRunning, bool isDashing, bool isIdle, ToolEffect toolEffect, bool isDemonTransform, bool isDemon, bool isDemonPunch, bool isDemonPowerPunch, bool idleLeft, bool idleRight)
    {
        animator.SetFloat(Settings.inputX, inputX);
        animator.SetBool(Settings.isWalking, isWalking);
        animator.SetBool(Settings.isRunning, isRunning);
        animator.SetBool(Settings.isDashing, isDashing);
        animator.SetBool(Settings.isDemon, isDemon);

        animator.SetInteger(Settings.toolEffect, (int)toolEffect);

        if (isDemonTransform)
        {

            animator.SetTrigger(Settings.isDemonTransform);

        }
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
