using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    private Vector3 localEulers;
    public float mouseSensitivity = 3;
    public Transform head;
    Transform playerCamera;

    private void Awake()
    {

        playerCamera = transform.Find("PlayerCamera");

        localEulers = playerCamera.transform.localEulerAngles;

    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(InputManager.MouseMovement().x * mouseSensitivity * Vector3.up);
        localEulers += (-InputManager.MouseMovement().y * mouseSensitivity * Vector3.right);
        localEulers.x = Mathf.Clamp(localEulers.x, -75, 75);
        localEulers.z = Mathf.Clamp(localEulers.z, -75, 75);
        playerCamera.transform.localRotation = Quaternion.Euler(localEulers);

        head.localRotation = playerCamera.transform.localRotation;
        head.localRotation = Quaternion.Euler(head.localEulerAngles.x + 60, head.localEulerAngles.y, head.localEulerAngles.z);

    }
}
