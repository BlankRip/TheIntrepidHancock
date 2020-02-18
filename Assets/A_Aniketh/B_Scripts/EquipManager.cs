using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    [SerializeField] Transform equipPosition;                //The position the weapon will equip
    bool equipStatus;                                        //Status if the player has a weapon equipped
    GameObject currentWeapon;                                //The weapon which is corrently equipped
    Rigidbody currentWeaponRb;                               //The rigidbody component attached to the currently equipped weapon
    J_Weapon currentWeaponComponent;                         //The Weapon script attached to the currently equipped weapon


    public void EquipWeapon(GameObject weapon, Rigidbody rb)
    {
        //Checks if player already has a weapon if so drops it
        if (equipStatus == true)
        {
            DropWeapon(currentWeapon, currentWeaponRb);
        }
        //Equipping the weapon
        weapon.transform.position = equipPosition.position;
        weapon.transform.rotation = equipPosition.rotation;
        weapon.transform.SetParent(equipPosition);
        rb.isKinematic = true;
        equipStatus = true;
        currentWeaponComponent = weapon.GetComponent<J_Weapon>();
        currentWeaponComponent.myEquipState = true;                        //Setting the currently equipped weapon to set its satatus to true
        currentWeapon = weapon;
        currentWeaponRb = rb;
    }

    public void DropWeapon(GameObject weapon, Rigidbody rb)
    {
        //Dropping the weapon
        weapon.transform.SetParent(null);
        rb.isKinematic = false;
        rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
        StartCoroutine(currentWeaponComponent.EquipableAfter());           //Starting a Couroutine which will set the weapons equip status to false after the frame ends
        currentWeaponComponent = null;
        equipStatus = false;
    }

    //Dessabel the collider for the weapon after the swing done in a animation event so stored as public funtion
    public void DisableColliderEvent()
    {
        if (currentWeaponComponent != null)
        {
            currentWeaponComponent.DeactivateCollider();                   //Disables the attack collider for the currently equipped weapon
        }
    }
}
