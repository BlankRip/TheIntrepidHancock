using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Transform longSword;
    public Transform shortSword;
    public Transform playerHand;
    bool equipLS;
    bool equipSS;


    void Start()
    {
        equipLS = false;
        equipSS = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(longSword.transform.position, transform.position) < 3f) // might change 2 raycast
        {
            Debug.Log("in range of long sword press r to equip");
            if (Input.GetKeyDown(KeyCode.R))
            {
                equipLS = true;
            }
        }

        if (Vector3.Distance(shortSword.transform.position, transform.position) < 3f) // might change 2 raycast
        {
            Debug.Log("in range of short sword press r to equip");
            if (Input.GetKeyDown(KeyCode.R))
            {
                equipSS = true;
            }
        }

        if (equipLS == true)
        {
            equipSS = false;
            longSword.position = playerHand.position;
            longSword.rotation = playerHand.rotation;
            if (Input.GetKeyDown(KeyCode.G))
            {
                equipLS = false;
            }
        }
        else
        {

        }

        if (equipSS == true)
        {
            equipLS = false;
            shortSword.position = playerHand.position;
            shortSword.rotation = playerHand.rotation;
            if (Input.GetKeyDown(KeyCode.G))
            {
                equipSS = false;
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
    }
}
