using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustMove : MonoBehaviour
{
    Rigidbody rb;
    float horizontalInput;
    float verticalInput;
    [SerializeField] float speed = 5;
    PlayerMovement move;
    bool sprint;
    bool crouch;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        move = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

    }

    private void FixedUpdate()
    {
        move.Movement(horizontalInput, verticalInput, speed, sprint, crouch);
    }
}
