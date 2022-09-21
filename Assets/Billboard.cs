using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    public Transform target;

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(target);
        transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        transform.Rotate(0, 180, 0);

    }
}
