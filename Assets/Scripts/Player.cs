using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Variables")]
    CharacterController characterController;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform orientation;
    Velocity velocity;

    [Header("Movement")]
    public float movementSpeed = 5;
    public float jumpHeight = 10;
    Vector3 movementVector;

    [Header("Air/Jumping")]
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;
    private bool grounded = false;
    private float coyoteTime = 0;
    private float jumpBuffer = 0;

    [Header("Stamina")]
    public float stamina = 100;
    public float maxStamina = 100;
    float staminaCooldown;

    Image sBar;

    private void Awake()
    {

        velocity = GetComponent<Velocity>();

        characterController = GetComponent<CharacterController>();

    }

    private void Update()
    {

        if (InputManager.Jump())
        {

            jumpBuffer = 0.3f;

        }

        coyoteTime -= Time.deltaTime;
        jumpBuffer -= Time.deltaTime;

    }

    private void FixedUpdate()
    {

        grounded = characterController.isGrounded;

        if (grounded)
        {

            if (velocity.velocity.y < 0) velocity.velocity.y = 0;

            coyoteTime = 0.2f;

        }

        GravityAmplifier();

        if (coyoteTime > 0 && jumpBuffer > 0)
        {

            velocity.velocity = new Vector3(velocity.velocity.x, 0, velocity.velocity.z);
            Jump();
            coyoteTime = 0;
            jumpBuffer = 0;

        }
        PlayerMovement();

        if (transform.position.y < -50)
        {

            transform.position = new Vector3(0, 1, 0);
            velocity.velocity = Vector3.zero;

        }

    }

    void PlayerMovement()
    {

        movementVector = Vector3.Lerp(movementVector, InputManager.MovementVector().x * orientation.right + InputManager.MovementVector().y * orientation.forward, 0.2f);

        if (grounded)
        {
            characterController.Move(movementVector * movementSpeed * Time.deltaTime);
        }
        else
        {

            if (movementVector.x > 0 && velocity.velocity.x + movementVector.x * movementSpeed * Time.deltaTime < movementSpeed)
            {
                velocity.velocity.x += movementVector.x * Time.deltaTime * movementSpeed * velocity.velocity.x >= -movementSpeed ? 1 : 0.4f;
            }
            if (movementVector.x < 0 && velocity.velocity.x + movementVector.x * movementSpeed * Time.deltaTime > -movementSpeed)
            {
                velocity.velocity.x += movementVector.x * Time.deltaTime * movementSpeed * velocity.velocity.x <= movementSpeed ? 1 : 0.4f;
            }
            if (movementVector.z > 0 && velocity.velocity.z + movementVector.z * movementSpeed * Time.deltaTime < movementSpeed)
            {
                velocity.velocity.z += movementVector.z * Time.deltaTime * movementSpeed * velocity.velocity.z >= -movementSpeed ? 1 : 0.4f;
            }
            if (movementVector.z < 0 && velocity.velocity.z + movementVector.z * movementSpeed * Time.deltaTime > -movementSpeed)
            {
                velocity.velocity.z += movementVector.z * Time.deltaTime * movementSpeed * velocity.velocity.z <= movementSpeed ? 1 : 0.4f;
            }

        }

    }

    void Jump()
    {

        velocity.velocity += orientation.up * jumpHeight;

    }
    
    void GravityAmplifier()
    {

        if (characterController.velocity.y < 0 && !grounded)
        {

            velocity.velocity.y += velocity.gravityValue * Time.deltaTime * (fallMultiplier - 1);

        } else if (InputManager.Fall() && !grounded)
        {

            velocity.velocity.y += velocity.gravityValue * Time.deltaTime * (lowJumpMultiplier - 1);

        }

    }


}
