using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheAI : MonoBehaviour
{
    public bool playerFound;
    public Animator myAnimator;
    [Range(0, 1)] public float coolDownSpeed = 1;
    [Range(1, 2)] public float idleTimeSpeed = 1;

    TreeNode root;

    [Header("For Player Detection")]
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform headTransform;
    [SerializeField] Transform feetTransform;

    Vector3 playerDir;
    Vector3 headDir;
    Vector3 feetDir;
    RaycastHit hit;
    RaycastHit hitHead;
    RaycastHit hitFeet;

    [SerializeField] int raycastToPlayerDistanceLimiter;
    [SerializeField] float fieldOfViewAngle;
    float angle;
    bool playerDetected = false;

    [Header("For Steering")]
    [SerializeField] GameObject target;
    [SerializeField] float maxVelocity;
    [SerializeField] float maxForce;
    [SerializeField] int framesAhead;

    Vector3 steering;
    Rigidbody targerRb;
    Rigidbody rb;

    [Header("For Collision Avoidance")]

    [SerializeField] Transform castPoint;
    [SerializeField] LayerMask repelLayers;
    [SerializeField] float repelPow = 1, rayLength = 2, castOffset = 0.5f;

    RaycastHit hitLeft, hitFront, hitRight;
    Vector3[] castVectors = new Vector3[3];
    Vector3 avoidanceVector;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targerRb = target.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        root.Run(this);
    }


    public void Seek(Vector3 seekPoition, float slowRadios)
    {
        Vector3 desigeredVelocity = (seekPoition - transform.position).normalized * maxVelocity;

        float distance = Vector3.Distance(transform.position, seekPoition);
        if (distance < slowRadios)
            desigeredVelocity = desigeredVelocity * (distance / slowRadios);

        steering = (desigeredVelocity - rb.velocity) + avoidanceVector;
        if (steering.magnitude > maxForce)
        {
            steering = steering.normalized * maxForce;
        }
    }

    public void Pursuit(Vector3 seekPoition, Rigidbody targetRigidBody, float slowRadios)
    {
        Vector3 futurePos = seekPoition + targetRigidBody.velocity * framesAhead;
        Seek(futurePos, slowRadios);
    }

    public Vector3 CollisionAvoidance()
    {
        avoidanceVector = Vector3.zero;
        // casts three rays
        if (!Physics.Raycast(castPoint.position + transform.rotation * castVectors[0] * castOffset, transform.rotation * castVectors[0], out hitLeft, rayLength, repelLayers))
            hitLeft.distance = rayLength;
        if (!Physics.Raycast(castPoint.position + transform.rotation * castVectors[1] * castOffset, transform.rotation * castVectors[1], out hitFront, rayLength, repelLayers))
            hitFront.distance = rayLength;
        if (!Physics.Raycast(castPoint.position + transform.rotation * castVectors[2] * castOffset, transform.rotation * castVectors[2], out hitRight, rayLength, repelLayers))
            hitRight.distance = rayLength;

        float factor_Left = (1 - (hitLeft.distance / rayLength));
        float factor_Front = (1 - (hitFront.distance / rayLength));
        float factor_Right = (1 - (hitRight.distance / rayLength));

        // repel vector addition
        avoidanceVector += repelPow * transform.right * factor_Left;
        avoidanceVector += repelPow * -transform.forward * factor_Front;
        avoidanceVector += repelPow * -transform.right * factor_Right;

        rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(rb.velocity.normalized, Vector3.up), Time.fixedDeltaTime * 10);
        return avoidanceVector;
    }

    public void playerDetection()
    {
        headDir = headTransform.position - transform.position;
        feetDir = feetTransform.position - transform.position;
        playerDir = playerTransform.position - transform.position;

        angle = Vector3.Angle(playerDir.normalized, transform.forward);
        if (angle < fieldOfViewAngle * 0.5f)
        {
            if (Physics.Raycast(transform.position, playerDir.normalized, out hit, raycastToPlayerDistanceLimiter))//, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(transform.position, playerDir.normalized * hit.distance, Color.blue); // enemy to player raycast
                if (hit.collider.tag == "Player")
                {
                    playerDetected = true;
                    Debug.Log("DETECTED THE PLAYER // raycast hit belly");
                }
                else if (hit.collider.tag != "Player")
                {
                    Debug.Log("bellycast // player in range but behind something?");
                }
            }

            if (Physics.Raycast(transform.position, headDir.normalized, out hitHead, raycastToPlayerDistanceLimiter)) //, layerMask))
            {
                Debug.DrawRay(transform.position, headDir.normalized * hitHead.distance, Color.blue); // enemy to player raycast
                if (hitHead.collider.tag == "Player")
                {
                    playerDetected = true;
                    Debug.Log("DETECTED THE PLAYER // raycast hit head");
                }
                else if (hitHead.collider.tag != "Player")
                {
                    Debug.Log("headcast // player in range but behind something?");
                }
            }

            if (Physics.Raycast(transform.position, feetDir.normalized, out hitFeet, raycastToPlayerDistanceLimiter)) //, layerMask))
            {
                Debug.DrawRay(transform.position, feetDir.normalized * hitFeet.distance, Color.blue); // enemy to player raycast
                if (hitFeet.collider.tag == "Player")
                {
                    playerDetected = true;
                    Debug.Log("DETECTED THE PLAYER // raycast hit feet");
                }
                else if (hitFeet.collider.tag != "Player")
                {
                    Debug.Log("feetcast // player in range but behind something?");
                }
            }
        }
        else
        {
            Debug.Log("No player in sight undetected...");
        }
    }
}
