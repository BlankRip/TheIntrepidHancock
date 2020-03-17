using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI : MonoBehaviour
{
    public Transform castPoint;
    public Vector3[] waypoints;
    public int currentPointIndex;
    public Vector3 nextPos;
    public float speed;
    public float targetShiftRange;
    Transform player;
    RaycastHit hit;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // get the way point
        if (waypoints.Length == 0)
        {
            CollectWayPoints();
        }


        // execute behaviours

        else
        {
            Vector3 dir = player.position - castPoint.position;
            bool playerInSight = Physics.Raycast(castPoint.position, dir, out hit);

            if (playerInSight && hit.collider.gameObject.CompareTag("Player"))
            {
                dir.y = 0;
                transform.Translate(dir.normalized * speed * Time.deltaTime);
            }
            else
            {
                Vector3 vecToGoal = nextPos - transform.position;
                float distToGoal = vecToGoal.magnitude;

                if (distToGoal < targetShiftRange)
                {
                    currentPointIndex += 1;
                    if (currentPointIndex == waypoints.Length)
                    {
                        CollectWayPoints();
                    }
                    else
                    {
                        nextPos = waypoints[currentPointIndex];
                    }
                }
                Vector3 moveDir = nextPos - transform.position;
                moveDir.y = 0;
                transform.Translate(moveDir.normalized * speed * Time.deltaTime);
            }
        }
        

    }

    public void CollectWayPoints()
    {
        waypoints = SampleEnemyCtrl.instance.GetMeRoute(transform);
        currentPointIndex = 0;
        nextPos = waypoints[currentPointIndex];
    }
}
