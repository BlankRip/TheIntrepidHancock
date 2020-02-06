using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerMovement movementController;                           //The movement script
    PlayerStats myStats;                                         //The Stats script

    float horizontalInput;                                       //Horizontal motion or input values between 1 & 0 (Asises)
    float verticalInput;                                         //Vertical motion or input values between 1 & 0 (Asises)
    float InitialSetSpeed;                                       //The noraml speed the player for tracking purposes
    bool crouch;                                                 //If the player is crouching
    static bool sprint;                                          //If the player is sprinting
    [SerializeField] float speed;                                //The normal speed of the player
    [Range(0,5)] [SerializeField] float stoppingSpeed;           //The speed just before he stops moving
    [SerializeField] KeyCode crouchKey;                          //The crouch key
    [SerializeField] KeyCode sprintKey;                          //The sprint key

    private void Start()
    {
        movementController = GetComponent<PlayerMovement>();
        myStats = GetComponent<PlayerStats>();
        InitialSetSpeed = speed;
    }

    private void Update()
    {
        //The horizontal and vertical inputs
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //Setting the speed to the standard move speed else reduce it to make it stop soon
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            speed = InitialSetSpeed;
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            speed = InitialSetSpeed;
        else
            speed = stoppingSpeed;

        //Checking to crouch
        if (Input.GetKeyDown(crouchKey))
            crouch = true;
        else if (Input.GetKeyUp(crouchKey))
            crouch = false;

        //Check if sprinting and has stamina
        if (Input.GetKeyDown(sprintKey))
            sprint = true;
        else if (Input.GetKeyUp(sprintKey))
            sprint = false;

        //Manageing the consumtion of stamina when sprinting
        if (horizontalInput != 0 || verticalInput != 0)
            myStats.StaminaReducion(ref sprint);
        //Managing stamina recovary when not sprinting
        myStats.StaminaRecovary(sprint);
    }

    private void FixedUpdate()
    {
        //Calling movement from the movement script
        movementController.Movement(horizontalInput, verticalInput, speed, sprint, crouch);
    }





    //Maybe use as ref for animations
    //void RotationAnimation()
    //{
        //If less than thresh hold speed then it will just rotate in that direction
        //if (verticalMove > 0 && verticalMove < moveThreshHold)
        //{

        //}
        //else if (verticalMove < 0 && verticalMove > -moveThreshHold)
        //{
        //    turnAngle = Quaternion.Euler(0, rotateAround.eulerAngles.y + 180, 0);
        //    rb.rotation = Quaternion.Slerp(transform.rotation, turnAngle, rotationSpeed);
        //}
        //else if (horizontalMove > 0 && horizontalMove < moveThreshHold)
        //{
        //    turnAngle = Quaternion.Euler(0, rotateAround.eulerAngles.y + 90, 0);
        //    rb.rotation = Quaternion.Slerp(transform.rotation, turnAngle, rotationSpeed);
        //}
        //else if (horizontalMove < 0 && horizontalMove > -moveThreshHold)
        //{
        //    turnAngle = Quaternion.Euler(0, rotateAround.eulerAngles.y - 90, 0);
        //    rb.rotation = Quaternion.Slerp(transform.rotation, turnAngle, rotationSpeed);
        //}
    //}
}
