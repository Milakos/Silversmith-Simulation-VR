using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrecensePhysics : MonoBehaviour
{
    public Transform target;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody>(); 
    }

    private void FixedUpdate() 
    {
        rb.velocity = (target.position - transform.position)/Time.fixedDeltaTime;
        Quaternion rotationDifference = target.rotation * Quaternion.Inverse(transform.rotation);
        rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);
        Vector3 rotationDifedreneInDegree = angleInDegree * rotationAxis;
        rb.angularVelocity = (rotationDifedreneInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);  
    }
}
