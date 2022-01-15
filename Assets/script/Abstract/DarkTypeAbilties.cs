using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yushan.Enums;
namespace Yushan.DarkAbilites
{
    public abstract class DarkTypeAbilties : MonoBehaviour
    {
        public float inputX;
        public bool isWalking;
        public bool isRunning;
        public bool isSprinting;
        public bool isDashing;
        public bool isIdle;
        public bool isDemonTransform;
        public ToolEffect toolEffect = ToolEffect.none;


    }
}

