using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    private Vector3 localEulers;
    public float mouseSensitivity = 3;
    public Transform playerCamera;
    public Transform orientation;

    private void Awake()
    {

        localEulers = playerCamera.transform.localEulerAngles;

    }

    // Update is called once per frame
    void Update()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        localEulers.y += InputManager.MouseMovement().x * mouseSensitivity;
        localEulers.x -= InputManager.MouseMovement().y * mouseSensitivity;
        
        localEulers.x = Mathf.Clamp(localEulers.x, -75, 75);

        orientation.localRotation = Quaternion.Euler(0, localEulers.y, 0);
        playerCamera.transform.localRotation = Quaternion.Euler(localEulers.x, localEulers.y, 0);

    }
}
