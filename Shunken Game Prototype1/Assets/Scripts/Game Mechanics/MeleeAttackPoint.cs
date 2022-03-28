using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeleeAttackPoint : MonoBehaviour
{
    Cinemachine.CinemachineImpulseSource impulseSource;
    [SerializeField] float hitSoundDelay;
    [SerializeField] LayerMask enemyLayerMask;
    private MeleeAttacking meleeAttackSystem;
    private Collider hitTrigger;
    [SerializeField] float attackRange;
    [SerializeField] bool isLeftHand;
    public bool attacking;
    // Start is called before the first frame update
    void Start()
    {
        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        meleeAttackSystem = GetComponentInParent<MeleeAttacking>();
        hitTrigger = GetComponent<Collider>();
        meleeAttackSystem.OnMeleeAttack += OnMeleeAttack;
        
    }

  
    private void IsEnemyInRange()
    {
     
        int maxColliders = 2;
     Collider[] hitColliders = new Collider[maxColliders];

    int numColliders = Physics.OverlapSphereNonAlloc(this.transform.position, attackRange, hitColliders, enemyLayerMask);
        for (int i = 0; i < numColliders; i++)
        {
         

            if( hitColliders[i].gameObject.TryGetComponent( out  EnemyHitBox hitBox))
            {
                hitBox.DamageAbsorbed("Melee");
                impulseSource.GenerateImpulse(Camera.main.transform.forward);
            }

        }

    }
    private void OnMeleeAttack(object sender, MeleeAttacking.OnMeleeAttackEventArgs e)
    {
        Debug.Log("OnMeleeAttack");
        if (isLeftHand && e.meleeUsed == "Jab")
        {
            attacking = true;
            Invoke("IsEnemyInRange", hitSoundDelay);

        }
        else if (!isLeftHand && e.meleeUsed == "Cross")
        {
            attacking = true;
           Invoke("IsEnemyInRange", hitSoundDelay);
        }
        else if (!isLeftHand && e.meleeUsed == "FlyingKnee")
        {
            attacking = true;
            Invoke("IsEnemyInRange", hitSoundDelay);
        }
    }
    public void HitSound()
    {
        meleeAttackSystem.PickAndPlayRandomHitSound();
    }
}
