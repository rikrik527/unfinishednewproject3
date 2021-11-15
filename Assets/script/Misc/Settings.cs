
using UnityEngine;

public static class Settings
{
    // playerMovement
    public const float sprintSpeed = 20.666f;
    public const float runningSpeed = 12.666f;
    public const float walkingSpeed = 2.666f;

    // player animation parameters
    public static int inputX;
    public static int isWalking;
    public static int isRunning;
    public static int isSprinting;
    public static int isDashing;
    public static int isIdle;
    public static int toolEffect;
    public static int isDemonPowerCharge;
    public static int isDemonTransform;
    public static int isDemon;
    public static int isDemonPunch;
    public static int isDemonPowerPunch;
    public static int isDemonIdle;
    public static int isDemonSecondPunch;
    public static int isDemonPowerPunch2;
    public static int isDemonPowerCharge2;
    // shared animation parameter
    public static int idleLeft;
    public static int idleRight;
    //demonactioncontrollers

    public static int isElectricEffect;





    static Settings()
    {
        //player Animation Parameters
        inputX = Animator.StringToHash("inputX");
        isWalking = Animator.StringToHash("isWalking");
        isRunning = Animator.StringToHash("inRunning");
        isDashing = Animator.StringToHash("isDashing");
        toolEffect = Animator.StringToHash("toolEffect");
        isIdle = Animator.StringToHash("isIdle");
        isDemonTransform = Animator.StringToHash("isDemonTransform");
        isDemon = Animator.StringToHash("isDemon");
        isDemonPunch = Animator.StringToHash("isDemonPunch");
        isDemonPowerPunch = Animator.StringToHash("isDemonPowerPunch");
        isDemonSecondPunch = Animator.StringToHash("isDemonSecondPunch");
        isSprinting = Animator.StringToHash("isSprinting");
        // shared animation parameters
        idleLeft = Animator.StringToHash("idleLeft");
        idleRight = Animator.StringToHash("idleRight");
        isDemonIdle = Animator.StringToHash("isDemonIdle");
        //demon action controller
        isElectricEffect = Animator.StringToHash("isElectricEffect");
        isDemonPowerCharge = Animator.StringToHash("isDemonPowerCharge");
        isDemonPowerCharge2 = Animator.StringToHash("isDemonPowerCharge2");
        isDemonPowerPunch2 = Animator.StringToHash("isDemonPowerPunch2");
    }
}
