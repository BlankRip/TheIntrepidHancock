using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{

    public int speed;
    public int godSpeed;
    public int jump;
    Vector3 _EulerAngleVelocity;
    Rigidbody rb;

    void Start()
    {
        _EulerAngleVelocity = new Vector3(0, 200, 0);
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //transform.position += transform.forward * speed * Time.deltaTime;
            //rb.velocity = transform.forward * speed * Time.deltaTime;
            rb.MovePosition(transform.position += (transform.forward * speed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.A))
        {
            //transform.position += -transform.right * speed * Time.deltaTime;
            //rb.velocity = -transform.right * speed * Time.deltaTime;
            rb.MovePosition(transform.position += (-transform.right * speed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.S))
        {
            //transform.position += -transform.forward * speed * Time.deltaTime;
            //rb.velocity = -transform.forward * speed * Time.deltaTime;
            rb.MovePosition(transform.position += (-transform.forward * speed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.D))
        {
            //transform.position += transform.right * speed * Time.deltaTime;
            //rb.velocity = transform.right * speed * Time.deltaTime;
            rb.MovePosition(transform.position += (transform.right * speed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //transform.position += transform.right * speed * Time.deltaTime;
            //rb.velocity = transform.right * speed * Time.deltaTime;
            rb.MovePosition(transform.position += (transform.right * godSpeed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Quaternion deltaRotation = Quaternion.Euler(-_EulerAngleVelocity * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
        if (Input.GetKey(KeyCode.E))
        {
            Quaternion deltaRotation = Quaternion.Euler(_EulerAngleVelocity * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
        }
    }
}
