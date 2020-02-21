using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPieceExplosion : MonoBehaviour
{
    Rigidbody rb;                                                                    //Rigidbody of the shard or broken piece
    [SerializeField] float explosionForce = 7;                                       //The amount of explosion force
    [Range(0.0f, 0.5f)] [SerializeField] float explosionRaious = 0.01f;              //The radious in which the explosion force is applied
    [SerializeField] float upWardsModifier = 0.00001f;                               //Well, upwards modifier, we do't really need it, depends

    void Start()
    {
        //Gets the regidbody and applies a explosion force to it
        rb = GetComponent<Rigidbody>();
        rb.AddExplosionForce(explosionForce, transform.position, explosionRaious, upWardsModifier, ForceMode.Impulse);
    }
}
