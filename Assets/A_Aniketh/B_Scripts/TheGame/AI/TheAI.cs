using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheAI : MonoBehaviour
{
    #region For Tree Nodes
    public Animator myAnimator;
    TreeNode root;

    [Header("Timers for cooldown & Idle")]
    [Range(0, 1)] public float coolDownSpeed = 1;
    [Range(1, 2)] public float idleTimeSpeed = 1;

    [Header("For Chase")]
    public float attackRange;

    [Header("For Searching")]
    public int maxNumberOfSearches;
    public int minNumberOfSearches;
    [HideInInspector] public Vector3 lastSeenPos;
    #endregion

    #region Player Detection Variables
    [Header("For Player Detection")]
    public bool playerFound;
    public bool justEscaped;
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform headTransform;
    [SerializeField] Transform feetTransform;
    [SerializeField] int raycastToPlayerDistanceLimiter;
    [SerializeField] float fieldOfViewAngle;
    
    float rayCastLength;
    float angle;

    Vector3 playerDir;
    Vector3 headDir;
    Vector3 feetDir;
    RaycastHit hit;
    RaycastHit hitHead;
    RaycastHit hitFeet;

    //For Debugging
    [Header("For Detection Debugging")]
    public Transform leftTransform;
    public Transform rightTransform;
    Vector3 leftDir;
    Vector3 rightDir;
    public int raycastDistance;
    #endregion

    #region Steering Behavior Variables
    [Header("For Steering")]
    [SerializeField] float maxVelocity;
    [SerializeField] float maxForce;
    [SerializeField] int framesAhead;
    public float slowCircleRadios;
    public GameObject target;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Rigidbody targerRb;
    #endregion

    #region Collision Avoidance Variables
    [Header("For Collision Avoidance")]
    [SerializeField] Transform castPoint;
    [SerializeField] LayerMask repelLayers;
    [SerializeField] float repelPow = 1, rayLength = 2, castOffset = 0.5f;

    RaycastHit hitLeft, hitFront, hitRight;
    Vector3[] castVectors = new Vector3[3];
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targerRb = target.GetComponent<Rigidbody>();
        rayCastLength = raycastToPlayerDistanceLimiter;

        #region Creating Tree
        //Creating branch nodes into variables for easy use
        TreeNode patrolSequence = new SequenceNode();
        TreeNode patrolSelector = new SelectorNode();
        TreeNode patrolRandomSelect = new RandomSelectorNode();

        TreeNode chaseSequence = new SelectorNode();
        TreeNode chaseRandomSelect = new RandomSelectorNode();

        //Making the tree
        root = new SelectorNode();

        root.refToChildren.Add(patrolSequence);
        patrolSequence.refToChildren.Add(new PlayerFoundCheckNode());
        patrolSequence.refToChildren.Add(patrolSelector);

        patrolSelector.refToChildren.Add(new SearchNode());
        patrolSelector.refToChildren.Add(patrolRandomSelect);

        patrolRandomSelect.refToChildren.Add(new PatroleNode());
        patrolRandomSelect.refToChildren.Add(new IdleNode());


        root.refToChildren.Add(chaseSequence);
        chaseSequence.refToChildren.Add(new ChasePlayerNode());
        chaseSequence.refToChildren.Add(new AttackNode());
        chaseSequence.refToChildren.Add(chaseRandomSelect);

        chaseRandomSelect.refToChildren.Add(new TauntNode());
        chaseRandomSelect.refToChildren.Add(new CoolDownNode());
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        playerDetection();
        root.Run(this);
    }


    public Vector3 Seek(Vector3 seekPoition, float slowRadios)
    {
        Vector3 desigeredVelocity = (seekPoition - transform.position).normalized * maxVelocity;

        float distance = Vector3.Distance(transform.position, seekPoition);
        if (distance < slowRadios)
            desigeredVelocity = desigeredVelocity * (distance / slowRadios);

        Vector3 steering = (desigeredVelocity - rb.velocity);
        if (steering.magnitude > maxForce)
        {
            steering = steering.normalized * maxForce;
        }
        return steering;
    }

    public Vector3 Pursuit(Vector3 seekPoition, Rigidbody targetRigidBody, float slowRadios)
    {
        Vector3 futurePos = seekPoition + targetRigidBody.velocity * framesAhead;
        Vector3 steering = Seek(futurePos, slowRadios);
        return steering;
    }

    public Vector3 CollisionAvoidance()
    {
        Vector3 avoidanceVector = Vector3.zero;
        // casts three rays
        if (!Physics.Raycast(castPoint.position + transform.rotation * castVectors[0] * castOffset, transform.rotation * castVectors[0], out hitLeft, rayLength, repelLayers))
            hitLeft.distance = rayLength;
        if (!Physics.Raycast(castPoint.position + transform.rotation * castVectors[1] * castOffset, transform.rotation * castVectors[1], out hitFront, rayLength, repelLayers))
            hitFront.distance = rayLength;
        if (!Physics.Raycast(castPoint.position + transform.rotation * castVectors[2] * castOffset, transform.rotation * castVectors[2], out hitRight, rayLength, repelLayers))
            hitRight.distance = rayLength;

        float factor_Left = (1 - (hitLeft.distance / rayLength));
        float factor_Front = (1 - (hitFront.distance / rayLength));
        float factor_Right = (1 - (hitRight.distance / rayLength));

        // repel vector addition
        avoidanceVector += repelPow * transform.right * factor_Left;
        avoidanceVector += repelPow * -transform.forward * factor_Front;
        avoidanceVector += repelPow * -transform.right * factor_Right;
        Debug.Log("<color=red>Avoiding</color>");
        rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(rb.velocity.normalized, Vector3.up), Time.fixedDeltaTime * 10);
        return avoidanceVector;
    }

    public void playerDetection()
    {
        headDir = headTransform.position - transform.position;
        feetDir = feetTransform.position - transform.position;
        playerDir = playerTransform.position - transform.position;

        //========remove V V V if you dont want debug========
        leftDir = leftTransform.position - transform.position;
        rightDir = rightTransform.position - transform.position;
        Debug.DrawRay(transform.position, leftDir.normalized * raycastDistance, Color.gray); // angle left
        Debug.DrawRay(transform.position, rightDir.normalized * raycastDistance, Color.gray); // angle right
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.white); // angle middle 
        //========remove ^ ^ ^ if you dont want debug========

        angle = Vector3.Angle(playerDir.normalized, transform.forward);
        if (angle < fieldOfViewAngle * 0.5f)
        {
            bool bellyHitCheck = Physics.Raycast(transform.position, playerDir.normalized, out hit, playerDir.magnitude);
            bool headHitCheck = Physics.Raycast(transform.position, headDir.normalized, out hitHead, headDir.magnitude);
            bool footHitCheck = Physics.Raycast(transform.position, feetDir.normalized, out hitFeet, feetDir.magnitude);

            Debug.DrawRay(transform.position, playerDir.normalized * hit.distance, Color.blue); // enemy to player raycast
            Debug.DrawRay(transform.position, headDir.normalized * hitHead.distance, Color.blue); // enemy to player raycast
            Debug.DrawRay(transform.position, feetDir.normalized * hitFeet.distance, Color.blue); // enemy to player raycast
          /*
            if (hit.collider.tag == "Player" || hitHead.collider.tag == "Player" || hitFeet.collider.tag == "Player")
            {
                playerFound = true;
                rayCastLength = Mathf.Infinity;
                Debug.Log("<color=pink> DETECTED THE PLAYER // raycast hit </color>");
            }
            */
    /*        
             if (bellyHitCheck )
            {
                playerFound = true;
                rayCastLength = Mathf.Infinity;
                Debug.Log("<color=pink> DETECTED THE PLAYER // raycast hit </color>");
            }
*/
            else
                Debug.Log("<color=pink> player in range but behind something? </color>");
        }
        else
        {
            Debug.Log("<color=yellow>" + playerFound + "</color>");
            if (playerFound)
            {
                rayCastLength = raycastToPlayerDistanceLimiter;
                playerFound = false;
                justEscaped = true;
                lastSeenPos = feetTransform.position;
            }
        }
    }
}
