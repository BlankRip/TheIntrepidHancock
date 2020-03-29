using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionScript : MonoBehaviour
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
    Vector3 playerDirNormalized;
    Vector3 playerDirHeadNormalized;
    Vector3 playerDirFeetNormalized;
    //
    public int raycastDistance;
    public int raycastToPlayerDistanceLimiter;
    public float fieldOfViewAngle;
    float angle;
    //
    RaycastHit hit;
    RaycastHit hitHead;
    RaycastHit hitFeet;
    //NEW VARIABLES FOR RADIUS CHECK (REQ BY ALWIN)////////////////////////////////
    RaycastHit hitRadius;
    [SerializeField] int radiusRange;
    //NEW VARIABLES FOR RADIUS CHECK //////////////////////////////////////////////
    bool playerDetected = false;
    //================================================

    void Update()
    {
        leftDir = leftTransform.position - transform.position;
        rightDir = rightTransform.position - transform.position;
        headDir = headTransform.position - transform.position;
        playerDirHeadNormalized = headDir.normalized;
        feetDir = feetTransform.position - transform.position;
        playerDirFeetNormalized = feetDir.normalized;
        playerDir = playerTransform.position - transform.position;
        playerDirNormalized = playerDir.normalized;
        //Debug.DrawRay(transform.position, playerDirNormalized * 100, Color.blue); // enemy to player raycast
        Debug.DrawRay(transform.position, leftDir.normalized * raycastDistance, Color.gray); // angle left
        Debug.DrawRay(transform.position, rightDir.normalized * raycastDistance, Color.gray); // angle right
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.white); // angle middle 
        angle = Vector3.Angle(playerDirNormalized, transform.forward);
        if (angle < fieldOfViewAngle * 0.5f)
        {
            if (Physics.Raycast(transform.position, playerDirNormalized, out hit, raycastToPlayerDistanceLimiter))//, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(transform.position, playerDirNormalized * hit.distance, Color.blue); // enemy to player raycast
                if (hit.collider.tag == "Player")
                {
                    //playerDetected = true;
                    Debug.Log("DETECTED THE PLAYER // raycast hit belly");
                }
                else if (hit.collider.tag != "Player")
                {
                    Debug.Log("bellycast // player in range but behind something?");
                }
            }

            if (Physics.Raycast(transform.position, playerDirHeadNormalized, out hitHead, raycastToPlayerDistanceLimiter)) //, layerMask))
            {
                Debug.DrawRay(transform.position, playerDirHeadNormalized * hitHead.distance, Color.blue); // enemy to player raycast
                if (hitHead.collider.tag == "Player")
                {
                    //playerDetected = true;
                    Debug.Log("DETECTED THE PLAYER // raycast hit head");
                }
                else if (hitHead.collider.tag != "Player")
                {
                    Debug.Log("headcast // player in range but behind something?");
                }
            }

            if (Physics.Raycast(transform.position, playerDirFeetNormalized, out hitFeet, raycastToPlayerDistanceLimiter)) //, layerMask))
            {
                Debug.DrawRay(transform.position, playerDirFeetNormalized * hitFeet.distance, Color.blue); // enemy to player raycast
                if (hitFeet.collider.tag == "Player")
                {
                    //playerDetected = true;
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
        ///alwin requested a radius around the enemy copy this and put it in wherever youd like to use it
        if (Vector3.Distance(transform.position, playerTransform.position) <= radiusRange) //default value 10
        {
            if (Physics.Raycast(transform.position, playerDirNormalized, out hitRadius, raycastToPlayerDistanceLimiter))//, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(transform.position, playerDirNormalized * hitRadius.distance, Color.blue); // enemy to player raycast
                if (hitRadius.collider.tag == "Player")
                {
                    Debug.Log("DETECTED THE PLAYER // raycast hit belly");
                }
                else if (hitRadius.collider.tag != "Player")
                {
                    Debug.Log("player in radius range but could be behind something?");
                }
            }
        }
        ///alwin requested a radius around the enemy copy this and put it in wherever youd like to use it
    }

    void detecttheplayer()
    {
        //========remove V V V if you dont want debug========
        leftDir = leftTransform.position - transform.position;
        rightDir = rightTransform.position - transform.position;
        //========remove ^ ^ ^ if you dont want debug========
        headDir = headTransform.position - transform.position;
        playerDirHeadNormalized = headDir.normalized;
        feetDir = feetTransform.position - transform.position;
        playerDirFeetNormalized = feetDir.normalized;
        playerDir = playerTransform.position - transform.position;
        playerDirNormalized = playerDir.normalized;

        //========remove V V V if you dont want debug========
        //Debug.DrawRay(transform.position, playerDirNormalized * 100, Color.blue); // enemy to player raycast
        Debug.DrawRay(transform.position, leftDir.normalized * raycastDistance, Color.gray); // angle left
        Debug.DrawRay(transform.position, rightDir.normalized * raycastDistance, Color.gray); // angle right
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.white); // angle middle 
        //========remove ^ ^ ^ if you dont want debug========

        angle = Vector3.Angle(playerDirNormalized, transform.forward);
        if (angle < fieldOfViewAngle * 0.5f)
        {
            if (Physics.Raycast(transform.position, playerDirNormalized, out hit, raycastToPlayerDistanceLimiter))//, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(transform.position, playerDirNormalized * hit.distance, Color.blue); // enemy to player raycast
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

            if (Physics.Raycast(transform.position, playerDirHeadNormalized, out hitHead, raycastToPlayerDistanceLimiter)) //, layerMask))
            {
                Debug.DrawRay(transform.position, playerDirHeadNormalized * hitHead.distance, Color.blue); // enemy to player raycast
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

            if (Physics.Raycast(transform.position, playerDirFeetNormalized, out hitFeet, raycastToPlayerDistanceLimiter)) //, layerMask))
            {
                Debug.DrawRay(transform.position, playerDirFeetNormalized * hitFeet.distance, Color.blue); // enemy to player raycast
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
