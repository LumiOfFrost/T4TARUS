using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    GameObject player;

    Rigidbody rigidBody;

    public LayerMask groundMask;
    private Vector3 slopeNormal;

    [Header("Movement")]

    public float movementSpeed = 5;
    public float jumpHeight = 10;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float airMultiplier = 0.4f;

    private float coyoteTime = 0;
    private float jumpBuffer = 0;

    public float stamina = 100;
    public float maxStamina = 100;
    float staminaCooldown;

    Image sBar;

    // Start is called before the first frame update
    void Awake()
    {

        player = this.gameObject;

        rigidBody = player.GetComponent<Rigidbody>();

        sBar = GameObject.Find("Canvas").transform.Find("StaminaBar").gameObject.GetComponent<Image>();

        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {

        Cursor.lockState = CursorLockMode.Locked;

        if (InputManager.Jump())
        {
            jumpBuffer = 0.2f;
        }

        if(staminaCooldown < 0)
        {

            stamina += Time.deltaTime * 33;

        }

        stamina = Mathf.Clamp(stamina, 0, maxStamina);

        staminaCooldown -= Time.deltaTime;

        jumpBuffer -= Time.deltaTime;
        coyoteTime -= Time.deltaTime;

        UpdateBars();

    }

    private void UpdateBars()
    {

        sBar.fillAmount = Mathf.Lerp(sBar.fillAmount, stamina / maxStamina, 0.08f);

    }

    bool IsOnSlope()
    {

        RaycastHit ray;
        Physics.Raycast(transform.position + 0.05f * transform.up, Vector3.down, out ray, 0.1f, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal);

        slopeNormal = ray.normal;

        return ray.normal != Vector3.up;

    }

    private void FixedUpdate()
    {

        rigidBody.drag = coyoteTime > 0 ? 5 : 0;

        Vector3 movementVector = InputManager.MovementVector().x * transform.right + InputManager.MovementVector().y * transform.forward;

        if (Physics.CheckSphere(transform.position + transform.up * 0.25f, 0.29f, groundMask, QueryTriggerInteraction.UseGlobal))
        {
            coyoteTime = 0.2f;
            staminaCooldown = 0;
        }

        if (rigidBody.velocity.y < 0 && coyoteTime <= 0)
        {
            rigidBody.velocity += Vector3.up * -39.24f * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (InputManager.Fall() && coyoteTime <= 0)
        {
            rigidBody.velocity += Vector3.up * -39.24f * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (coyoteTime > 0 && jumpBuffer > 0)
        {

            rigidBody.AddForce(jumpHeight * Vector3.up, ForceMode.Impulse);
            jumpBuffer = 0;
            coyoteTime = 0;

        }

        if (IsOnSlope())
        {

            movementVector = Vector3.ProjectOnPlane(movementVector, slopeNormal);

        }

        rigidBody.AddForce(movementVector.normalized * movementSpeed * 10f * (coyoteTime > 0 ? 1 : airMultiplier), ForceMode.Force);

        Vector3 clampedVeloc = Vector3.ClampMagnitude(new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z), movementSpeed);

        rigidBody.velocity = new Vector3(clampedVeloc.x, rigidBody.velocity.y, clampedVeloc.z);

    }

}
