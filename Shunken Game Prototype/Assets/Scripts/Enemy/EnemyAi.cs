using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FIMSpace.Basics;
using FIMSpace.GroundFitter;
using UnityEngine.Animations.Rigging;


public class EnemyAi : MonoBehaviour
{
   
    //Audio
    [SerializeField] string backGroundMusicTitle = "Background Music", combatMusicTitle = "Combat Song Flute";
    //GroundFitter
    private FGroundFitter fitter;
    //
    [SerializeField] NavMeshAgent agent;
  
    Vector3 originalPosition;
    //Animation
   public Animator animator;
    bool isMoving;
    public float attackReactionDuration;
    //What Type of Enemy?
    [SerializeField] string enemyType;
    [SerializeField] Rig centipedeWalkingRigLeft, centipedeWalkingRigRight, centipedeSpineRig, centipedeHindLegsRig;
    //other scripts
    [SerializeField] EnemyBehavior enemyBehavior;
    PlayerBehavior playerBehavior;
  
    private AntAttackSequence ant;
    //targetting
 
    public Transform player, attackPoint, targetPoint;
   [SerializeField] Transform headTransform;
   // private LineRenderer laserLine;
    public LayerMask whatIsGround, whatIsPlayer;
    //Audio

    //Centipede
    [SerializeField] float centipedeThreatenTime;
    [SerializeField] CentipedeAttackSequence centipedeAttack;
    //Grasshopper
    private GrasshopperAttackSequence grasshopper;
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    //PlayerDetection
    RaycastHit hit;
    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    private float timer;

    //EnemyLineOfSightCheck
    private bool playerInSight;
    private Transform playerSightTarget;
   
    //States
    public float sightRange, attackRange;
    private float sightRangeWhenPlayerSneaking;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake ()
    {
        playerBehavior = GameObject.Find("Player").GetComponent<PlayerBehavior>();
       
     
      
        originalPosition = this.transform.position;
      
        //agent = GetComponent<NavMeshAgent>();
      
       
    }
    private void Start()
    {
        sightRangeWhenPlayerSneaking = sightRange / 2;
        if (enemyType == "Ant")
        ant = GetComponent<AntAttackSequence>();
        else if (enemyType == "Grasshopper") grasshopper = GetComponent<GrasshopperAttackSequence>();
        player = GameObject.Find("PlayerModel").GetComponent<Transform>();
        playerSightTarget = GameObject.Find("playerSightTarget").GetComponent<Transform>();
        if (fitter != null)
        {
            fitter = GetComponent<FGroundFitter>();
            fitter.UpAxisRotation = transform.rotation.eulerAngles.y;
        }
       
    }
   
