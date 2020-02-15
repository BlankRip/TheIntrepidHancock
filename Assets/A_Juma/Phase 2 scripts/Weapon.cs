using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject playerObj;

    public Transform weaponPos;

    Rigidbody weaponRB;

    GameObject equipCheckAccess;


    // Start is called before the first frame update
    void Start()
    {
        weaponRB = GetComponent<Rigidbody>();
        equipCheckAccess = GameObject.FindGameObjectWithTag("WeaponCheckTag");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, playerObj.transform.position) < 3f)
        {
            Debug.Log("in range of a weapon press r to equip");
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (equipCheckAccess.GetComponent<WeaponCheck>().equipCheck == false)
                {
                    EquipWeapon();
                }
                if (equipCheckAccess.GetComponent<WeaponCheck>().equipCheck == true)
                {

                }
            }
        }
    }

    public void EquipWeapon()
    {
        transform.position = weaponPos.position;
        transform.rotation = weaponPos.rotation;
        transform.SetParent(weaponPos);
        weaponRB.isKinematic = true;
        equipCheckAccess.GetComponent<WeaponCheck>().equipCheck = true;
    }

    public void DropWeapon()
    {
        transform.SetParent(null);
        weaponRB.isKinematic = false;
        weaponRB.AddForce(Vector3.forward * 10f, ForceMode.Impulse);
        equipCheckAccess.GetComponent<WeaponCheck>().equipCheck = false;
    }
}
