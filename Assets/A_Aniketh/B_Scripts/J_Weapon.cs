using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Weapon : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Animator swingAnim;
    [SerializeField] Collider attackCollider;
    [Header("KeyBindings for actions")]
    [SerializeField] KeyCode attackKey = KeyCode.Mouse0;
    [SerializeField] KeyCode equipKey = KeyCode.R;
    [SerializeField] KeyCode dropKey = KeyCode.G;
    [HideInInspector] public bool myEquipState;
    Rigidbody weaponRB;
    EquipManager manageEquipment;

    void Start()
    {
        myEquipState = false;
        weaponRB = GetComponent<Rigidbody>();
        manageEquipment = FindObjectOfType<EquipManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 3f && !myEquipState)
        {
            Debug.Log("in range of a weapon press r to equip");
            if (Input.GetKeyDown(equipKey))
            {
                manageEquipment.EquipWeapon(this.gameObject, weaponRB);
            }
        }

        if (myEquipState == true)
        {
            if (Input.GetKeyDown(attackKey))
            {
                attackCollider.enabled = true;
                swingAnim.SetTrigger("Attack");
            }

            if (Input.GetKeyDown(dropKey))
            {
                manageEquipment.DropWeapon(this.gameObject, weaponRB);
            }
        }
    }

    public IEnumerator EquipableAfter()
    {
        yield return new WaitForEndOfFrame();
        myEquipState = false;
    }

    public void DeactivateCollider()
    {
        attackCollider.enabled = false;
    }
}
