public delegate void MovementDelegate(float inputX, bool isWalking, bool isRunning, bool isDashing, bool isIdle, ToolEffect toolEffect, bool isDemonTransform, bool isDemon, bool isDemonPunch, bool isDemonPowerPunch, bool idleLeft, bool idleRight);


public static class EventHandler
{
    //movement event

    public static event MovementDelegate MovementEvent;

    //movement event call for publishers

    public static void CallMovementEvent(float inputX, bool isWalking, bool isRunning, bool isDashing, bool isIdle, ToolEffect toolEffect, bool isDemonTransform, bool isDemon, bool isDemonPunch, bool isDemonPowerPunch, bool idleLeft, bool idleRight)
    {
        if (MovementEvent != null)
        {
            MovementEvent(inputX, isWalking, isRunning, isDashing, isIdle, toolEffect, isDemonTransform, isDemon, isDemonPunch, isDemonPowerPunch, idleLeft, idleRight);
        }
    }
}
