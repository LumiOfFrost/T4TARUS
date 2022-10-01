using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class Velocity : MonoBehaviour
{

    public bool useGravity = true;
    public float gravityValue = -19.62f;
    public Vector3 velocity;
    CharacterController cc;
    public float groundDamp = 3;
    public float airDamp = 3;

    float floot = 0;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        
        if (useGravity)
        {
            velocity.y += gravityValue * Time.deltaTime;
        }

        cc.Move(velocity * Time.deltaTime);

        Frictify();

    }

    void Frictify()
    {

        if (cc.isGrounded)
        {

            velocity.x = Mathf.SmoothDamp(velocity.x, 0, ref floot, groundDamp);
            velocity.z = Mathf.SmoothDamp(velocity.z, 0, ref floot, groundDamp);

        }
        else
        {

            velocity.x = Mathf.SmoothDamp(velocity.x, 0, ref floot, airDamp);
            velocity.z = Mathf.SmoothDamp(velocity.z, 0, ref floot, airDamp);

        }

        

    }

}
