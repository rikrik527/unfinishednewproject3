public delegate void MovementDelegate(float inputX, bool isWalking, bool isRunning, bool isDashing, bool isIdle, ToolEffect toolEffect, bool isDemonPunch, bool isDemonPowerPunch, bool idleLeft, bool idleRight);

public delegate void DemonDelegate(bool isDemonTransform, bool isDemon);
public static class EventHandler
{
    //movement event

    public static event MovementDelegate MovementEvent;
    public static event DemonDelegate DemonEvent;
    //demon actions event call for publishers
    public static void CallDemonEvent(bool isDemonTransform, bool isDemon)
    {
        if (DemonEvent != null)
        {
            DemonEvent(isDemonTransform, isDemon);
        }
    }

    //movement event call for publishers

    public static void CallMovementEvent(float inputX, bool isWalking, bool isRunning, bool isDashing, bool isIdle, ToolEffect toolEffect, bool isDemonPunch, bool isDemonPowerPunch, bool idleLeft, bool idleRight)
    {
        if (MovementEvent != null)
        {
            MovementEvent(inputX, isWalking, isRunning, isDashing, isIdle, toolEffect, isDemonPunch, isDemonPowerPunch, idleLeft, idleRight);
        }
    }
}
