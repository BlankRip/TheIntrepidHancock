using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject playerObj;


    [HideInInspector] public bool equipped;
    Rigidbody weaponRB;

    WeaponCheck equipper;

    // Start is called before the first frame update
    void Start()
    {
        equipped = false;
        weaponRB = GetComponent<Rigidbody>();
        equipper = FindObjectOfType<WeaponCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, playerObj.transform.position) < 3f && !equipped)
        {
            Debug.Log("in range of a weapon press r to equip");
            if (Input.GetKeyDown(KeyCode.R))
            {
                equipper.EquipWeapon(this.gameObject, weaponRB);
            }
        }

        if(equipped)
        {

        }
        //if (Vector3.Distance(transform.position, playerObj.transform.position) < 3f)
        //{
        //    Debug.Log("in range of a weapon press r to equip");
        //    if (Input.GetKeyDown(KeyCode.R))
        //    {
        //        if (equipCheckAccess.GetComponent<WeaponCheck>().equipCheck == false)
        //        {
        //           equipper.EquipWeapon();
        //        }
        //    }
        //}

        //if (transform.position == weaponPos.position)
        //{
        //    if (Input.GetKeyDown(KeyCode.G))
        //    {
        //        equipper.DropWeapon();
        //    }
        //}
    }



}
