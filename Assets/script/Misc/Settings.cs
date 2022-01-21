
using UnityEngine;

public static class Settings
{
    // playerMovement
    public const float sprintSpeed = 20.666f;
    public const float runningSpeed = 18.666f;
    public const float walkingSpeed = 2.666f;

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
    }

}
