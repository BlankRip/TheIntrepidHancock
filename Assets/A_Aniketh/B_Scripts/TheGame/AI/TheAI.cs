using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheAI : MonoBehaviour
{
    public bool playerFound;
    public Animator myAnimator;
    [Range(0, 1)] public float coolDownSpeed = 1;

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
}
