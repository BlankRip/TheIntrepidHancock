﻿using System.Collections;
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
    [SerializeField] bool useAnimtion;                           //Choose to use animation this is here just to make the designer test the game without animations
    [SerializeField] Animator animController;                    //The animator for the player

    //Things for weapon attach and playing animation
    [SerializeField] KeyCode attackKey = KeyCode.Mouse0;
    [HideInInspector] public J_Weapon equippedWeapon;
    bool attacking;

    [Header("Things for VFX in Player")]
    [SerializeField] ParticleSystem footStepParticlesLeft;       //Foot steps dust spwner for left leg
    [SerializeField] ParticleSystem footStepParticlesRight;      //Foot steps dust spwner for right leg

    private void Start()
    {
        movementController = GetComponent<PlayerMovement>();
        myStats = GetComponent<PlayerStats>();
        InitialSetSpeed = speed;
    }

    private void Update()
    {
        if (!attacking)
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

            //------------------------------------------------------- ANIMATIONS ---------------------------------------------------

            if (useAnimtion)
            {
                animController.SetFloat("Speed", verticalInput);
                if (horizontalInput != 0)
                    animController.SetFloat("Direction", horizontalInput);
                else
                    animController.SetFloat("Direction", Input.GetAxis("Anim Mouse X"));
                Debug.Log("<color=blue>" + Input.GetAxis("Anim Mouse X") + " </color>");

                if (verticalInput == 0 && horizontalInput != 0)
                {
                    animController.SetFloat("Horizontal", horizontalInput);
                }
                else
                    animController.SetFloat("Horizontal", 0);

                //Checking if this weapon is equipped
                if (equippedWeapon != null)
                {
                    if (Input.GetKeyDown(attackKey))
                    {
                        attacking = true;
                        sprint = false;
                        crouch = false;
                        animController.SetTrigger("Attack");
                        equippedWeapon.activateEffects = true;
                    }
                }
            }

            //------------------------------------------------------- ANIMATIONS ---------------------------------------------------

            //Manageing the consumtion of stamina when sprinting
            if (horizontalInput != 0 || verticalInput != 0)
                myStats.StaminaReducion(ref sprint);
            //Managing stamina recovary when not sprinting
            myStats.StaminaRecovary(sprint);
        }
    }

    private void FixedUpdate()
    {
        //Calling movement from the movement script
        if (!attacking)
            movementController.Movement(horizontalInput, verticalInput, speed, sprint, crouch);
    }



    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            myStats.ReduceHealth(20);
        }
    }

    //Function that deactivates the attack collider for the equipped weapon when the attack animation ends
    public void AttackAnimationEndEvent()
    {
        attacking = false;
        if (equippedWeapon != null)
            equippedWeapon.activateEffects = false;
    }

    //Funtions that play the footstep dust as an animation event
    public void RightFootAnimationEvent()
    {
        footStepParticlesRight.Play();
    }

    public void LeftFootAnimationEvent()
    {
        footStepParticlesLeft.Play();
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
