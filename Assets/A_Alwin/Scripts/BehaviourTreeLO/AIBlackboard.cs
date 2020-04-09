using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlwinScript.AI
{
    public class AIBlackboard : MonoBehaviour
    {
        public Transform[] pathPoints;
        public Transform rechargePoint;
        public Vector3 reachPoint;
        public float batteryCharge = 100, compareThrushold = 10, steeringFactor = 0.5f;
        public Vector3 velocity;
        AINode rootNode;

        void Start()
        {
            rootNode = new AISelector();
            // ai charfing section
            AINode chargeCheckSequence = new AISequencer();
            chargeCheckSequence.childNodes.Add(new AICheckCharge());
            chargeCheckSequence.childNodes.Add(new AIReachTargetPoint());
            chargeCheckSequence.childNodes.Add(new AICharger());
            rootNode.childNodes.Add(chargeCheckSequence);

            // ai moveing section
            AINode moveSequence = new AISequencer();
            moveSequence.childNodes.Add(new AIPickWayPoint());
            moveSequence.childNodes.Add(new AIReachTargetPoint());
            rootNode.childNodes.Add(moveSequence);


        }

        void Update()
        {
            rootNode.Run(this);
        }
    }
}