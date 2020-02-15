using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFly : MonoBehaviour
{
    [SerializeField] string targetTag;           //The tag of the object the fierfly shouldFly to
    Transform target;                            //The object that the firefly will go to
    Vector3 startPosition;                       //The the position at which the firefly is spawned
    Vector3 midPoint;                            //The third mid way point required to calculate the bezier curve
    float distance;                              //The distance between the spawnPos and targetPos
    float t;                                     //Tracker to perform the interpolation

    void Start()
    {
        target = GameObject.FindGameObjectWithTag(targetTag).transform;
        startPosition = transform.position;
        t = 0;
    }

    void Update()
    {
        distance = Vector3.Distance(startPosition, target.position);
        //Getting mid point by getting direction to place the point and multiply with half the distance
        midPoint = ((target.up.normalized + startPosition.normalized) / 2) * (distance / 2);
        //Moving the firefly by interpolation wiht the bezier curve equation
        transform.position = (1 - t) * (1 - t) * startPosition + (1 - t) * 2 * t * midPoint + t * t * target.position;
        if (t <= 1)
            t += Time.deltaTime;
        else
            Destroy(gameObject);     //Destroy the object when reached the target

    }
}
