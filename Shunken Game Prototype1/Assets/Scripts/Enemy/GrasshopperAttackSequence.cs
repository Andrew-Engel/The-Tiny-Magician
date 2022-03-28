using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class GrasshopperAttackSequence : MonoBehaviour
{
    //Used as bool for sound and camera effects in other scripts
    public bool playerWithinAttackRange;

    public bool playerWithinMinRange;
    public bool playerWithinMaxRange;
    Vector3 targetPosition;
    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;
   public bool Grounded;
    bool jumping;
    [SerializeField] AudioClip wingFlappingSound, landingSound;
    private AudioSource _audio;
    [SerializeField] ParticleSystem landingEffect;
  
    NavMeshAgent agent;
    public int attackDamage;
    [SerializeField] float attackRate;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float leapDistance = 25f;
    [SerializeField] float closeAttackRange;
    public Transform playerTransform;
    Transform grasshopperLandingTransform;
    [SerializeField] Transform apexTransform;
    [SerializeField] float leapHeight;
    [SerializeField] Transform attackSource;
    [SerializeField] float leapDuration = 2f, closeRangeAttackDuration;
    [SerializeField] Animator anim;
    public bool attackOccuring = false;
    GameBehavior _gameManager;
    // Impulse Sources
    Cinemachine.CinemachineImpulseSource impulseSource;
    private void Start()
    {
        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        _audio = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        grasshopperLandingTransform = GameObject.Find("GrasshopperLandingPoint").GetComponent<Transform>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();

    }
    public void GrassHopperAttack()
    {
        Debug.Log("Attack Grasshopper");
        
        GroundedCheck();
           //playerWithinMaxRange =  Physics.Raycast(this.transform.position, playerTransform.position- this.transform.position, leapDistance,  playerLayer);
         // playerWithinMinRange = Physics.Raycast(this.transform.position, playerTransform.position - this.transform.position, leapDistance - 10f, playerLayer);
          playerWithinMaxRange =  Physics.CheckSphere(this.transform.position,  leapDistance,  playerLayer, QueryTriggerInteraction.Ignore);
        playerWithinMinRange = Physics.CheckSphere(this.transform.position, leapDistance -10f, playerLayer, QueryTriggerInteraction.Ignore); ;
       
        if (playerWithinMaxRange && !playerWithinMinRange && Grounded)
        {
            Debug.Log("Leap!!");
            StartCoroutine(GrasshopperLeap());

        }
        else if (playerWithinMaxRange && playerWithinMaxRange)
        {
            Debug.Log("CloseAttack");
            StartCoroutine(GrasshopperCloseAttack());

        }
    }
  public IEnumerator GrasshopperLeap()
    {
        jumping = true;
        agent.enabled = false;
        InvokeRepeating("GrasshopperLeapTrajectory", 0.3f, 0.8f);
        InvokeRepeating("PlayFlapSound", 0f, 0.8f);
        attackOccuring = true;
        anim.SetBool("Leap",true);
        targetPosition = playerTransform.position;
        transform.LookAt(targetPosition);
      
        GrasshopperLeapTrajectory();
        yield return new WaitForSeconds(leapDuration);
        LandingFX();
        agent.enabled = true;
        anim.SetBool("Leap", false);
        CancelInvoke();
        attackOccuring = false;
    }
    public IEnumerator GrasshopperCloseAttack()
    {
        
        agent.SetDestination(playerTransform.position);
        
        if (Physics.CheckSphere(attackSource.position, closeAttackRange, playerLayer, QueryTriggerInteraction.Ignore))
        {
            playerWithinAttackRange = true;
            agent.SetDestination(this.transform.position);
            attackOccuring = true;
            anim.SetBool("Attack", true);
            _gameManager.HP -= attackDamage;
            yield return new WaitForSeconds(attackRate);
            anim.SetBool("Attack", false);
            attackOccuring = false;
            playerWithinAttackRange = false;
        }

        else playerWithinAttackRange = false;
        yield break;

    }
    
    private void GrasshopperLeapTrajectory()
    {
        Vector3 targetLanding = playerTransform.position + new Vector3(0, 5, 0);

        transform.DOJump(targetLanding, leapHeight, 1, leapDuration, false);
    }
    private void PlayFlapSound()
    {
        _audio.PlayOneShot(wingFlappingSound, 2f);
    }
    void OnTriggerEnter (Collider other)
    {
       
        if (other.CompareTag("Landable") && jumping)
        {
            impulseSource.GenerateImpulse(Camera.main.transform.forward);
            Debug.Log("GrasshopperLanded");
            jumping = false;
            Instantiate(landingEffect, this.transform.position + new Vector3(0, -1, 0), Quaternion.identity);
            _audio.PlayOneShot(landingSound, 0.7f);
        }
    }
    void LandingFX()
    {
        impulseSource.GenerateImpulse(Camera.main.transform.forward);
        Debug.Log("GrasshopperLanded");
        jumping = false;
        Instantiate(landingEffect, this.transform.position + new Vector3(0, -1, 0), Quaternion.identity);
        _audio.PlayOneShot(landingSound, 0.7f);
    }
    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);

        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

    
    }

}
