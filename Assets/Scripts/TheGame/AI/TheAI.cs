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
        TreeNode postAttackRandomSelect = new RandomSelectorNode();

        TreeNode searchSequence = new SequenceNode();
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

    public Vector3 Pursuit(Vector3 seekPoition, Rigidbody targetRigidBody, float slowRadios, float persuitRadius)
    {
        float distance = Vector3.Distance(transform.position, seekPoition);

        float currentFramesAhead;
        if (distance < persuitRadius)
            currentFramesAhead = distance / persuitRadius;
        else
            currentFramesAhead = framesAhead;

        Vector3 futurePos = seekPoition + targetRigidBody.velocity * currentFramesAhead;
        Vector3 steering = Seek(futurePos, slowRadios/3);
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
     
        rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(rb.velocity.normalized, Vector3.up), Time.fixedDeltaTime * turnStrength);
        return avoidanceVector;
    }

    public void playerDetection()
    {
        headDir = headTransform.position - transform.position;
        feetDir = feetTransform.position - transform.position;
        playerDir = playerTransform.position - transform.position;

        angle = Vector3.Angle(playerDir.normalized, transform.forward);

        if (Vector3.Distance(transform.position, playerTransform.position) <= radiusRange) //default value 10
        {
            if (Physics.Raycast(transform.position, playerDir.normalized, out hitRadius, radiusRange))//, Mathf.Infinity, layerMask))
            {
                if (hitRadius.collider.tag == "Player")
                {
                    playerFound = true;
                    inCircleRange = true;
                    rayCastLength = Mathf.Infinity;
                }
            }
        }
        else
            inCircleRange = false;

        if (angle < fieldOfViewAngle * 0.5f)
        {
            bool bellyHitCheck = Physics.Raycast(transform.position, playerDir.normalized, out hit, rayCastLength);
            bool headHitCheck = Physics.Raycast(transform.position, headDir.normalized, out hitHead, rayCastLength);
            bool footHitCheck = Physics.Raycast(transform.position, feetDir.normalized, out hitFeet, rayCastLength);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                playerFound = true;
                inFieldOfVisionRange = true;
                rayCastLength = Mathf.Infinity;
            }
            else if (hitHead.collider != null && hitHead.collider.CompareTag("Player"))
            {
                playerFound = true;
                inFieldOfVisionRange = true;
                rayCastLength = Mathf.Infinity;
            }
            else if (hitFeet.collider != null && hitFeet.collider.CompareTag("Player"))
            {
                playerFound = true;
                inFieldOfVisionRange = true;
                rayCastLength = Mathf.Infinity;
            }
            else
                inFieldOfVisionRange = false;
        }

        if (!inFieldOfVisionRange && !inCircleRange)
        {
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
        AttackSoundSource.Play();
        mAttackCollier.enabled = true;
    }

    public void OnAttackEndAnimation()
    {
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
