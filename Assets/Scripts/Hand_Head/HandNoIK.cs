using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class HandNoIK : MonoBehaviour
{
    
    Animator anim;
    SkinnedMeshRenderer mesh;
    private float gripTarget;
    private float triggerTarget;
    private float gripCurrent;
    private float triggerCurrent;
    [SerializeField]
    private float animationSpeed;

    //physics MOvement
    [SerializeField]
    private GameObject followObject;
    [SerializeField]
    private float followSpeed = 30f;
    [SerializeField]
    private float rotateSpeed = 50f;
    [SerializeField]
    private Vector3 rotationOffset;
    [SerializeField]
    private Vector3 positionOffset;
    private Transform followTarget;
    private Rigidbody body;

    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();

        //Physics MOvement
        followTarget = followObject.transform;
        body = GetComponent<Rigidbody>();
        // body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        // body.interpolation = RigidbodyInterpolation.Interpolate;
        body.mass = 20f;

        //Teleport hands
        body.position = followTarget.position;
        body.rotation = followTarget.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AnimateHand();
        PhysicsMove();   
    }

    private void PhysicsMove()
    {
        //Position
        var positionWithOffset = followTarget.position + positionOffset;
        // var positionWithOffset = followTarget.TransformPoint(positionOffset);
        var distance = Vector3.Distance(positionWithOffset, transform.position);
        body.velocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);

        //Rotation
        var rotationWIthOffset = followTarget.rotation * Quaternion.Euler(rotationOffset);
        var q = rotationWIthOffset * Quaternion.Inverse(body.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        
        if (Mathf.Abs(axis.magnitude)!= Mathf.Infinity)
        {
            if (angle > 180.0f)
            {
                angle -=360.0f;
            }
            body.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed);
        }        
    }

    internal void SetGrip(float v)
    {
        gripTarget = v;
    }

    internal void SetTrigger(float v)
    {
        triggerTarget = v;
    }

    void AnimateHand()
    {
        if(gripCurrent != gripTarget)
        {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * animationSpeed);
            anim.SetFloat("Grip", gripCurrent);
        }
        if(triggerCurrent != triggerTarget)
        {
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * animationSpeed);
            anim.SetFloat("Trigger", triggerCurrent);
        }
    }

    public void ToggleVisibility()
    {
        mesh.enabled = !mesh.enabled;
    }
}

