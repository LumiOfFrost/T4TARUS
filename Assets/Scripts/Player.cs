using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Variables")]
    Rigidbody rigidBody;
    public Transform cameraTransform;
    public Transform orientation;
    ConstantForce gravity;

    [Header("Movement")]
    public float movementSpeed = 5;
    public float jumpHeight = 10;
    public float rigidBodyDrag = 6;
    public float airDrag = 2;
    public float maxSlopeAngle = 45;
    public LayerMask groundMask;

    [Header("Air/Jumping")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float airMovementMultiplier = 0.4f;
    private bool grounded = false;
    private float coyoteTime = 0;
    private float jumpBuffer = 0;

    [Header("Stamina")]
    public float stamina = 100;
    public float maxStamina = 100;
    float staminaCooldown;

    Image sBar;

    Vector3 SlopeNormal()
    {

        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundMask, QueryTriggerInteraction.UseGlobal);
        return hit.normal;

    }

    private void Awake()
    {

        gravity = GetComponent<ConstantForce>();

        rigidBody = GetComponent<Rigidbody>();

    }

    private void Update()
    {

        grounded = Physics.CheckSphere(transform.position + Vector3.up * 0.25f, 0.29f, groundMask);

        if (grounded)
        {

            coyoteTime = 0.2f;

        }

        if (InputManager.Jump())
        {

            jumpBuffer = 0.3f;

        }

        coyoteTime -= Time.deltaTime;
        jumpBuffer -= Time.deltaTime;

    }

    private void FixedUpdate()
    {

        ManageDrag();
        GravityAmplifier();
        if (coyoteTime > 0 && jumpBuffer > 0 && Vector3.Angle(SlopeNormal(), Vector3.up) <= maxSlopeAngle)
        {

            rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
            Jump();
            coyoteTime = 0;
            jumpBuffer = 0;

        }
        PlayerMovement();
        MovementLimiter();

        if (transform.position.y < -50)
        {

            transform.position = new Vector3(0, 1, 0);
            rigidBody.velocity = Vector3.zero;

        }

    }

    void PlayerMovement()
    {

        float slopeAngle = Vector3.Angle(SlopeNormal(), Vector3.up);

        gravity.enabled = !(grounded && slopeAngle <= maxSlopeAngle);

        Vector3 movementVector = InputManager.MovementVector().x * orientation.right + InputManager.MovementVector().y * orientation.forward;

        movementVector = Vector3.ProjectOnPlane(movementVector, grounded ? SlopeNormal() : Vector3.up);

        if (slopeAngle <= maxSlopeAngle)
        {

            rigidBody.AddForce(movementVector.normalized * movementSpeed * 10 * (grounded ? 1 : airMovementMultiplier), ForceMode.Acceleration);

        } else if (grounded)
        {

            rigidBody.AddForce(movementVector.normalized * movementSpeed * 10 * (grounded ? 1 : airMovementMultiplier) * 0.2f, ForceMode.Acceleration);

        }

    }

    void ManageDrag()
    {

        rigidBody.drag = grounded ? rigidBodyDrag : airDrag;

    }

    void Jump()
    {

        rigidBody.AddForce(transform.up * jumpHeight, ForceMode.Impulse);

    }
    
    void GravityAmplifier()
    {

        if (rigidBody.velocity.y < 0 && !grounded)
        {

            rigidBody.AddForce(transform.up * Physics.gravity.y * (fallMultiplier - 1));

        } else if (InputManager.Fall() && !grounded)
        {

            rigidBody.AddForce(transform.up * Physics.gravity.y * (lowJumpMultiplier - 1));

        }

    }

    void MovementLimiter()
    {

        Vector3 flatVelocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);

        if (grounded && SlopeNormal() == Vector3.up)
        {

            if(flatVelocity.magnitude > movementSpeed)
            {

                flatVelocity = Vector3.Lerp(flatVelocity, flatVelocity.normalized * movementSpeed, 0.16f);

            }

            rigidBody.velocity = new Vector3(flatVelocity.x, rigidBody.velocity.y, flatVelocity.z);

        } else if (grounded && SlopeNormal() != Vector3.up)
        {

            if (rigidBody.velocity.magnitude > movementSpeed)
            {

                rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, rigidBody.velocity.normalized * movementSpeed, 0.16f);

            }

        }
        else
        {

            if (flatVelocity.magnitude > movementSpeed)
            {

                flatVelocity = Vector3.Lerp(flatVelocity, flatVelocity.normalized * movementSpeed, 0.2f);

            }

            rigidBody.velocity = new Vector3(flatVelocity.x, rigidBody.velocity.y, flatVelocity.z);


        }

    }

}
