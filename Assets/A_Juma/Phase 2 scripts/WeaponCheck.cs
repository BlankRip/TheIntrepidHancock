using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCheck : MonoBehaviour
{

    public bool equipCheck;
    public GameObject weaponType1;
    public GameObject weaponType2;
    public GameObject weaponType3;

    // Start is called before the first frame update
    void Start()
    {
        equipCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (equipCheck == true)
        {
            Debug.Log("Player currently has something equipped press G to drop OR press r to equip a new weapon and drop the currently held weapon");

            if (Input.GetKeyDown(KeyCode.G))
            {
                weaponType1.GetComponent<Weapon>().DropWeapon();
                //transform.DetachChildren();       r these 2 the same?         transform.SetParent(null);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
               // weaponType1.GetComponent<Weapon>().DropWeapon();
            }
        }
    }
}
