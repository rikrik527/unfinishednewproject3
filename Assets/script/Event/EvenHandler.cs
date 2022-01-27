using System.Collections.Generic;
using System;
using Yushan.Enums;

public delegate void DarkMovementDelegate(float movX, float movY, bool isRunning, bool isDarkDoubleSpearKick, bool isRunningRight, bool isRunningLeft, bool isSprint, bool isSprintRight, bool isSprintLeft, bool isDashing, bool isDarkSpinBack, bool isDarkKneeKick, bool isDarkCrossKick, bool isJumping, bool isFalling, bool isSprintJump, bool isSprintFall, bool isRunningJump, bool isRunningFall, bool isWallGrab, bool isWallJumping, bool isWallFall, bool isIdle, bool isDarkPowerUp);
public delegate void DarkComboDelegate(bool isDarkSpinKick, bool isDarkWhirlWindKick, bool isDarkSweepKick, bool isDarkDoubleSweepKick, bool isDarkSpinHeadKick);

public delegate void DemonDelegate(bool isDemon, bool isDemonIdle, bool isDemonPunch, bool isDemonPowerCharge, bool isDemonPowerPunch, bool isDemonSecondPunch, bool isDemonPowerCharge2, bool isDemonPowerPunch2);
public static class EventHandler
{

    public static event Action<InventoryLocation, List<InventoryItem>> InventoryUpdatedEvent;

    public static void CallInventoryUpdatedEvent(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {
        if (InventoryUpdatedEvent != null)
        {
            InventoryUpdatedEvent(inventoryLocation, inventoryList);
        }
    }
    //movement event


    public static event DemonDelegate DemonEvent;
    //demon actions event call for publishers
    public static void CallDemonEvent(bool isDemon, bool isDemonIdle, bool isDemonPunch, bool isDemonPowerCharge, bool isDemonPowerPunch, bool isDemonSecondPunch, bool isDemonPowerCharge2, bool isDemonPowerPunch2)
    {
        if (DemonEvent != null)
        {
            DemonEvent(isDemon, isDemonIdle, isDemonPunch, isDemonPowerCharge, isDemonPowerPunch, isDemonSecondPunch, isDemonPowerCharge2, isDemonPowerPunch2);
        }
    }
    //player moves
    public static event DarkMovementDelegate DarkMovementEvent;
    public static void CallDarkMovementEvent(float movX, float movY, bool isRunning, bool isDarkDoubleSpearKick, bool isRunningRight, bool isRunningLeft, bool isSprint, bool isSprintRight, bool isSprintLeft, bool isDashing, bool isDarkSpinBack, bool isDarkKneeKick, bool isDarkCrossKick, bool isJumping, bool isFalling, bool isSprintJump, bool isSprintFall, bool isRunningJump, bool isRunningFall, bool isWallGrab, bool isWallJumping, bool isWallFall, bool isIdle, bool isDarkPowerUp)
    {
        if (DarkMovementEvent != null)
        {
            DarkMovementEvent(movX, movY, isRunning, isDarkDoubleSpearKick, isRunningRight, isRunningLeft, isSprint, isSprintRight, isSprintLeft, isDashing, isDarkSpinBack, isDarkKneeKick, isDarkCrossKick, isJumping, isFalling, isSprintJump, isSprintFall, isRunningJump, isRunningFall, isWallGrab, isWallJumping, isWallFall, isIdle, isDarkPowerUp);
        }
    }
    //dark combo system call for publishers

    public static event DarkComboDelegate DarkComboEvent;
    public static void CallDarkComboEvent(bool isDarkSpinKick, bool isDarkWhirlWindKick, bool isDarkSweepKick, bool isDarkDoubleSweepKick, bool isDarkSpinHeadKick)
    {
        if (DarkComboEvent != null)
        {
            DarkComboEvent(isDarkSpinKick, isDarkWhirlWindKick, isDarkSweepKick, isDarkDoubleSweepKick, isDarkSpinHeadKick);
        }

    }
}
