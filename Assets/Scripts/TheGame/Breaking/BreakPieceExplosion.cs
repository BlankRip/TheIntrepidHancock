using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPieceExplosion : MonoBehaviour
{
    Rigidbody rb;                                                                    //Rigidbody of the shard or broken piece
    [Tooltip("The amount of explosion force")] 
    [SerializeField] float explosionForce = 7;
    [Tooltip("The radious in which the explosion force is applied")] 
    [Range(0.0f, 0.5f)] [SerializeField] float explosionRaious = 0.01f;
    [Tooltip("Well, upwards modifier, we do't really need it, depends")] 
    [SerializeField] float upWardsModifier = 0.00001f;
    Collider collider;

    void Start()
    {
        //Gets the regidbody and applies a explosion force to it
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<MeshCollider>();
        rb.AddExplosionForce(explosionForce, transform.position, explosionRaious, upWardsModifier, ForceMode.Impulse);
    }

    private void Update()
    {
        if(rb.IsSleeping())
        {
            rb.isKinematic = true;
            collider.enabled = false;
        }
    }
}
