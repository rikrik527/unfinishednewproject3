

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


using Yushan.Enums;
namespace Yushan.abilities
{
    public abstract class PlayerAbilities : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField]
        protected int health;
        [SerializeField]
        protected int darkPower;
        [SerializeField]
        protected int level;
        [SerializeField]
        protected float experience;
        [SerializeField]
        protected int skillPoint;
        protected bool darkPowerEmpty;
        protected bool healthLow;
        protected bool getsDarkPower;
        protected bool darkPowerFull;






        //combo
        public bool comboPossible;
        public int comboStep;
        public bool inputSmash;
















        //player object
        protected GameObject player;
        protected Transform playerTransform;
        protected SpriteRenderer spriteRenderer;



        //get current play clip info
        protected AnimatorClipInfo[] animatorClipInfo;
        protected string clipName;
        protected float clipLength;

        public virtual void Awake()
        {
            player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<GameObject>();
            playerTransform = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Transform>();
            spriteRenderer = GameObject.FindGameObjectWithTag(Tags.PlayerSprite).GetComponent<SpriteRenderer>();
        }
        public virtual void Start()
        {

        }
    }

}


