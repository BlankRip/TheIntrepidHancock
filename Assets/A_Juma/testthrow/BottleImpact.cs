using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleImpact : MonoBehaviour
{

    GameObject sphere;
    public bool thrownMode;

    void Awake()
    {
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        thrownMode = false;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            if (thrownMode == true)
            {
                Instantiate(sphere, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }
}
