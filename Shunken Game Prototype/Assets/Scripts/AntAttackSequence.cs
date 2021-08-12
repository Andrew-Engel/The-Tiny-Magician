using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntAttackSequence : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private float attackRange, attackRate;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform attackSource;
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

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator AntAttackAnimation()
    {
        animator.SetBool("Attack", true);

        attackOccuring = true;
        
        if (Physics.CheckSphere(attackSource.position, attackRange, playerLayer, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("OUCH AN ANT");
            _gameManager.HP -= attackDamage;

        }
        yield return new WaitForSeconds(attackRate);
        attackOccuring = false;

        animator.SetBool("Attack", false);
    }
}
