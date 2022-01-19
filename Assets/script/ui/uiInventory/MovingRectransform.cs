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
    public Vector2 screenPos3d;
    public Camera camera;
    [SerializeField] private GameObject followCamera;
    [SerializeField] private GameObject followCameraLeft;
    private void Awake()
    {
        camera = Camera.main;

    }
    private void Update()
    {
        screenPos = camera.WorldToScreenPoint(targetObject.gameObject.transform.position);
        screenPos2d = new Vector2(screenPos.x, screenPos.y + 80);
        screenPos3d = new Vector2(screenPos.x - 10, screenPos.y + 50);
        //MoveRect();
    }
    private void FixedUpdate()
    {
        MovetowardRect();
    }
    public void MoveRect()
    {
        objectToMove.gameObject.transform.position = new Vector2(screenPos.x, screenPos.y + 80);
        Debug.Log("moverect" + targetObject.name + "" + objectToMove.name);
        if (followCameraLeft.active)
        {
            Debug.Log("followcameraleft");
            objectToMove.gameObject.transform.position = new Vector2(screenPos.x - 10, screenPos.y + 50);
        }
        if (followCamera.active)
        {
            Debug.Log("followcamera active");
            objectToMove.gameObject.transform.position = new Vector2(screenPos.x, screenPos.y + 80);
        }
    }
    public void MovetowardRect()
    {
        float speed = 1000f;
        objectToMove.gameObject.transform.position = Vector2.MoveTowards(objectToMove.gameObject.transform.position, screenPos2d, speed * Time.deltaTime);
        if (followCameraLeft.active)
        {
            objectToMove.gameObject.transform.position = Vector2.MoveTowards(objectToMove.gameObject.transform.position, screenPos3d, speed * Time.deltaTime);
        }
    }
}
