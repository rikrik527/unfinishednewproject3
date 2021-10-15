using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonActions : MonoBehaviour
{


    private Animator animator;
    private Rigidbody2D rigidbody2D;
    private MovementAnimationController movementAnimationController;


    private bool isDemonTransform;

    private bool isDemonPunch;
    private bool isDemonPowerPunch;

    private bool isDemon;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        movementAnimationController = GetComponentInChildren<MovementAnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        DemonTransform();
        //DemonPunch();

    }
    private void DemonTransform()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("demon transform");

            movementAnimationController.IsDemonTransform();
            Debug.Log(isDemonTransform);
            StartCoroutine(BecomeDemon());

        }


    }
    IEnumerator BecomeDemon()
    {
        yield return new WaitForSeconds(1f);
        isDemon = true;
        animator.SetBool(Settings.isDemon, true);
        Debug.Log(isDemon);
    }
    private void DemonPunch()
    {
        if (isDemon && Input.GetMouseButtonDown(0))
        {
            isDemonPunch = true;
            Debug.Log("punch");
            animator.SetTrigger(Settings.isDemonPunch);

            Debug.Log("isDemonPunch" + isDemonPunch);
        }
    }
    //public void DemonPowerPunch()
    //{
    //    if (isDemonPunch == true && Input.GetMouseButtonDown(1))
    //    {
    //        isDemonPunch = false;
    //        animator.SetTrigger(Settings.isDemonPowerPunch);
    //    }
    //}
}
