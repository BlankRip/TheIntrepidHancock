using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Weapon : MonoBehaviour
{
    GameObject objectThatCanPickUp;
    public Collider rangeTrigger;
    [HideInInspector] public bool myEquipStatus;
    bool readyToPick;

    //Things needed when attacking
    [Tooltip("The collider that is enabled and desabled when the player is attacking")] 
    [SerializeField] Collider attackCollider;
    [Tooltip("The trail attached to the weapon")] 
    [SerializeField] TrailRenderer weaponTrail;
    [Tooltip("The audio source of this weapon")]
    [SerializeField] AudioSource swingSource;
    [Tooltip("The audio clips played on swing")]
    [SerializeField] AudioClip[] swingClips;

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


        //If equipped can drop the weapon
        if (myEquipStatus)
        {
            if (Input.GetKeyDown(dropKey))
            {
                manageEquipment.DropWeapon(this, weaponRB);      //Giveng the weapon to the equip manager to drop it
            }
        }
        else
        {
            if (Input.GetKeyDown(equipKey) && readyToPick)
            {
                manageEquipment.EquipWeapon(this, weaponRB);      //Giveng the weapon to the equip manager to equip it
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !myEquipStatus)
        {
            readyToPick = true;
            manageEquipment.ShowUI(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            readyToPick = false;
            manageEquipment.NoUI();
        }
    }

    //Function that activates the things needed when attacking
    public void ActivateEffects()
    {
        for (int i = 0; i < swingClips.Length; i++)
        {
            swingSource.PlayOneShot(swingClips[i]);
        }
        weaponTrail.emitting = true;
        attackCollider.enabled = true;
    }

    //Function that deactivates the things that need to be gone when not attacking
    public void DeactivateEffects()
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
