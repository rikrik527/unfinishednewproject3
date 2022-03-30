using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMovementController : MonoBehaviour
{
    //declare reference variables
    PlayerInput playerInput;
    
    // variables to store player input values
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;

    float movX;
    float movY;
    private float movementSpeed;
    public float movementAcceleration;
    public float sprintAcceleration;
    private Rigidbody2D rigidbody2D;
    private void Awake()
    {
        //intial set reference variables
        playerInput = new PlayerInput();
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerInput.playerController.move.started += context =>
        {
            currentMovementInput = context.ReadValue<Vector2>();
            currentMovement.x = currentMovementInput.x;
            currentMovement.y = currentMovementInput.y;
            isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
        };

    }
    private void OnEnable()
    {
        playerInput.playerController.Enable();
    }
    private void OnDisable()
    {
        playerInput.playerController.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        DarkMovement();
    }

    private void DarkMovement()
    {
        movX = Input.GetAxisRaw("Horizontal");
        movY = Input.GetAxisRaw("Vertical");
        if (isRunning)
        {
            rigidbody2D.AddForce(new Vector2(movX, 0f) * movementAcceleration);
            if (Mathf.Abs(rigidbody2D.velocity.x) > movementSpeed)
            {
                rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * movementSpeed, rigidbody2D.velocity.y);
            }

        }
        if (isSprint)
        {

            rigidbody2D.AddForce(new Vector2(movX, 0f) * sprintAcceleration);
            if (Mathf.Abs(rigidbody2D.velocity.x) > movementSpeed)
            {
                rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * movementSpeed, rigidbody2D.velocity.y);
            }

        }
    }
}
