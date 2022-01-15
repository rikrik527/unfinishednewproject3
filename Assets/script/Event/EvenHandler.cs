using System.Collections.Generic;
using System;
using Yushan.Enums;


public delegate void MovementDelegate(float inputX, bool isWalking, bool isRunning, bool isSpringing, bool isDashing, bool isIdle, ToolEffect toolEffect, bool isDemonTransform, bool isDarkDoubleSpearKick);

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

    public static event MovementDelegate MovementEvent;
    public static event DemonDelegate DemonEvent;
    //demon actions event call for publishers
    public static void CallDemonEvent(bool isDemon, bool isDemonIdle, bool isDemonPunch, bool isDemonPowerCharge, bool isDemonPowerPunch, bool isDemonSecondPunch, bool isDemonPowerCharge2, bool isDemonPowerPunch2)
    {
        if (DemonEvent != null)
        {
            DemonEvent(isDemon, isDemonIdle, isDemonPunch, isDemonPowerCharge, isDemonPowerPunch, isDemonSecondPunch, isDemonPowerCharge2, isDemonPowerPunch2);
        }
    }

    //movement event call for publishers

    public static void CallMovementEvent(float inputX, bool isWalking, bool isRunning, bool isSprinting, bool isDashing, bool isIdle, ToolEffect toolEffect, bool isDemonTransform, bool isDarkDoubleSpearKick)
    {
        if (MovementEvent != null)
        {
            MovementEvent(inputX, isWalking, isRunning, isSprinting, isDashing, isIdle, toolEffect, isDemonTransform, isDarkDoubleSpearKick);
        }
    }


}
