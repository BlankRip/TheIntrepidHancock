using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheAI : MonoBehaviour
{
    public bool playerFound;
    public Animator myAnimator;
    [Range(0, 1)] public float coolDownSpeed = 1;
    [Range(1, 2)] public float idleTimeSpeed = 1;

    TreeNode root;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        root.Run(this);
    }


    public void Seek(Vector3 seekPoition, float slowRadios)
    {

    }

    public void Persuit(Vector3 seekPoition, Rigidbody targetRigidBody, float slowRadios)
    {

    }

    public void CollisionAvoidance()
    {

    }

    public void playerDetection()
    {

    }
}
