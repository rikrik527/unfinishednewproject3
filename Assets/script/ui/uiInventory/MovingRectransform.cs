using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yushan.movement;
using Cinemachine;
public class MovingRectransform : MonoBehaviour
{
    public UiInventoryBar uiInventoryBar;
    [SerializeField] private GameObject objectToMove;
    public Vector2 objVector2;
    [SerializeField] private GameObject targetObject;
    public Vector2 targetVector2;
    public Vector2 _targetVector2;
    public Vector3 screenPos;
    public Vector2 screenPos2d;
    public Camera camera;
    [SerializeField] private GameObject followCamera;
    [SerializeField] private GameObject followCameraLeft;

    private void Awake()
    {
        camera = Camera.main;

    }
    private void Start()
    {
        targetVector2 = new Vector2(targetObject.gameObject.transform.position.x, targetObject.gameObject.transform.position.y);
        _targetVector2 = new Vector2(targetObject.gameObject.transform.position.x, targetObject.gameObject.transform.position.y - 100f);
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
        objectToMove.gameObject.transform.position = Vector2.MoveTowards(objectToMove.gameObject.transform.position, targetVector2, 1000 * Time.deltaTime);
        Debug.Log("moverect" + targetObject.name + "" + objectToMove.name);
        if (followCameraLeft.gameObject.active)
        {
            objectToMove.gameObject.transform.position = Vector2.MoveTowards(objectToMove.gameObject.transform.position, targetVector2, 1000 * Time.deltaTime);
        }
    }
}
