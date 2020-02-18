using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Weapon : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Animator swingAnim;                      //Animator used to play the swing weapon animation
    [SerializeField] Collider attackCollider;                 //The collider that is enabled and desabled when the player is attacking
    [Header("KeyBindings for actions")]
    [SerializeField] KeyCode attackKey = KeyCode.Mouse0;
    [SerializeField] KeyCode equipKey = KeyCode.E;
    [SerializeField] KeyCode dropKey = KeyCode.G;

    [HideInInspector] public bool myEquipState;              //The eqip status of the weapon
    Rigidbody weaponRB;                                      //The rigidboady of the weapon
    EquipManager manageEquipment;                            //The equipment manager which will manage equipping, dropping and swapping of weapons

    void Start()
    {
        myEquipState = false;
        weaponRB = GetComponent<Rigidbody>();
        manageEquipment = FindObjectOfType<EquipManager>();
    }

    void Update()
    {
        //Checking if player is in pick-up range
        if (Vector3.Distance(transform.position, player.transform.position) < 3f && !myEquipState)
        {
            Debug.Log("in range of a weapon press r to equip");
            if (Input.GetKeyDown(equipKey))
            {
                manageEquipment.EquipWeapon(this.gameObject, weaponRB);      //Giveng the weapon to the equip manager to equip it
            }
        }

        //Checking if this weapon is equipped
        if (myEquipState == true)
        {
            if (Input.GetKeyDown(attackKey))
            {
                attackCollider.enabled = true;
                swingAnim.SetTrigger("Attack");
            }

            //Drops the weapon if the player wants to
            if (Input.GetKeyDown(dropKey))
            {
                manageEquipment.DropWeapon(this.gameObject, weaponRB);
            }
        }
    }

    /*The IEnumerator that will set the equipment status to false after the frame ends 
    as if not the check will happen again on the same frame and will re-equip this weapon without swapping*/
    public IEnumerator EquipableAfter()
    {
        yield return new WaitForEndOfFrame();
        myEquipState = false;
    }

    //Function that deactivates the attack collider for this weapon
    public void DeactivateCollider()
    {
        attackCollider.enabled = false;
    }
}
