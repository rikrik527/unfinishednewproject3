using System;

public delegate void MovementDelegate(float inputX, bool isWalking, bool isRunning, bool isSpringing, bool isDashing, bool isIdle, ToolEffect toolEffect, bool idleLeft, bool idleRight);

public delegate void DemonDelegate(bool isDemonTransform, bool isDemon, bool isDemonPowerCharge, bool isDemonPowerPunch, bool isDemonPunch, bool isDemonSecondPunch, bool isDemonIdle);
public static class EventHandler
{
    //movement event

    public static event MovementDelegate MovementEvent;
    public static event DemonDelegate DemonEvent;
    //demon actions event call for publishers
    public static void CallDemonEvent(bool isDemonTransform, bool isDemon, bool isDemonPowerCharge, bool isDemonPowerPunch, bool isDemonPunch, bool isDemonSecondPunch, bool isDemonIdle)
    {
        if (DemonEvent != null)
        {
            DemonEvent(isDemonTransform, isDemon, isDemonPowerCharge, isDemonPowerPunch, isDemonPunch, isDemonSecondPunch, isDemonIdle);
        }
    }

    //movement event call for publishers

    public static void CallMovementEvent(float inputX, bool isWalking, bool isRunning, bool isSprinting, bool isDashing, bool isIdle, ToolEffect toolEffect, bool idleLeft, bool idleRight)
    {
        if (MovementEvent != null)
        {
            MovementEvent(inputX, isWalking, isRunning, isSprinting, isDashing, isIdle, toolEffect, idleLeft, idleRight);
        }
    }


}
