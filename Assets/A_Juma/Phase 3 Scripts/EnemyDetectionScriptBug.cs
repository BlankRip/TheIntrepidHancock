using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionScriptBug : MonoBehaviour
{
    //================================================
    public Transform playerTransform;
    public Transform leftTransform;
    public Transform rightTransform;
    public Transform headTransform;
    public Transform feetTransform;
    //
    Vector3 leftDir;
    Vector3 rightDir;
    //
    Vector3 playerDir;
    Vector3 headDir;
    Vector3 feetDir;
    //
    public float raycastDistance;
    public float raycastToPlayerDistanceLimiter;
    public float fieldOfViewAngle;
    float angle;
    //
    RaycastHit hit;
    RaycastHit hitHead;
    RaycastHit hitFeet;
    //
    bool playerFound = false;
    //================================================

    void Start()
    {
        raycastDistance = 30;
        raycastToPlayerDistanceLimiter = Mathf.Infinity;
    }

    void Update()
    {
        headDir = headTransform.position - transform.position;
        feetDir = feetTransform.position - transform.position;
        playerDir = playerTransform.position - transform.position;

        //========remove V V V if you dont want debug========
        leftDir = leftTransform.position - transform.position;
        rightDir = rightTransform.position - transform.position;
        Debug.DrawRay(transform.position, leftDir.normalized * raycastDistance, Color.gray); // angle left
        Debug.DrawRay(transform.position, rightDir.normalized * raycastDistance, Color.gray); // angle right
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.white); // angle middle 
        //========remove ^ ^ ^ if you dont want debug========

        angle = Vector3.Angle(playerDir.normalized, transform.forward);
        if (angle < fieldOfViewAngle * 0.5f)
        {
            /*if (Physics.Raycast(transform.position, playerDir.normalized, out hit, raycastToPlayerDistanceLimiter) ||
                Physics.Raycast(transform.position, headDir.normalized, out hitHead, raycastToPlayerDistanceLimiter) ||
                Physics.Raycast(transform.position, feetDir.normalized, out hitFeet, raycastToPlayerDistanceLimiter))//, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(transform.position, playerDir.normalized * hit.distance, Color.blue); // enemy to player raycast
                Debug.DrawRay(transform.position, headDir.normalized * hitHead.distance, Color.blue); // enemy to player raycast
                Debug.DrawRay(transform.position, feetDir.normalized * hitFeet.distance, Color.blue); // enemy to player raycast
                if (hit.collider.tag == "Player" || hitHead.collider.tag == "Player" || hitFeet.collider.tag == "Player")
                {
                    playerFound = true;
                    raycastToPlayerDistanceLimiter = Mathf.Infinity;
                    Debug.Log("DETECTED THE PLAYER // raycast hit belly");
                }
                else //if (hit.collider.tag != "Player" || hitHead.collider.tag != "Player" || hitFeet.collider.tag != "Player")
                {
                    if (playerFound == true)
                    {
                        raycastDistance = raycastToPlayerDistanceLimiter;
                        playerFound = false;
                        Debug.Log("just escaped bool = true");
                        Debug.Log("last seen pos = transofmr .pos");
                    }
                    Debug.Log("bellycast // player in range but behind something?");
                }
            }*/

            if (Physics.Raycast(transform.position, playerDir.normalized, out hitHead, raycastToPlayerDistanceLimiter))
            {
                //Debug.DrawRay(transform.position, playerDir.normalized * hit.distance, Color.blue); // enemy to player raycast
                Debug.DrawRay(transform.position, headDir.normalized * hitHead.distance, Color.green); // enemy to player raycast
                Debug.DrawRay(transform.position, feetDir.normalized * hitFeet.distance, Color.red); // enemy to player raycast
                if (hitHead.collider.tag == "Player")// <--- problem is here // the reason null exception is happening is becauyse of the 1 raycast and the first raycast it shot out will only work and 
                {                                    //      if that one is blocked it cant return the hit info for the other raycast because the other casts are not working only the first one is happening
                    playerFound = true;
                    raycastToPlayerDistanceLimiter = Mathf.Infinity;
                    Debug.Log("DETECTED THE PLAYER // raycast hit belly");
                }
                else //if (hit.collider.tag != "Player" || hitHead.collider.tag != "Player" || hitFeet.collider.tag != "Player")
                {
                    if (playerFound == true)
                    {
                        raycastDistance = raycastToPlayerDistanceLimiter;
                        playerFound = false;
                        Debug.Log("just escaped bool = true");
                        Debug.Log("last seen pos = transofmr .pos");
                    }
                    Debug.Log("bellycast // player in range but behind something?");
                }
            }
        }
    }
}