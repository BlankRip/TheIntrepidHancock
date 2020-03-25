using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour
{
    [SerializeField] float maxVelocity;
    [SerializeField] float maxForce;
    [SerializeField] float slowRadius = 1;
    [SerializeField] int framesAhead = 1;
    [SerializeField] GameObject target;
    Rigidbody targetRb;
    Vector3 steering;

    Rigidbody rb;


    public Transform castPoint;
    public float repelPow = 1, rayLength = 2, castOffset = 0.5f;
    RaycastHit hitLeft, hitFront, hitRight;
    Vector3[] castVectors = new Vector3[3];
    Vector3 avoidanceVector;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRb = target.GetComponent<Rigidbody>();

        castVectors[0] = (Vector3.forward - Vector3.right).normalized;
        castVectors[1] = Vector3.forward;
        castVectors[2] = (Vector3.forward + Vector3.right).normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CollisionAvoidance();
        Persuit(target.transform.position, targetRb, slowRadius);

        rb.velocity += steering;
    }

    void SeekPoint(Vector3 targetPos, float slowradious)
    {
        Vector3 desigeredVelocity = (targetPos - transform.position).normalized * maxVelocity;

        float distance = Vector3.Distance(transform.position, targetPos);
        if (distance < slowradious)
            desigeredVelocity = desigeredVelocity * (distance / slowradious);

        steering = desigeredVelocity - rb.velocity;
        if(steering.magnitude > maxForce)
        {
            steering = steering.normalized * maxForce + avoidanceVector;
        }
    }

    void Persuit(Vector3 targetPos, Rigidbody rbTarget, float slowRadius)
    {
        Vector3 futurePos = targetPos + rbTarget.velocity * framesAhead;
        SeekPoint(futurePos, slowRadius);
    }

    Vector3 CollisionAvoidance()
    {
        avoidanceVector = Vector3.zero;
        // casts three rays
        if (!Physics.Raycast(castPoint.position + transform.rotation * castVectors[0] * castOffset, transform.rotation * castVectors[0], out hitLeft, rayLength))
            hitLeft.distance = rayLength;
        if (!Physics.Raycast(castPoint.position + transform.rotation * castVectors[1] * castOffset, transform.rotation * castVectors[1], out hitFront, rayLength))
            hitFront.distance = rayLength;
        if (!Physics.Raycast(castPoint.position + transform.rotation * castVectors[2] * castOffset, transform.rotation * castVectors[2], out hitRight, rayLength))
            hitRight.distance = rayLength;

        float factor_Left = (1 - (hitLeft.distance / rayLength));
        float factor_Front = (1 - (hitFront.distance / rayLength));
        float factor_Right = (1 - (hitRight.distance / rayLength));

        // repel vector addition
        avoidanceVector += repelPow * transform.right * factor_Left;
        avoidanceVector += repelPow * -transform.forward * factor_Front;
        avoidanceVector += repelPow * -transform.right * factor_Right;
        return avoidanceVector;
    }
}
