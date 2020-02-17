using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    [SerializeField] Transform equipPosition;
    bool equipStatus;
    GameObject currentWeapon;
    Rigidbody currentWeaponRb;
    J_Weapon currentWeaponComponent;


    public void EquipWeapon(GameObject weapon, Rigidbody rb)
    {
        if (equipStatus == true)
        {
            Debug.Log("<color=green>equpped status true and will drop wep to switch </color>" + weapon);
            DropWeapon(currentWeapon, currentWeaponRb);
        }
        Debug.Log("<color=yellow>pick up wep now </color>" + weapon);
        weapon.transform.position = equipPosition.position;
        weapon.transform.rotation = equipPosition.rotation;
        weapon.transform.SetParent(equipPosition);
        rb.isKinematic = true;
        equipStatus = true;
        currentWeaponComponent = weapon.GetComponent<J_Weapon>();
        currentWeaponComponent.myEquipState = true;
        currentWeapon = weapon;
        currentWeaponRb = rb;
    }

    public void DropWeapon(GameObject weapon, Rigidbody rb)
    {
        Debug.Log("<color=red> droping wep </color>" + weapon);
        weapon.transform.SetParent(null);
        rb.isKinematic = false;
        rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
        StartCoroutine(currentWeaponComponent.EquipableAfter());
        currentWeaponComponent = null;
        equipStatus = false;
    }

    public void DisableColliderEvent()
    {
        if (currentWeaponComponent != null)
        {
            currentWeaponComponent.DeactivateCollider();
        }
    }
}
