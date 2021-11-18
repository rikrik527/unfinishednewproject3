using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DemonState
{
    public int DemonCode;
    public Demon_State Demon_State;

    public Animation animation;
    public string skillName;
    public string skillDescription;
    public string skillTriggerDescription;
    public SpriteRenderer sprite;
    public bool canBePickedUp;
    public bool canBeEaten;
    public bool canBeDropped;
    public bool canBeCarried;



}

