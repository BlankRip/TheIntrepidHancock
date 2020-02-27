using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Weapon : MonoBehaviour
{
    [SerializeField] GameObject objectThatCanPickUp;
    [SerializeField] Collider attackCollider;                 //The collider that is enabled and desabled when the player is attacking
    [HideInInspector] public bool myEquipStatus;
    [Header("KeyBindings for actions")]
    [SerializeField] KeyCode equipKey = KeyCode.E;           //The key pressed to equip weapon
    [SerializeField] KeyCode dropKey = KeyCode.F;           //The key pressed to drop weapon
    Rigidbody weaponRB;                                      //The rigidboady of the weapon
    EquipManager manageEquipment;                            //The equipment manager which will manage equipping, dropping and swapping of weapons

    void Start()
    {
        weaponRB = GetComponent<Rigidbody>();
        manageEquipment = FindObjectOfType<EquipManager>();
    }

    void Update()
    {
        //Checking if player is in pick-up range
        if (Vector3.Distance(transform.position, objectThatCanPickUp.transform.position) < 3f && !myEquipStatus)
        {
            Debug.Log("in range of a weapon press E to equip");
            if (Input.GetKeyDown(equipKey))
            {
                manageEquipment.EquipWeapon(this, weaponRB, attackCollider);      //Giveng the weapon to the equip manager to equip it
            }
        }

        if(myEquipStatus)
        {
            if (Input.GetKeyDown(dropKey))
            {
                manageEquipment.DropWeapon(this, weaponRB);      //Giveng the weapon to the equip manager to drop it
            }
        }
    }

    /*The IEnumerator that will set the equipment status to false after the frame ends 
    as if not the check will happen again on the same frame and will re-equip this weapon without swapping*/
    public IEnumerator EquipableAfter()
    {
        yield return new WaitForEndOfFrame();
        myEquipStatus = false;
    }
}
