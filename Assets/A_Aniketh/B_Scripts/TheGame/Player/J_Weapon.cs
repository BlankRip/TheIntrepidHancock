using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Weapon : MonoBehaviour
{
    [Tooltip("The object can pick this weapon up")] 
    [SerializeField] GameObject objectThatCanPickUp;
    [HideInInspector] public bool myEquipStatus;

    //Things needed when attacking
    [Tooltip("The collider that is enabled and desabled when the player is attacking")] 
    [SerializeField] Collider attackCollider;
    [Tooltip("The trail attached to the weapon")] 
    [SerializeField] TrailRenderer weaponTrail;
    [HideInInspector] public bool activateEffects;            //Bool to activate things that are needed to be done when attacking

    [Header("KeyBindings for actions")]
    [Tooltip("The key pressed to equip weapon")] 
    [SerializeField] KeyCode equipKey = KeyCode.E;
    [Tooltip("The key pressed to drop weapon")] 
    [SerializeField] KeyCode dropKey = KeyCode.F;

    Rigidbody weaponRB;                                      //The rigidboady of the weapon
    EquipManager manageEquipment;                            //The equipment manager which will manage equipping, dropping and swapping of weapons

    void Start()
    {
        objectThatCanPickUp = GameObject.FindGameObjectWithTag("Player");
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
                manageEquipment.EquipWeapon(this, weaponRB);      //Giveng the weapon to the equip manager to equip it
            }
        }
        //If equipped can drop the weapon
        if (myEquipStatus)
        {
            if (Input.GetKeyDown(dropKey))
            {
                manageEquipment.DropWeapon(this, weaponRB);      //Giveng the weapon to the equip manager to drop it
            }

            //Actives effects for the weapon and its attack collider
            if (activateEffects)
                ActivateEffects();
            else
                DeactivateEffects();
        }


    }

    //Function that activates the things needed when attacking
    void ActivateEffects()
    {
        weaponTrail.emitting = true;
        attackCollider.enabled = true;
    }

    //Function that deactivates the things that need to be gone when not attacking
    void DeactivateEffects()
    {
        attackCollider.enabled = false;
        weaponTrail.emitting = false;
    }

    /*The IEnumerator that will set the equipment status to false after the frame ends 
    as if not the check will happen again on the same frame and will re-equip this weapon without swapping*/
    public IEnumerator EquipableAfter()
    {
        yield return new WaitForEndOfFrame();
        myEquipStatus = false;
        DeactivateEffects();
    }
}
