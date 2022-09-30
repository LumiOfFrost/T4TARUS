using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantExplosionForce : MonoBehaviour
{

    public float radius = 2;
    public float force = 10;

    public LayerMask layerMask;

    private void FixedUpdate()
    {

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask, QueryTriggerInteraction.UseGlobal);

        foreach (Collider hit in hits)
        {

            if(hit.attachedRigidbody)
            {
                hit.attachedRigidbody.AddExplosionForce(force * (hit.GetComponent<Player>() ? 10 : 1), transform.position, radius, 1, ForceMode.Impulse);
            }
            if(hit.GetComponent<Velocity>())
            {

                hit.GetComponent<Velocity>().velocity -= Time.deltaTime * (hit.transform.position - transform.position - transform.up).normalized * (force * (1 - (hit.transform.position - transform.position - transform.up).magnitude));

            }

        }

    }

}
