using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollissionAvoidance : MonoBehaviour
{
    public float moveSpeed, turnSpeed;
    public Transform castPoint;
    public float repelPow = 1, rayLength = 2, castOffset = 0.5f;
    RaycastHit hitLeft, hitFront, hitRight;
    Vector3[] castVectors = new Vector3[3];
    Vector3 avoidanceVector;
    public Rigidbody rb;

    private void Start()
    {
        castVectors[0] = (Vector3.forward - Vector3.right).normalized;
        castVectors[1] = Vector3.forward;
        castVectors[2] = (Vector3.forward + Vector3.right).normalized;
    }

    public void FixedUpdate()
    {
        avoidanceVector = Vector3.zero;
        // casts three rays
        if (!Physics.Raycast(castPoint.position + transform.rotation * castVectors[0] * castOffset, transform.rotation * castVectors[0], out hitLeft, rayLength)) hitLeft.distance = rayLength;
        if(!Physics.Raycast(castPoint.position + transform.rotation * castVectors[1] * castOffset, transform.rotation * castVectors[1], out hitFront, rayLength))hitFront.distance = rayLength;
        if(!Physics.Raycast(castPoint.position + transform.rotation * castVectors[2] * castOffset, transform.rotation * castVectors[2], out hitRight, rayLength)) hitRight.distance = rayLength;

        float factor_Left = (1 - (hitLeft.distance / rayLength));
        float factor_Front = (1 - (hitFront.distance / rayLength));
        float factor_Right = (1 - (hitRight.distance / rayLength));

        // repel vector addition
        avoidanceVector += repelPow * transform.right * factor_Left;
        avoidanceVector += repelPow * -transform.forward * factor_Front;
        avoidanceVector += repelPow * -transform.right * factor_Right;

    //    Debug.Log(hitLeft.distance + " - " + hitFront.distance + " - " + hitRight.distance);
        Debug.DrawRay(castPoint.position + transform.rotation * castVectors[0] * castOffset, transform.rotation * castVectors[0] * rayLength, Color.Lerp(Color.white, Color.red, factor_Left));
        Debug.DrawRay(castPoint.position + transform.rotation * castVectors[1] * castOffset, transform.rotation * castVectors[1] * rayLength, Color.Lerp(Color.white, Color.red, factor_Front));
        Debug.DrawRay(castPoint.position + transform.rotation * castVectors[2] * castOffset, transform.rotation * castVectors[2] * rayLength, Color.Lerp(Color.white, Color.red, factor_Right));

        rb.velocity = transform.forward * moveSpeed + avoidanceVector;
        rb.angularVelocity = new Vector3(0, (factor_Left - factor_Right) * turnSpeed * Time.fixedDeltaTime, 0);
    }

}
