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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRb = target.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

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
            steering = steering.normalized * maxForce;
        }
    }

    void Persuit(Vector3 targetPos, Rigidbody rbTarget, float slowRadius)
    {
        Vector3 futurePos = targetPos + rbTarget.velocity * framesAhead;
        SeekPoint(futurePos, slowRadius);
    }
}
