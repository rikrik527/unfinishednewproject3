using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yushan.DemonType;

namespace Yushan.DemonAbilities
{
    public abstract class DemonTypeAbilities : MonoBehaviour
    {
        public bool isDemon;
        public bool isDemonPowerCharge;
        public bool isDemonPunch;
        public bool isDemonIdle;
        public bool isDemonPowerPunch;
        public bool isDemonSecondPunch;
        public bool isDemonPowerCharge2;
        public bool isDemonPowerPunch2;



        //timer
        protected const float demonPowerChargeDuration = 1f;
        protected float demonPowerChargeTimer = 0f;
        protected float demonPowerChargeTimer2 = 0f;
        protected float speedFactor = 2.3f;
        //demon
        //demon power punch
        protected Vector2 targetPosition;//where you want the gameobject to move to
        protected Vector2 currentVelocity = Vector2.zero;//this is used inside the function dont touch
        protected float smoothing = 0.5f;
        protected float maxSpeed = 10f;
        protected float acceleration = 5.0f;

        protected float curSpeed = 0.0f;




    }
}

