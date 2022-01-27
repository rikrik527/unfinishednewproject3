
using UnityEngine;

public static class Settings
{
    // playerMovement
    public const float sprintSpeed = 20.666f;
    public const float runningSpeed = 18.666f;
    public const float walkingSpeed = 2.666f;

    // player moves
    public static int isRunningRight;
    public static int isRunningLeft;
    public static int isSprint;
    public static int isSprintRight;
    public static int isSprintLeft;
    public static int isRunning;
    public static int isRunningJump;
    public static int isRunningFall;
    public static int isSprintJump;
    public static int isSprintFall;
    public static int isJumping;
    public static int isFalling;
    public static int isDashing;
    public static int isDarkKneeKick;
    public static int isDarkCrossKick;
    public static int isDarkDoubleSpearKick;
    public static int isWallGrab;
    public static int isWallJumping;
    public static int isWallFall;
    public static int isDarkSpintBack;
    public static int isDarkPowerUp;


    public static int playerInitialInventoryCapacity = 24;
    public static int playerMaximumInventoryCapacity = 48;

    // dark combo system
    public static int isDarkSpinKick;
    public static int isDarkWhirlWindKick;
    public static int isDarkSweepKick;
    public static int isDarkDoubleSweepKick;
    public static int isDarkSpinHeadKick;

    static Settings()
    {
        // dark combo
        isDarkSpinKick = Animator.StringToHash("isDarkSpinKick");
        isDarkWhirlWindKick = Animator.StringToHash("isDarkWhirlWindKick");
        isDarkSweepKick = Animator.StringToHash("isDarkSweepKick");
        isDarkDoubleSweepKick = Animator.StringToHash("isDarkDoubleSweepKick");
        isDarkSpinHeadKick = Animator.StringToHash("isDarkSpinHeadKick");


        //dark movement
        isRunningRight = Animator.StringToHash("isRunningRight");
        isRunningLeft = Animator.StringToHash("isRunningLeft");
        isSprint;
        isSprintLeft;
        isSprintRight;
        isRunning;
        isRunningJump;
        isRunningFall;
        isSprintJump;
        isSprintFall;
        isJumping;
        isFalling;
        isDashing;
        isDarkKneeKick;
        isDarkCrossKick;
        isDarkDoubleSpearKick;
        isWallGrab;
        isWallJumping;
        isWallFall;
        isDarkSpintBack;
        isDarkPowerUp;
    }

}
