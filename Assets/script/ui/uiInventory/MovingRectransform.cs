using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yushan.movement;
public class MovingRectransform : MonoBehaviour
{
    public UiInventoryBar uiInventoryBar;
    [SerializeField] private GameObject objectToMove;
    [SerializeField] private GameObject targetObject;
    public Vector3 screenPos;
    public Vector2 screenPos2d;
    public Camera camera;
    private void Awake()
    {
        camera = Camera.main;

    }
    private void Update()
    {
        screenPos = camera.WorldToScreenPoint(targetObject.gameObject.transform.position);

    }
    private void FixedUpdate()
    {
        MoveRect();
    }
    public void MoveRect()
    {
        objectToMove.gameObject.transform.position = Vector2.MoveTowards(objectToMove.gameObject.transform.position, screenPos, Time.deltaTime * 1000);
        Debug.Log("moverect" + targetObject.name + "" + objectToMove.name);
    }
}
