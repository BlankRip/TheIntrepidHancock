using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCheck : MonoBehaviour
{

    [SerializeField] Transform equipPosition;
    GameObject currentWeapon;
    Rigidbody currentWeaponRb;


    public void EquipWeapon(GameObject weapon, Rigidbody rb)
    {
        if(currentWeapon != null)
        {
            DropWeapon(currentWeapon, currentWeaponRb);
        }

        weapon.transform.position = equipPosition.position;
        weapon.transform.rotation = equipPosition.rotation;
        weapon.transform.SetParent(equipPosition);
        rb.isKinematic = true;
        weapon.GetComponent<Weapon>().equipped = true;
        currentWeapon = weapon;
        currentWeaponRb = rb;
    }

    void DropWeapon(GameObject weapon, Rigidbody rb)
    {
        weapon.transform.SetParent(null);
        rb.isKinematic = false;
        rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
        weapon.GetComponent<Weapon>().equipped = false;
    }

}
