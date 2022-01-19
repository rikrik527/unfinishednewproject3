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
    public Vector3 screenPosObj;
    public Vector2 screenPos2dObj;
    public Vector2 screenPos3dObj;
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
        screenPosObj = camera.WorldToScreenPoint(objectToMove.gameObject.transform.position);
        screenPos2d = new Vector2(screenPos.x + 160, screenPos.y + 150);
        screenPos3d = new Vector2(screenPos.x, screenPos.y + 150);
        screenPos2dObj = new Vector2(screenPos.x, screenPos.y);
        screenPos3dObj = new Vector2(screenPos.x, screenPos.y);
        MoveRect();
    }
    private void FixedUpdate()
    {
        //MovetowardRect();
    }
    public void MoveRect()
    {
        objectToMove.gameObject.transform.LeanSetPosX(targetObject.gameObject.transform.position.x + 150);
        objectToMove.gameObject.transform.LeanSetPosY(targetObject.gameObject.transform.position.y + 150);
        float distance = Vector2.Distance(screenPos2dObj, screenPos2d);
        Debug.Log(distance);
        if (Vector2.Distance(screenPos2dObj, screenPos2d) > 210)
        {
            objectToMove.gameObject.transform.LeanSetPosX((targetObject.gameObject.transform.position.x + 150) - 219);
            objectToMove.gameObject.transform.LeanSetPosY(targetObject.gameObject.transform.position.y + 150);
        }
        if (followCameraLeft.active)
        {
            objectToMove.gameObject.transform.LeanSetPosX(targetObject.gameObject.transform.position.x);
            objectToMove.gameObject.transform.LeanSetPosY(targetObject.gameObject.transform.position.y + 150);
        }

        //objectToMove.gameObject.transform.position = new Vector2(screenPos.x, screenPos.y + 80);
        //Debug.Log("moverect" + targetObject.name + "" + objectToMove.name);
        //if (followCameraLeft.active)
        //{
        //    Debug.Log("followcameraleft");
        //    objectToMove.gameObject.transform.position = new Vector2(screenPos.x - 10, screenPos.y + 50);
        //}
        //if (followCamera.active)
        //{
        //    Debug.Log("followcamera active");
        //    objectToMove.gameObject.transform.position = new Vector2(screenPos.x, screenPos.y + 80);
        //}
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
