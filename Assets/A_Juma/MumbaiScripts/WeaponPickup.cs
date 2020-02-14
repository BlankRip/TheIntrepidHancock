using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{ // PLEASE DO NOT CHANGE ANYTHING I WILL PROBABLY CONDENSE THIS // SHOULDVE USED A STATE MACHINE FOR THIS BUT WHATEVER JUST LEAVE AS IT IS PLEASE THANKS :)
    public Transform longSword;
    public Transform shortSword;

    public Transform playerHand;

    Rigidbody rbLS;
    Rigidbody rbSS;

    public bool equipLS;
    public bool equipSS;

    public bool open2LS;
    public bool open2SS;

    public bool emptyHand; //<--- Problem Cause
    bool startRotationPosition;


    void Start()
    {
        rbLS = longSword.GetComponent<Rigidbody>();
        rbSS = shortSword.GetComponent<Rigidbody>();
        startRotationPosition = false;
        equipLS = false;
        equipSS = false;
        emptyHand = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (emptyHand == true || (open2LS == true && open2SS == false))
        {
            if (Vector3.Distance(longSword.transform.position, transform.position) < 3f) // LONGSWORD // might change 2 raycast
            {
                Debug.Log("in range of long sword press r to equip");
                if (Input.GetKeyDown(KeyCode.R))
                {
                    if (equipSS == true)
                    {
                        DropShortSword();
                        //rbSS.AddForce(transform.forward * 20f, ForceMode.Impulse);
                    }
                    equipLS = true;
                    startRotationPosition = true;
                }
            }
        }

        if (emptyHand == true || (open2SS == true && open2LS == false))
        {
            if (Vector3.Distance(shortSword.transform.position, transform.position) < 3f) // SHORTSWORD // might change 2 raycast
            {
                Debug.Log("in range of short sword press r to equip");
                if (Input.GetKeyDown(KeyCode.R))
                {
                    if (equipLS == true)
                    {
                        DropLongSword();
                        //rbLS.AddForce(transform.forward * 20f, ForceMode.Impulse);
                    }
                    equipSS = true;
                    startRotationPosition = true;
                }
            }
        }

        if (equipLS == true)
        {
            open2LS = false;
            open2SS = true;
            emptyHand = false;
            equipSS = false;
            longSword.SetParent(playerHand);
            rbLS.isKinematic = true;
            if (startRotationPosition == true)
            {
                longSword.position = playerHand.position; //new Vector3(0, 0, 0);
                longSword.rotation = playerHand.rotation; //Quaternion.identity;
                startRotationPosition = false;
            }
            //longSword.position = playerHand.position;
            //longSword.rotation = playerHand.rotation;
            if (Input.GetKeyDown(KeyCode.G))
            {
                DropLongSword();
                open2LS = true;
                open2SS = true;
                emptyHand = true;
            }
        }
        else
        {

        }

        if (equipSS == true)
        {
            open2LS = true;
            open2SS = false;
            emptyHand = false;
            equipLS = false;
            shortSword.SetParent(playerHand);
            rbSS.isKinematic = true;
            if (startRotationPosition == true)
            {
                shortSword.position = playerHand.position; //new Vector3(0, 0, 0);
                shortSword.rotation = playerHand.rotation; //Quaternion.Euler(0,0,0);
                startRotationPosition = false;
            }
            //shortSword.position = playerHand.position;
            //shortSword.rotation = playerHand.rotation;
            if (Input.GetKeyDown(KeyCode.G))
            {
                DropShortSword();
                open2LS = true;
                open2SS = true;
                emptyHand = true;
            }
        }
        else
        {

        }
        /*
        if (Input.GetMouseButtonDown(0))
        {
            playerHand.position = Vector3.Lerp(playerHand.position, new Vector3(playerHand.position.x, playerHand.position.y, playerHand.position.z + 2f), 20f * Time.deltaTime);
            playerHand.position = Vector3.Lerp(playerHand.position, new Vector3(playerHand.position.x, playerHand.position.y, playerHand.position.z - 2f), 10f * Time.deltaTime);
        }
        */

        void DropLongSword()
        {
            longSword.SetParent(null);
            rbLS.isKinematic = false;
            //rbLS.AddForce(new Vector3(0, 0, 10), ForceMode.Impulse);
            rbLS.AddForce(transform.forward * 10f, ForceMode.Impulse);
            equipLS = false;
        }

        void DropShortSword()
        {
            shortSword.SetParent(null);
            rbSS.isKinematic = false;
            //rbSS.AddForce(new Vector3(0, 0, 10), ForceMode.Impulse);
            rbSS.AddForce(transform.forward * 10f, ForceMode.Impulse);
            equipSS = false;
        }
    }
}

        /*// Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }*/