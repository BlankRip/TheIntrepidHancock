using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    [Tooltip("The position the weapon will equip")] 
    [SerializeField] Transform equipPosition;
    [Tooltip("How much above the weapon will the UI appere")]
    [SerializeField] float UIHight = 0.5f;
    Player objectToAttachTo;                                        //Status if the player has a weapon equipped
    J_Weapon currentWeapon;                                //The weapon which is corrently equipped
    Rigidbody currentWeaponRb;                               //The rigidbody component attached to the currently equipped weapon
    GameObject pickUpUI;                                    //The UI text object

    private void Start()
    {
        objectToAttachTo = FindObjectOfType<Player>();
        pickUpUI = GameManager.instance.eUIobj;
    }

    public void EquipWeapon(J_Weapon weapon, Rigidbody rb)
    {
        //Checks if player already has a weapon if so drops it
        if (objectToAttachTo.equippedWeapon != null)
            DropWeapon(currentWeapon, currentWeaponRb);
        //Equipping the weapon
        weapon.rangeTrigger.SetActive(false);
        NoUI();
        weapon.transform.position = equipPosition.position;
        weapon.transform.SetParent(equipPosition);
        weapon.transform.localRotation = Quaternion.identity;
        rb.isKinematic = true;
        weapon.myEquipStatus = true;
        objectToAttachTo.equippedWeapon = weapon;
        currentWeapon = weapon;
        currentWeaponRb = rb;
    }

    public void DropWeapon(J_Weapon weapon, Rigidbody rb)
    {
        //Dropping the weapon
        GameManager.instance.eResetUI = true;
        weapon.transform.SetParent(null);
        rb.isKinematic = false;
        rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
        StartCoroutine(weapon.EquipableAfter());
        weapon.rangeTrigger.SetActive(true); ;
        objectToAttachTo.equippedWeapon = null;
    }

    public void ShowUI(J_Weapon weapon)
    {
        pickUpUI.transform.position = new Vector3(weapon.transform.position.x, weapon.transform.position.y + UIHight, weapon.transform.position.z);
        GameManager.instance.eResetUI = true;
        pickUpUI.SetActive(true);
    }

    public void NoUI()
    {
        pickUpUI.SetActive(false);
    }
}
