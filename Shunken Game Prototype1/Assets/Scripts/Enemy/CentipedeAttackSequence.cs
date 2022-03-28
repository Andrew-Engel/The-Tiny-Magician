using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CentipedeAttackSequence : MonoBehaviour
{
    public bool playerInRange;
 
    [SerializeField] private Animator animator;

    [SerializeField] private float attackRange, attackRate;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform attackSource;
    private Transform player;
    private PlayerBehavior playerBehavior;
    public float centipedeAttackDelay;
    public bool attackOccuring = false;
    private GameBehavior _gameManager;
    public int attackDamage = 10;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        //centipedeAttackLocation = GameObject.Find("Centipede Attack Location").GetComponent<Transform>();
        playerBehavior = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
      ///  if (attackOccuring && _gameManager.buttonTapCount > buttonTapsForEscape)
      ///  {StartCoroutine(CentipedeEscape());
      ///      Debug.Log("Escape");
     ///   }
        
    }
    public IEnumerator CentipedeAttackAnimation()
    {
        animator.SetBool("Attack", true);

        attackOccuring = true;
       
       if (Physics.CheckSphere(attackSource.position,attackRange,playerLayer))
        {
            playerInRange = true;
            _gameManager.HP -= attackDamage;
            
        }
        yield return new WaitForSeconds(attackRate);
        attackOccuring = false;

        animator.SetBool("Attack", false);
        playerInRange = false;

    }
   
}
