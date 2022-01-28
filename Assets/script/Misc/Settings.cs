
using UnityEngine;

public static class Settings
{
    // playerMovement
    public const float sprintSpeed = 20.666f;
    public const float runningSpeed = 18.666f;
    public const float walkingSpeed = 2.666f;

    // player moves
    public static int movX;

    public static int movY;
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
    public static int isDarkSpinBack;
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
        movY = Animator.StringToHash("movY");
        movX = Animator.StringToHash("movX");
        isRunningRight = Animator.StringToHash("isRunningRight");
        isRunningLeft = Animator.StringToHash("isRunningLeft");
        isSprint= Animator.StringToHash("isSprint");
        isSprintLeft= Animator.StringToHash("isSprintLeft");
        isSprintRight= Animator.StringToHash("isSprintRight");
        isRunning= Animator.StringToHash("isRunning");
        isRunningJump= Animator.StringToHash("isRunningJump");
        isRunningFall= Animator.StringToHash("isRunningFall");
        isSprintJump= Animator.StringToHash("isSprintJump");
        isSprintFall= Animator.StringToHash("isSprintFall");
        isJumping= Animator.StringToHash("isJumping");
        isFalling= Animator.StringToHash("isFalling");
        isDashing= Animator.StringToHash("isDashing");
        isDarkKneeKick= Animator.StringToHash("isDarkKneeKick");
        isDarkCrossKick= Animator.StringToHash("isDarkCrossKick");
        isDarkDoubleSpearKick= Animator.StringToHash("isDarkDoubleSpearKick");
        isWallGrab= Animator.StringToHash("isWallGrab");
        isWallJumping= Animator.StringToHash("isWallJump");
        isWallFall= Animator.StringToHash("isWallFall");
        isDarkSpinBack= Animator.StringToHash("isDarkSpinBack");
        isDarkPowerUp= Animator.StringToHash("isDarkPowerUp");
    }

}
