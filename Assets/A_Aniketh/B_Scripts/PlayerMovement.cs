﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    Vector3 referanceVelocity = Vector3.zero;
    Vector3 targetVelocity;
    Quaternion turnAngle;
    [SerializeField] Collider crouchDisableCollider;                     //The collider that will be disabled when the player is crouching
    [Range(0,1)] [SerializeField] float crouchSpeedReduction;            //The amount by which the speed should be reduced while crouching
    [Range(1, 2)] [SerializeField] float sprintSpeedMultiplyer;          //The amount by whcic the speed should be increased
    [SerializeField] bool useRotate;                                     //If this is ticked true then the player will rotate around a target boject
    [SerializeField] Transform rotateAround;                             //The object used as ref to rotate
    [SerializeField] float rotationSpeed;                                //How fast will it rotate
    [Range(0, 0.5f)] [SerializeField] float smoothMovementBy;            //The time it takes to reach the target velocity
    [Range(0, 0.5f)] [SerializeField] float moveThreshHold;              //The thresh hold values, the player will move when inputs is greater than this values

    private void Start()
    {
        rb = GetComponent<Rigidbody>();    //Getting rigid body of the gameObject
    }

    public void Movement(float horizontalMove, float verticalMove, float speed, bool sprint, bool crouch)
    {
        //Checking if Crouching, if so then disabeling the extra part of the collider
        if(crouch)
        {
            speed = speed * crouchSpeedReduction;
            if (crouchDisableCollider != null)
                crouchDisableCollider.enabled = false;
        }
        else
        {
            if (crouchDisableCollider != null)
                crouchDisableCollider.enabled = true;
        }

        //Check if sprinting then increase movement speed
        if (sprint && !crouch)
            speed = speed * sprintSpeedMultiplyer;

        //The velocity at which the object needs to be
        targetVelocity = new Vector3(horizontalMove * speed, rb.velocity.y, verticalMove * speed);
        targetVelocity = transform.rotation * targetVelocity;                //To apply verlocity in the direction the player is rotated

        //If the input values are over a perticualr thresh hold then the object will move with a desired velocity
        if (horizontalMove > moveThreshHold || horizontalMove < -moveThreshHold || verticalMove > moveThreshHold || verticalMove < -moveThreshHold)
        {
            if (useRotate)
            {
                turnAngle = Quaternion.Euler(0, rotateAround.eulerAngles.y, 0);
                rb.rotation = Quaternion.Slerp(transform.rotation, turnAngle, rotationSpeed);
            }
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref referanceVelocity, Time.deltaTime * smoothMovementBy);
        }
    }
}
