using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{//storing meleeattacking to play sounds
    MeleeAttacking meleeAttacking;
   
    private EnemyBehavior enemyBehavior;
    // Start is called before the first frame update
    void Start()
    {
        meleeAttacking = GameObject.Find("Player").GetComponent<MeleeAttacking>();
        if (gameObject.TryGetComponent(typeof(EnemyBehavior), out Component component))
        { enemyBehavior = GetComponent<EnemyBehavior>(); }
        else enemyBehavior = GetComponentInParent<EnemyBehavior>();
    }
    private void OnParticleCollision(GameObject part)
    {
        Debug.Log("IceLanceHIt");
        switch (part.name)
        { 
         case "IceLance(Clone)":
               
                int iceLanceDamageAmount;
                iceLanceDamageAmount = UnityEngine.Random.Range(MagicCasting.iceDamage - 5, MagicCasting.iceDamage + 5);
                bool isIceLanceCritical = UnityEngine.Random.Range(0, 100) < 10;
                if (isIceLanceCritical) iceLanceDamageAmount *= 2;


                enemyBehavior.enemyLives -= iceLanceDamageAmount;

                DamagePopup.Create(this.transform.position, iceLanceDamageAmount, isIceLanceCritical);
                break;
        case "EarthShatter(Clone)":

                int damageAmount;
             damageAmount = UnityEngine.Random.Range(MagicCasting.earthWaveDamage - 5, MagicCasting.earthWaveDamage + 5);
                bool isCritical = UnityEngine.Random.Range(0, 100) < 20;
                if (isCritical) damageAmount *= 2;

 
            enemyBehavior.enemyLives -= damageAmount;

                DamagePopup.Create(this.transform.position, damageAmount, isCritical);
            //healthbar.SetHealth(_lives);
            Debug.Log("EARTHWAVE");
            break;
            case "FlameThrower(Clone)":

                int flameThrowerDamageAmount;
                flameThrowerDamageAmount = UnityEngine.Random.Range(MagicCasting.flamethrowerDamage -1, MagicCasting.flamethrowerDamage + 1);
                bool isFlameThrowerCritical = UnityEngine.Random.Range(0, 100) < 50;
                if (isFlameThrowerCritical) flameThrowerDamageAmount *= 0;


                enemyBehavior.enemyLives -= flameThrowerDamageAmount;

                DamagePopup.Create(this.transform.position, flameThrowerDamageAmount, isFlameThrowerCritical);
                //healthbar.SetHealth(_lives);
               
                break;
        
        }

    }
    public void DamageAbsorbed(string spell)
    {
        switch (spell)
        {
            case "FireBall":

                int FireBallDamageAmount;
                FireBallDamageAmount = UnityEngine.Random.Range(MagicCasting.fireBallDamage - 5, MagicCasting.fireBallDamage + 5);
                bool isFireBallCritical = UnityEngine.Random.Range(0, 100) < 10;
                if (isFireBallCritical) FireBallDamageAmount *= 2;


                enemyBehavior.enemyLives -= FireBallDamageAmount;

                DamagePopup.Create(this.transform.position, FireBallDamageAmount, isFireBallCritical);
                break;
            case "Melee":
                //PlaySoundOnHit
                meleeAttacking.PickAndPlayRandomHitSound();

                //calculate damage

                int MeleeDamageAmount;
                MeleeDamageAmount = UnityEngine.Random.Range(MeleeAttacking.meleeDamage - 2, MeleeAttacking.meleeDamage + 2);
                bool isMeleeCritical = UnityEngine.Random.Range(0, 100) < 10;
                if (isMeleeCritical) MeleeDamageAmount *= 2;

                if (enemyBehavior != null)
                enemyBehavior.enemyLives -= MeleeDamageAmount;

                DamagePopup.Create(this.transform.position, MeleeDamageAmount, isMeleeCritical);
                break;
            case "Bomb":

                int bombDamage;
                bombDamage = UnityEngine.Random.Range(ExplosionOnContact.damage -8, ExplosionOnContact.damage * +8);
                bool isBombCritical = UnityEngine.Random.Range(0, 100) < 10;
                if (isBombCritical) bombDamage *= 2;

                if (enemyBehavior != null)
                    enemyBehavior.enemyLives -= bombDamage;

                DamagePopup.Create(this.transform.position, bombDamage, isBombCritical);
                break;
        }
    }
 
 
}
