using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    GameObject playerCamera;
    GameObject player;

    Rigidbody rb;

    public Vector3 velocity;
    public Vector3 localEulers;

    [Header("Movement")]

    public float movementSpeed = 5;
    public float mouseSensitivity = 3;
    public float jumpHeight = 10;
    public float gravityScale = 3;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private float coyoteTime = 0;
    private float jumpBuffer = 0;

    public float stamina = 100;
    public float maxStamina = 100;
    float staminaCooldown;
    public Transform head;

    Vector3 movementForward;
    Vector3 movementRight;

    Image sBar;

    // Start is called before the first frame update
    void Awake()
    {

        player = this.gameObject;

        rb = player.GetComponent<Rigidbody>();

        playerCamera = GameObject.FindWithTag("PlayerCamera");
        sBar = GameObject.Find("Canvas").transform.Find("StaminaBar").gameObject.GetComponent<Image>();

        Cursor.lockState = CursorLockMode.Locked;

        localEulers = playerCamera.transform.localEulerAngles;

    }

    // Update is called once per frame
    void Update()
    {

        Cursor.lockState = CursorLockMode.Locked;

        transform.Rotate(InputManager.MouseMovement().x * mouseSensitivity * Vector3.up);
        localEulers += (-InputManager.MouseMovement().y * mouseSensitivity * Vector3.right);
        localEulers.x = Mathf.Clamp(localEulers.x, -75, 75);
        localEulers.z = Mathf.Clamp(localEulers.z, -75, 75);
        playerCamera.transform.localRotation = Quaternion.Euler(localEulers);

        movementRight = new Vector3(transform.right.x, 0, transform.right.z).normalized;
        movementForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;


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

        head.localRotation = playerCamera.transform.localRotation;
        head.localRotation = Quaternion.Euler(head.localEulerAngles.x + 60, head.localEulerAngles.y, head.localEulerAngles.z);

    }

    private void UpdateBars()
    {

        sBar.fillAmount = Mathf.Lerp(sBar.fillAmount, stamina / maxStamina, 0.08f);

    }

    private void FixedUpdate()
    {

        LayerMask mask = LayerMask.GetMask("Solid");

        if (Physics.CheckSphere(transform.position + transform.up * 0.45f, 0.5f, mask))
        {
            coyoteTime = 0.2f;
            staminaCooldown = 0;
        }

        velocity = rb.velocity;

        Vector3 movementVector;
        movementVector = new Vector3(0, 0, 0);

        movementVector += InputManager.MovementVector().y * movementSpeed * movementForward;
        movementVector += InputManager.MovementVector().x * movementSpeed * movementRight;

        if (velocity.y < 0)
        {
            velocity.y -= 9.8f * Time.deltaTime * gravityScale * fallMultiplier;
        }
        else if (InputManager.Fall())
        {
            velocity.y -= 9.8f * Time.deltaTime * gravityScale * lowJumpMultiplier;
        }
        else
        {
            velocity.y -= 9.8f * Time.deltaTime * gravityScale;
        }

        if (coyoteTime > 0 && jumpBuffer > 0)
        {

            velocity.y = jumpHeight;
            jumpBuffer = 0;
            coyoteTime = 0;

        }

        movementVector.y = velocity.y;

        velocity = Vector3.Lerp(velocity, movementVector, coyoteTime > 0 ? 0.1f : 0.07f);

        rb.velocity = velocity;

    }

}
