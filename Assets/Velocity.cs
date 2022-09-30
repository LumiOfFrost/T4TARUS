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
    public float friction = 1;

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

        Frictify(friction);

        cc.Move(velocity * Time.deltaTime);

    }

    void Frictify(float t)
    {

        if (cc.isGrounded)
        {

            float drop;
            float speed;
            float newSpeed;
            Vector3 veloc = velocity;

            veloc.y = 0;
            speed = veloc.magnitude;
            drop = 0.6f * Time.deltaTime * t;

            newSpeed = speed - drop;

            veloc = veloc.normalized * newSpeed;

            velocity = new Vector3(veloc.x, velocity.y, veloc.z);

        }
        else
        {

            velocity.x = Mathf.Lerp(velocity.x, 0, 0.01f);
            velocity.z = Mathf.Lerp(velocity.z, 0, 0.01f);

        }

    }

}