   private void Patroling()
    {
        if (agent.enabled)
        {
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
            {
                if (fitter != null)
                {
                    if (fitter.LastRaycast.transform) transform.position = fitter.LastRaycast.point;

                    Quaternion targetRot = Quaternion.LookRotation(walkPoint - transform.position);
                    fitter.UpAxisRotation = Mathf.LerpAngle(fitter.UpAxisRotation, targetRot.eulerAngles.y, Time.deltaTime * 7f);
                }
                agent.SetDestination(walkPoint);

               headTransform.LookAt(walkPoint);
            }
            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 10f)
                walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
      //  Debug.Log("SWP");
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 20f, whatIsGround)) walkPointSet = true;
    }
    private void ChasePlayer()
    {
        if (agent.enabled)
        {

            if (!FindObjectOfType<AudioManager>().IsSoundPlaying(combatMusicTitle) && !PauseMenuFunctionality.gameIsPaused)
            {
                AudioManager.instance.StopPlaying(backGroundMusicTitle);
                AudioManager.instance.Play(combatMusicTitle);
            }
            agent.SetDestination(player.position);

            switch (enemyType)
            {
                case ("Centipede"):
            
                headTransform.LookAt(player, Vector3.up);
                if (centipedeWalkingRigLeft.weight == 0f)
                { centipedeWalkingRigLeft.weight = 1f; }
                if (centipedeWalkingRigRight.weight == 0f)
                { centipedeWalkingRigRight.weight = 1f; }
                if (centipedeSpineRig.weight == 0f)
                { centipedeSpineRig.weight = 1f; }
                if (centipedeHindLegsRig.weight == 0f)
                { centipedeHindLegsRig.weight = 1f; }
                animator.SetBool("Threaten", false);
                    break;
                default:
            { transform.LookAt(player);

                animator.SetBool("Attack", false);
            }
                    break;
        }

           


        }
    }
    private void AttackPlayer ()
    {
        if (!FindObjectOfType<AudioManager>().IsSoundPlaying(combatMusicTitle) && enemyBehavior.enemyLives > 0  && !PauseMenuFunctionality.gameIsPaused)
        {
            AudioManager.instance.StopPlaying(backGroundMusicTitle);
            AudioManager.instance.Play(combatMusicTitle);
           }
        //Make sure enemy doesn't move
        if (agent.enabled)
        {


          
            switch (enemyType)
            {
                case ("Centipede"):
                    if (!centipedeAttack.attackOccuring)
                    {
                        agent.SetDestination(transform.position);

                        animator.SetBool("Threaten", true);
                        centipedeSpineRig.weight = 0f;
                        headTransform.LookAt(player, Vector3.up);
                        centipedeWalkingRigLeft.weight = 0f;
                        centipedeWalkingRigRight.weight = 0f;
                        CentipedeAttack();

                    }
                    break;
                case ("Ant"):
                    agent.SetDestination(transform.position);
                    transform.LookAt(player);
                        AntAttack();
                    break;
                case ("Grasshopper"):
                    Debug.Log("Grasshopper Attack AI");
                    if (!grasshopper.attackOccuring)
                    {
                       
                        grasshopper.GrassHopperAttack();
                    }
                    break;
                default:
                    Debug.Log("Attack");
                    transform.LookAt(player);
                    break;
            }
       

        }
    }
    private void AntAttack()
    {
        if (!ant.attackOccuring)
        {
            StartCoroutine(ant.AntAttackAnimation());
        }
    }
   private void CentipedeAttack()
    {
        if (!centipedeAttack.attackOccuring)
        {
            centipedeWalkingRigLeft.weight = 0f;
            centipedeWalkingRigRight.weight = 0f;
            StartCoroutine(centipedeAttack.CentipedeAttackAnimation());
        }
    }
  
    // Update is called once per frame
    void Update()
    {

        LayerMask rayCastLayerMask = whatIsPlayer | whatIsGround;
        //Check for sight and attack range
       
        if (Sneaking.playerSneaking)
        { playerInSightRange = Physics.CheckSphere(transform.position, sightRangeWhenPlayerSneaking, whatIsPlayer, QueryTriggerInteraction.Ignore); }
        else playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer, QueryTriggerInteraction.Ignore);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer, QueryTriggerInteraction.Ignore);
        bool RayCastHit = Physics.Raycast(headTransform.position, playerSightTarget.position - headTransform.position, out hit,sightRange, rayCastLayerMask);
        if (RayCastHit)
        {
            if (hit.transform.tag == "Player")
            {
                playerInSight = true;
                
            }
            else
            {
               
                playerInSight = false;
            }
        }
        Debug.DrawRay(headTransform.position, playerSightTarget.position - headTransform.position, Color.yellow);
     

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange && playerInSight)
        {
         
          
            ChasePlayer();
           
        }
       if (playerInAttackRange && playerInSightRange)
        {
          
            AttackPlayer();
        
           
                }
      
       
        if (originalPosition != this.transform.position)
        {
            isMoving = true;
            animator.SetBool("IsMoving", isMoving);
            originalPosition = this.transform.position;
        }
        else if (originalPosition == this.transform.position)
        {
            isMoving = false;
            animator.SetBool("IsMoving", isMoving);
        }
        
    }
}
