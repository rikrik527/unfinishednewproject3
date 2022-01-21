using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yushan.movement;
using UnityEngine.UI;
public class MovingRectransform : MonoBehaviour
{
    public GameObject movingRec;

    private void Update()
    {

        Vector2 followPos = Camera.main.WorldToScreenPoint(this.transform.position);
        movingRec.gameObject.transform.position = followPos;

    }
}
