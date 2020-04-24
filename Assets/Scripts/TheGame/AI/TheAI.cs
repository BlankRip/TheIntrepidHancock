using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheAI : MonoBehaviour
{
    public Animator myAnimator;
    [SerializeField] Collider mAttackCollier;

    [Header("For Enemy Sounds")]
    public AudioSource AttackSoundSource;
    [SerializeField] AudioSource leftFootSource;
    [SerializeField] AudioSource rightFootSource;
    [HideInInspector] public bool playingChase;

    #region For Tree Nodes
    TreeNode root;
    [Range(1, 2.5f)] public float randomWeightageAdjuster = 1.8f;

    [Header("Timers for cooldown & Idle")]
    [Range(0, 1)] public float coolDownSpeed = 1;
    [Range(1, 2)] public float idleTimeSpeed = 1;

    [Header("For Chase")]
    public float maxWalkVel = 8;
    public float maxChaseVel = 10;
    public float attackRange;
    public float attackDuration = 2f;
    public float tauntDuration = 5f;
    [HideInInspector] public bool recentlyAttcked;
    [HideInInspector] public bool attacking;


    [Header("For Searching")]
    public int maxNumberOfSearches;
    public int minNumberOfSearches;
    [HideInInspector] public bool setSearchCount = true;
    [HideInInspector] public Vector3 lastSeenPos;
    public bool pathPointeReset = false;

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
    [SerializeField] int radiusRange;

    float rayCastLength;
    float angle;

    Vector3 playerDir;
    Vector3 headDir;
    Vector3 feetDir;
    RaycastHit hit;
    RaycastHit hitHead;
    RaycastHit hitFeet;
    RaycastHit hitRadius;

    bool inCircleRange;
    bool inFieldOfVisionRange;
    bool done;

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
    [HideInInspector] public float maxVelocity;
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
    [SerializeField] float repelPow = 1, rayLength = 2, castOffset = 0.5f, turnStrength;

    RaycastHit hitLeft, hitFront, hitRight;
    Vector3[] castVectors = new Vector3[3];

     [Header("Internal (For Programmers Only)")]
     public float reachRegisterDistance = 2;
     public Vector3 targetPoint;
     
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targerRb = target.GetComponent<Rigidbody>();
        rayCastLength = raycastToPlayerDistanceLimiter;

        // Innitializing the avoidance vector
        castVectors[0] = (Vector3.forward - Vector3.right).normalized;
        castVectors[1] = Vector3.forward * 0.5f;
        castVectors[2] = (Vector3.forward + Vector3.right).normalized;

        #region Creating Tree
        //Creating branch nodes into variables for easy use
        TreeNode attackSequence = new SequenceNode();
  //      TreeNode chaseSequence = new SequenceNode();
        TreeNode postAttackRandomSelect = new RandomSelectorNode();

        TreeNode searchSequence = new SequenceNode();
    //    TreeNode patrolSelector = new SelectorNode();
        TreeNode fallbackRandomSelect = new RandomSelectorNode();

        //Making the tree
        root = new SelectorNode();

        root.refToChildren.Add(attackSequence);
        attackSequence.refToChildren.Add(new PlayerFoundCheckNode());
        attackSequence.refToChildren.Add(new ChasePlayerNode());
        attackSequence.refToChildren.Add(new AttackNode());
        // cooldown or taunt after attack
        attackSequence.refToChildren.Add(postAttackRandomSelect);

        postAttackRandomSelect.refToChildren.Add(new TauntNode());
        postAttackRandomSelect.refToChildren.Add(new CoolDownNode());

        // search for player
        root.refToChildren.Add(searchSequence);
        searchSequence.refToChildren.Add(new PlayerEscapeCheck());
        searchSequence.refToChildren.Add(new SearchNode());

        // patrole section
        root.refToChildren.Add(fallbackRandomSelect);
        fallbackRandomSelect.refToChildren.Add(new PatroleNode());
        fallbackRandomSelect.refToChildren.Add(new IdleNode());
        //patrolSelector.refToChildren.Add(new SearchNode());
        //patrolSelector.refToChildren.Add(patrolRandomSelect);

//        patrolRandomSelect.refToChildren.Add(new PatroleNode());
//        patrolRandomSelect.refToChildren.Add(new IdleNode());


    //    root.refToChildren.Add(chaseSequence);
     //   chaseSequence.refToChildren.Add(new ChasePlayerNode());
     //   chaseSequence.refToChildren.Add(new AttackNode());
    //    chaseSequence.refToChildren.Add(postAttackRandomSelect);


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
/*
            Debug.DrawLine(castPoint.position + transform.rotation * castVectors[0] * castOffset, castPoint.position + castPoint.rotation * castVectors[0] * rayLength, Color.red);
            Debug.DrawLine(castPoint.position + transform.rotation * castVectors[1] * castOffset, castPoint.position + castPoint.rotation * castVectors[1] * rayLength, Color.red);
            Debug.DrawLine(castPoint.position + transform.rotation * castVectors[2] * castOffset, castPoint.position + castPoint.rotation * castVectors[2] * rayLength, Color.red);
     */
     
        rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(rb.velocity.normalized, Vector3.up), Time.fixedDeltaTime * turnStrength);
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
        /*
        Debug.DrawRay(transform.position, leftDir.normalized * raycastDistance, Color.gray); // angle left
        Debug.DrawRay(transform.position, rightDir.normalized * raycastDistance, Color.gray); // angle right
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.white); // angle middle 
        */
        //========remove ^ ^ ^ if you dont want debug========

        angle = Vector3.Angle(playerDir.normalized, transform.forward);

        if (Vector3.Distance(transform.position, playerTransform.position) <= radiusRange) //default value 10
        {
            if (Physics.Raycast(transform.position, playerDir.normalized, out hitRadius, radiusRange))//, Mathf.Infinity, layerMask))
            {
            //    Debug.DrawRay(transform.position, playerDir.normalized * hitRadius.distance, Color.yellow); // enemy to player raycast
                if (hitRadius.collider.tag == "Player")
                {
                    playerFound = true;
                    inCircleRange = true;
                    rayCastLength = Mathf.Infinity;
              //      Debug.Log("<color=pink> DETECTED THE PLAYER Cirlce // raycast hit </color>");
                }
        //                else
              //      Debug.Log("<color=pink> player in range Circle but behind something? </color>");
            }
        }
        else
            inCircleRange = false;

        if (angle < fieldOfViewAngle * 0.5f)
        {
            bool bellyHitCheck = Physics.Raycast(transform.position, playerDir.normalized, out hit, rayCastLength);
            bool headHitCheck = Physics.Raycast(transform.position, headDir.normalized, out hitHead, rayCastLength);
            bool footHitCheck = Physics.Raycast(transform.position, feetDir.normalized, out hitFeet, rayCastLength);
/*
            Debug.DrawRay(transform.position, playerDir.normalized * hit.distance, Color.blue); // enemy to player raycast
            Debug.DrawRay(transform.position, headDir.normalized * hitHead.distance, Color.blue); // enemy to player raycast
            Debug.DrawRay(transform.position, feetDir.normalized * hitFeet.distance, Color.blue); // enemy to player raycast
*/
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                playerFound = true;
                inFieldOfVisionRange = true;
                rayCastLength = Mathf.Infinity;
          //      Debug.Log("<color=pink> DETECTED THE PLAYER // raycast hit </color>");
            }
            else if (hitHead.collider != null && hitHead.collider.CompareTag("Player"))
            {
                playerFound = true;
                inFieldOfVisionRange = true;
                rayCastLength = Mathf.Infinity;
       //         Debug.Log("<color=pink> DETECTED THE PLAYER // raycast hit </color>");
            }
            else if (hitFeet.collider != null && hitFeet.collider.CompareTag("Player"))
            {
                playerFound = true;
                inFieldOfVisionRange = true;
                rayCastLength = Mathf.Infinity;
        //        Debug.Log("<color=pink> DETECTED THE PLAYER // raycast hit </color>");
            }
            else
                inFieldOfVisionRange = false;
        }

        if (!inFieldOfVisionRange && !inCircleRange)
        {
//    Debug.Log("<color=yellow>" + playerFound + "</color>");
            if (playerFound)
            {
                rayCastLength = raycastToPlayerDistanceLimiter;
                playerFound = false;
                justEscaped = true;
                lastSeenPos = playerTransform.position;
            }
        }
    }

    public void OnAttackStartAnimation()
    {
   //     Debug.Log("<color=black> RUNNING S </color>");
        AttackSoundSource.Play();
        mAttackCollier.enabled = true;
    }

    public void OnAttackEndAnimation()
    {
    //     Debug.Log("<color=black> RUNNING E </color>");
        mAttackCollier.enabled = false;
    }

    //Funtions that play the footstep dust as an animation event
    public void RightFootAnimationEvent()
    {
        leftFootSource.Play();
    }

    public void LeftFootAnimationEvent()
    {
        rightFootSource.Play();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(targetPoint, Vector3.one * 2);
    }
}
