using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class GrasshopperAttackSequence : MonoBehaviour
{
    bool jumping;
    [SerializeField] AudioClip wingFlappingSound;
    private AudioSource _audio;
    [SerializeField] ParticleSystem landingEffect;
    bool leaping;
    NavMeshAgent agent;
    public int attackDamage;
    [SerializeField] float attackRate;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float leapDistance = 25f;
    [SerializeField] float closeAttackRange;
    Transform playerTransform;
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
       
      bool playerWithinMaxRange =  Physics.Raycast(this.transform.position, playerTransform.position- this.transform.position, leapDistance,  playerLayer);
        bool playerWithinMinRange = Physics.Raycast(this.transform.position, playerTransform.position - this.transform.position, leapDistance - 8f, playerLayer);
        if (playerWithinMaxRange && !playerWithinMinRange) StartCoroutine(GrasshopperLeap());
        else if (playerWithinMaxRange && playerWithinMaxRange) StartCoroutine(GrasshopperCloseAttack());
    }
  public IEnumerator GrasshopperLeap()
    {
        jumping = true;
        InvokeRepeating("GrasshopperLeapTrajectory", 0.3f, 0.8f);
        attackOccuring = true;
        anim.SetBool("Leap",true);
        transform.LookAt(playerTransform.position);
        GrasshopperLeapTrajectory();
        yield return new WaitForSeconds(leapDuration);
     
        anim.SetBool("Leap", false);
        CancelInvoke();
        attackOccuring = false;
    }
    public IEnumerator GrasshopperCloseAttack()
    {
        agent.SetDestination(playerTransform.position);

        if (Physics.CheckSphere(attackSource.position, closeAttackRange, playerLayer, QueryTriggerInteraction.Ignore))
        {
            agent.SetDestination(this.transform.position);
            attackOccuring = true;
            anim.SetBool("Attack", true);
            _gameManager.HP -= attackDamage;
            yield return new WaitForSeconds(attackRate);
            anim.SetBool("Attack", false);
            attackOccuring = false;

        }

        else  yield break;

    }
    
    private void GrasshopperLeapTrajectory()
    { 
     

        transform.DOJump(playerTransform.position + new Vector3 (0,2,0), leapHeight, 1, leapDuration, false);
    }
    private void PlayFlapSound()
    {
        _audio.PlayOneShot(wingFlappingSound, 1f);
    }
    void OnTriggerEnter (Collider other)
    {
       
        if (other.tag == "Landable" && jumping)
        {
            impulseSource.GenerateImpulse(Camera.main.transform.forward);
            Debug.Log("GrasshopperLanded");
            jumping = false;
            Instantiate(landingEffect, this.transform.position + new Vector3(0, -1, 0), Quaternion.identity);
        }
    }
}
