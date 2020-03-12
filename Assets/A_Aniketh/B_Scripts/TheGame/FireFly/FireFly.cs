using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFly : MonoBehaviour
{
    [Tooltip("The tag of the object the fierfly shouldFly to")] [SerializeField] string targetTag;
    [Tooltip("The higher this value the more curvy it is")] [SerializeField] float curveMultiplier;

    Transform target;                            //The object that the firefly will go to
    Vector3 startPosition;                       //The the position at which the firefly is spawned
    Vector3 midPoint;                            //The third mid way point required to calculate the bezier curve
    Vector3 randomeUP;                           //The random unit vector added to make the curve random
    float distance;                              //The distance between the spawnPos and targetPos
    float t;                                     //Tracker to perform the interpolation

    void Start()
    {
        target = GameObject.FindGameObjectWithTag(targetTag).transform;
        randomeUP = Random.insideUnitSphere.normalized * curveMultiplier;
        startPosition = transform.position;
        t = 0;
    }

    void Update()
    {
        distance = Vector3.Distance(startPosition, target.position);
        //Getting mid point by getting direction to place the point and multiply with half the distance
        midPoint = (((target.up + startPosition).normalized) * (distance / 2)) + randomeUP;
        //Moving the firefly by interpolation wiht the bezier curve equation
        transform.position = (1 - t) * (1 - t) * startPosition + (1 - t) * 2 * t * midPoint + t * t * target.position;
        if (t <= 1)
            t += Time.deltaTime;
        else
            Destroy(gameObject);     //Destroy the object when reached the target

    }
}
