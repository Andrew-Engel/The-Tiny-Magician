using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AntAttackSequence : MonoBehaviour
{
    
    [SerializeField] private Animator animator;
   public bool playerInRange;
    [SerializeField] private float attackRange, attackRate;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform attackSource;
   // [SerializeField] Transform AttackPushCollider;
    //[SerializeField] Rigidbody AttackPushRigidBody;
    //public float explosionForce, explostionRadius;
    private Transform player;
    private PlayerBehavior playerBehavior;
    public bool attackOccuring = false;
    private GameBehavior _gameManager;
    public int attackDamage = 6;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
       
        playerBehavior = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
    }

    
    public IEnumerator AntAttackAnimation()
    {
        animator.SetBool("Attack", true);

        attackOccuring = true;
        
        if (Physics.CheckSphere(attackSource.position, attackRange, playerLayer, QueryTriggerInteraction.Ignore))
        {
            
            _gameManager.HP -= attackDamage;
            playerInRange = true;
        }
       // AttackPushCollider.DOLocalMoveY(0.007244f, 0.2f);
        //AttackPushRigidBody.AddExplosionForce(explosionForce, AttackPushCollider.position, explostionRadius);
        yield return new WaitForSeconds(attackRate);
        attackOccuring = false;
       // AttackPushCollider.DOLocalMoveY(0.0024f, 0.05f);
        animator.SetBool("Attack", false);
        playerInRange = false;
    }
   
}
