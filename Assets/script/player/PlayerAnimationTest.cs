using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTest : MonoBehaviour
{
    public float inputX;
    public bool isWalking;
    public bool isRunning;
    public bool isDashing;
    public bool isIdle;
    public ToolEffect toolEffect;
    public bool isDemonTransform;
    public bool isDemon;
    public bool isDemonPunch;
    public bool isDemonPowerPunch;
    public bool idleLeft;
    public bool idleRight;

    private void Update()
    {
        EventHandler.CallMovementEvent(inputX, isWalking, isRunning, isDashing, isIdle, toolEffect, isDemonTransform, isDemon, isDemonPunch, isDemonPowerPunch, idleLeft, idleRight);
    }
}
