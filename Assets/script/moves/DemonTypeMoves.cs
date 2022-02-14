using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Yushan.Enums;
using Yushan.movement;
namespace Yushan.DemonType
{
    public class DemonTypeMoves : MonoBehaviour
    {
        public static DemonTypeMoves Instance;
        //player object
        private GameObject player;
        private Transform playerTransform;
        private AnimatorStateInfo animatorStateInfo;
        private Animator animator;


        public void Awake()
        {
            Instance = this;
            player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<GameObject>();
            playerTransform = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Transform>();
            animator = GetComponent<Animator>();
            if (animator != null)
            {
                animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            }
        }
        private void LateUpdate()
        {


        }





        //public void DemonPowerPunch()
        //{
        //    //if (Input.GetMouseButtonDown(1) && isDemon && !isDemonPowerCharge)
        //    Debug.Log("clicked");

        //    demonPowerChargeTimer = 0;
        //    isDemonPowerCharge = true;
        //    Debug.Log("1");
        //    if (Input.GetMouseButton(1) && isDemon && isDemonPowerCharge)
        //    {

        //        while (demonPowerChargeTimer >= demonPowerChargeDuration)
        //        {
        //            Debug.Log("3" + demonPowerChargeTimer);
        //            isDemonPowerCharge = false;
        //            if (isDemonPowerCharge == false)
        //            {
        //                Effect.Instance.IsNotElectric();
        //                acceleration = 10f;
        //                maxSpeed = 15f;
        //                isDemonPowerPunch = true;
        //                demonPowerChargeTimer = 0;

        //                Debug.Log("4");

        //            }
        //            break;
        //        }
        //        if (Input.GetMouseButtonUp(2) && isDemon && isDemonPowerCharge)
        //        {

        //            Debug.Log("mouse up");
        //            isDemonPowerCharge = false;
        //            if (demonPowerChargeTimer < demonPowerChargeDuration)
        //            {

        //                Effect.Instance.IsNotElectric();
        //                Debug.Log("mouse up no power punch" + isDemonPowerCharge);
        //                acceleration = 2.5f;
        //                maxSpeed = 5f;


        //                isDemonPowerPunch = true;
        //                demonPowerChargeTimer = 0;

        //            }


        //        }

        //        Effect.Instance.ElectricEffectAfterDemonPowerPunch();
        //    }



        //    while (Player.Instance.playerDirection == Direction.left)
        //    {
        //        Debug.Log("left" + animatorStateInfo.IsName("demon power punch"));
        //        if (animatorStateInfo.IsName(Tags.DemonPowerCharge))
        //        {

        //        }
        //        if (animatorStateInfo.IsName("demon power punch transition") || animatorStateInfo.IsName(Tags.DemonPowerPunch2))
        //        {
        //            //targetPosition = Vector2.left * -10f * Time.deltaTime;


        //            //playerTransform.position = Vector2.SmoothDamp(playerTransform.position, targetPosition, ref currentVelocity, smoothing, maxSpeed);

        //            playerTransform.Translate(Vector2.left * curSpeed * Time.deltaTime);


        //            curSpeed += acceleration * Time.deltaTime;

        //            if (curSpeed > maxSpeed)
        //            {

        //                curSpeed = maxSpeed;
        //            }
        //            //playerTransform.Translate(Vector2.left * 10f * Time.deltaTime);

        //        }
        //        break;
        //    }

        //    while (Player.Instance.playerDirection == Direction.right)
        //    {
        //        Debug.Log("right" + animatorStateInfo.IsName("demon power punch"));
        //        if (animatorStateInfo.IsName(Tags.DemonPowerCharge))
        //        {

        //        }
        //        if (animatorStateInfo.IsName(Tags.DemonPowerPunch) || animatorStateInfo.IsName(Tags.DemonPowerPunchTransition))
        //        {
        //            Debug.Log("demon power punch transition");

        //            playerTransform.Translate(Vector2.right * curSpeed * Time.deltaTime);

        //            curSpeed += acceleration * Time.deltaTime;

        //            if (curSpeed > maxSpeed)
        //            {

        //                curSpeed = maxSpeed;
        //            }
        //            //playerTransform.Translate(Vector2.right * 10f * Time.deltaTime);
        //        }
        //        break;
        //    }

        //}


        //public void DemonIdle()
        //{
        //    isDemon = true;


        //    Debug.Log("isdemon" + isDemon);
        //    Debug.Log("isdemon idle" + Player.Instance.playerDirection);
    }
    //combo system method
    //attack

    //resetcombo
    //public void ResetComboFromPlayer()
    //{
    //    Player.Instance.ResetComBo();
    //}
    //public void ComboPossibleFromPlayer()
    //{
    //    Player.Instance.ComboPossible();
    //}
    //public void NextAtkFromPlayer()
    //{
    //    Player.Instance.NextAtk();
    //}
}

