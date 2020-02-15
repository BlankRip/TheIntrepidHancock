using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DampTest : MonoBehaviour
{
    //A simple smooth follow camera,
    // that follows the targets forward direction

    public Transform target;
    public float smooth = 0.3f;
    public float distance = 5.0f;
    public float yVelocity = 0.0f;

    void Update()
    {
        // Damp angle from current y-angle towards target y-angle
        float yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref yVelocity, smooth);
        // Position at the target
        Vector3 position = target.position;
        // Then offset by distance behind the new angle
        position += Quaternion.Euler(0, yAngle, 0) * new Vector3(0, 0, -distance);
        // Apply the position
        transform.position = position;

        // Look at the target
        transform.LookAt(target);
    }
}