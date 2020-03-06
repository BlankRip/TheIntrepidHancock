using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBot : MonoBehaviour
{
    public float speed = 20;
    public Transform targetLocation;
    public Transform chargePoint;
    public Transform[] wayPoints;
    public float charge;
    public float chargeDrain, rechargeThrushold;

    public MoveBotState currnt;
 

    void Start()
    {
        charge = 100;
        rechargeThrushold = 10;
        currnt = new MoveBotPatrole();
        currnt.AssignBot(this);

    }

    private void Update()
    {
        currnt.Execute();


        /*
        // patroling
        if (charge > rechargeThrushold)
        {

            if (targetLocation == null)
            {
                targetLocation = wayPoints[Random.Range(0, wayPoints.Length)];
            }

            float distance = (targetLocation.position - transform.position).magnitude;

            if (distance < 0.5f)
            {
                targetLocation = wayPoints[Random.Range(0, wayPoints.Length)];
            }

            else
            {
                MoveTo();
            }

        }

        // charge
        else
        {
            rechargeThrushold = 100;

            float distance = (chargePoint.position - transform.position).magnitude;

            if (distance > 1)
            {
                targetLocation = chargePoint;
                MoveTo();
            }
            else
            {
                charge += 10 * Time.deltaTime;

                if (charge > 95)
                {
                    rechargeThrushold = 10;
                }
            }
        }
        */

    }
/*
    void MoveTo()
    {
        transform.Translate((targetLocation.position - transform.position).normalized * Time.deltaTime * speed);
        charge -= chargeDrain * Time.deltaTime;
    }
    */
}
