﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCheck : MonoBehaviour
{
    [SerializeField] Transform equipPosition;
    bool equipStatus;
    GameObject currentWeapon;
    Rigidbody currentWeaponRb;
    Weapon currentWeaponComponent;
    //
    bool bottleEquip;
    [SerializeField] int throwingBottleForce = 30f;

    public void EquipWeapon(GameObject weapon, Rigidbody rb)
    {
        if(equipStatus == true)
        {
            Debug.Log("<color=green>equpped status true and will drop wep to switch </color>");
            DropWeapon(currentWeapon, currentWeaponRb);
        }
        Debug.Log("<color=yellow>pick up wep now </color>");
        weapon.transform.position = equipPosition.position;
        weapon.transform.rotation = equipPosition.rotation;
        weapon.transform.SetParent(equipPosition);
        rb.isKinematic = true;
        equipStatus = true;
        currentWeaponComponent = weapon.GetComponent<Weapon>();
        currentWeaponComponent.equipped = true;
        currentWeapon = weapon;
        currentWeaponRb = rb;
        if (currentWeapon.tag == "Bottle")
        {
            bottleEquip = true;
            currentWeaponComponent.holdingBottle = true;
            Debug.Log("check if tag is bottle if it is this will ocme up and bottle = true");
        }
    }

    public void DropWeapon(GameObject weapon, Rigidbody rb)
    {
        Debug.Log("<color=red> droping wep </color>");
        weapon.transform.SetParent(null);
        rb.isKinematic = false;
        rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
        currentWeaponComponent = null;
        weapon.GetComponent<Weapon>().equipped = false;
        equipStatus = false;
        bottleEquip = false;
    }

    public void ThrowBottle(GameObject weapon, Rigidbody rb)
    {
        Debug.Log("<color=red> Throwing Bottle </color>");
        weapon.transform.SetParent(null);
        rb.isKinematic = false;
        rb.AddForce(transform.forward * throwingBottleForce, ForceMode.Impulse);
        currentWeaponComponent.holdingBottle = false;
        currentWeaponComponent = null;
        weapon.GetComponent<Weapon>().equipped = false;
        equipStatus = false;
        bottleEquip = false;
    }

    public void DisableColliderEvent()
    {
        if (currentWeaponComponent != null)
        {
            currentWeaponComponent.DeactivateCollider();
        }
    }

}
