using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerMovement movementController;                           //The movement script
    PlayerHealth myStats;                                        //The Stats script

    float horizontalInput;                                       //Horizontal motion or input values between 1 & 0 (Asises)
    float verticalInput;                                         //Vertical motion or input values between 1 & 0 (Asises)
    float InitialSetSpeed;                                       //The noraml speed the player for tracking purposes
    bool crouch;                                                 //If the player is crouching
    static bool sprint;                                          //If the player is sprinting

    [Tooltip("The normal speed of the player")] 
    [SerializeField] float speed;
    [Tooltip("The speed just before he stops moving")] 
    [Range(0,5)] [SerializeField] float stoppingSpeed;
    [Tooltip("The sprint key")] 
    [SerializeField] KeyCode sprintKey;
    [Tooltip("Choose to use animation this is here just to make the designer test the game without animations")] 
    [SerializeField] bool useAnimtion;
    [Tooltip("The animator for the player")] 
    [SerializeField] Animator animController;

    //Things for weapon attach and playing animation
    [Tooltip("The key to launch attack")] 
    [SerializeField] KeyCode attackKey = KeyCode.Mouse0;
    [HideInInspector] public J_Weapon equippedWeapon;
    bool attacking;

    [Header("Things for attack animation")]
    [SerializeField] float gapBetweenAttacks;
    float attackGapTracker = 0;
    int pickAttackAnim;
    int previouAttackAnim;

    [Header("For Raycast to stop near the wall")]
    [SerializeField] Transform headPos;
    [SerializeField] LayerMask wallLayers;
    [SerializeField] float castDistance;
    RaycastHit hitInfo;
    Vector3 camForward;
    Quaternion rayRotationAngle;
    bool virticleIdleAtWall;
    bool horizontalIdleAtWall;

    [Header("Things for VFX in Player")]
    [Tooltip("Foot steps dust spwner for left leg")] 
    [SerializeField] ParticleSystem footStepParticlesLeft;
    [Tooltip("Foot steps dust spwner for right leg")] 
    [SerializeField] ParticleSystem footStepParticlesRight;

    [Header("Audio FX for player")]
    [Tooltip("Foot steps sound left leg")]
    [SerializeField] AudioSource leftFootAudio;
    [Tooltip("Foot steps sound right leg")]
    [SerializeField] AudioSource rightFootAudio;

    private void Start()
    {
        movementController = GetComponent<PlayerMovement>();
        myStats = GetComponent<PlayerHealth>();
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

            //Check if sprinting and has stamina
            if (Input.GetKeyDown(sprintKey))
                sprint = true;
            else if (Input.GetKeyUp(sprintKey))
                sprint = false;

            if ((verticalInput < 0) || ( horizontalInput > 0.1f && verticalInput < 0.1f && verticalInput > -0.1) ||(horizontalInput < -0.1f && verticalInput > -0.1f && verticalInput < 0.1f))
                sprint = false;
            //if (verticalInput > 0.1f || verticalInput < -0.1f) 
            //    horizontalInput =  Mathf.Clamp(horizontalInput, -0.2f, 0.2f);
            if (!sprint)
                verticalInput = Mathf.Clamp(verticalInput, -0.6f, 0.6f);

            //------------------------------------------------------- ANIMATIONS ---------------------------------------------------

            if (useAnimtion)
            {
                if (!virticleIdleAtWall)
                    animController.SetFloat("Speed", verticalInput);
                else
                    animController.SetFloat("Speed", 0);

                if (!horizontalIdleAtWall)
                {
                    if (verticalInput == 0 && horizontalInput != 0)
                        animController.SetFloat("Horizontal", horizontalInput);
                    else
                        animController.SetFloat("Horizontal", 0);
                }
                else
                    animController.SetFloat("Horizontal", 0);

                //Checking if this weapon is equipped
                if (equippedWeapon != null)
                {
                    if (Time.time > attackGapTracker)
                    {
                        if (Input.GetKeyDown(attackKey))
                        {
                            attacking = true;
                            sprint = false;
                            crouch = false;

                            //if (Time.time < attackGapTracker + 1)
                            //    pickAttackAnim++;
                            //else
                            //    pickAttackAnim = 1;

                            //if (pickAttackAnim >= 4)
                            //    pickAttackAnim = 1;

                            pickAttackAnim = Random.Range(1, 4);
                            if (pickAttackAnim == previouAttackAnim)
                            {
                                if (pickAttackAnim == 3)
                                    pickAttackAnim = 1;
                                else
                                    pickAttackAnim++;
                            }

                            Debug.Log("<color=blue>" + pickAttackAnim + "</color>");

                            if (pickAttackAnim == 1)
                                animController.SetTrigger("Attack1");
                            else if (pickAttackAnim == 2)
                                animController.SetTrigger("Attack2");
                            else if (pickAttackAnim == 3)
                                animController.SetTrigger("Attack3");

                            previouAttackAnim = pickAttackAnim;
                            equippedWeapon.ActivateEffects();
                            attackGapTracker = Time.time + gapBetweenAttacks;
                        }
                    }
                }
            }

            //------------------------------------------------------- ANIMATIONS ---------------------------------------------------
        }
    }

    private void FixedUpdate()
    {
        HittingWall();
        //Calling movement from the movement script
        if (!attacking)
            movementController.Movement(horizontalInput, verticalInput, speed, sprint, crouch);
    }

    // When close to wall it will stop motion in that direction and idle in spot until input in a diffrent direction
    void HittingWall()
    {
        if (Camera.main != null)
            camForward = Camera.main.transform.forward;
        camForward.y = 0;

        rayRotationAngle = Quaternion.LookRotation(camForward.normalized, Vector3.up);

        if (Physics.Raycast(headPos.position, rayRotationAngle * Vector3.forward, out hitInfo, castDistance, wallLayers))
        {
            if (verticalInput > 0)
            {
                verticalInput = 0;
                virticleIdleAtWall = true;
            }
        }
        else if (Physics.Raycast(headPos.position, rayRotationAngle * -Vector3.forward, out hitInfo, castDistance, wallLayers))
        {
            if (verticalInput < 0)
            {
                verticalInput = 0;
                virticleIdleAtWall = true;
            }
        }
        else
            virticleIdleAtWall = false;

        if (Physics.Raycast(headPos.position, rayRotationAngle * Vector3.right, out hitInfo, castDistance, wallLayers))
        {
            if (horizontalInput > 0)
            {
                horizontalInput = 0;
                horizontalIdleAtWall = true;
            }
        }
        if (Physics.Raycast(headPos.position, rayRotationAngle * -Vector3.right, out hitInfo, castDistance, wallLayers))
        {
            if (horizontalInput < 0)
            {
                horizontalInput = 0;
                horizontalIdleAtWall = true;
            }
        }
        else
            horizontalIdleAtWall = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            AudioManger.instance.PlayGruntClip();
            animController.SetTrigger("Damaged");
            myStats.ReduceHealth();
        }
    }

    //Function that deactivates the attack collider for the equipped weapon when the attack animation ends
    public void AttackAnimationEndEvent()
    {
        attacking = false;
        if (equippedWeapon != null)
            equippedWeapon.DeactivateEffects();
    }

    //Funtions that play the footstep dust as an animation event
    public void RightFootAnimationEvent()
    {
        leftFootAudio.Play();
        footStepParticlesRight.Play();
    }

    public void LeftFootAnimationEvent()
    {
        rightFootAudio.Play();
        footStepParticlesLeft.Play();
    }
}
