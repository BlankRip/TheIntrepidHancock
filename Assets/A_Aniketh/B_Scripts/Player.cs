using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerMovement movementController;

    float horizontalInput;
    float verticalInput;
    float InitialSetSpeed;
    bool crouch;
    [SerializeField] float speed;
    [Range(0,5)] [SerializeField] float stoppingSpeed;
    [SerializeField] KeyCode crouchKey;

    private void Start()
    {
        movementController = GetComponent<PlayerMovement>();
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
    }

    private void FixedUpdate()
    {
        //Calling movement from the movement script
        movementController.Movement(horizontalInput, verticalInput, speed, crouch);
    }
}
