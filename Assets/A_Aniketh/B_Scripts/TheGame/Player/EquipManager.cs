using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    [Tooltip("The position the weapon will equip")] 
    [SerializeField] Transform equipPosition;
    Player objectToAttachTo;                                        //Status if the player has a weapon equipped
    J_Weapon currentWeapon;                                //The weapon which is corrently equipped
    Rigidbody currentWeaponRb;                               //The rigidbody component attached to the currently equipped weapon

    private void Start()
    {
        objectToAttachTo = FindObjectOfType<Player>();
    }

    public void EquipWeapon(J_Weapon weapon, Rigidbody rb)
    {
        //Checks if player already has a weapon if so drops it
        if (objectToAttachTo.equippedWeapon != null)
        {
            DropWeapon(currentWeapon, currentWeaponRb);
        }
        //Equipping the weapon
        weapon.transform.position = equipPosition.position;
        weapon.transform.rotation = equipPosition.rotation;
        weapon.transform.SetParent(equipPosition);
        rb.isKinematic = true;
        weapon.myEquipStatus = true;
        objectToAttachTo.equippedWeapon = weapon;
        currentWeapon = weapon;
        currentWeaponRb = rb;
    }

    public void DropWeapon(J_Weapon weapon, Rigidbody rb)
    {
        //Dropping the weapon
        weapon.transform.SetParent(null);
        rb.isKinematic = false;
        rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
        StartCoroutine(weapon.EquipableAfter());
        objectToAttachTo.equippedWeapon = null;
    }
}
