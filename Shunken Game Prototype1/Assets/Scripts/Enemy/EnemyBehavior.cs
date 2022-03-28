using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyBehavior : MonoBehaviour
{
    public event EventHandler onEnemyDeath;
    public event EventHandler<OnEnemyHealthChangeEventArgs> OnEnemyHealthChange;
    public class OnEnemyHealthChangeEventArgs : EventArgs
    {
        public float enemyHealth;
    }
  //Audio
  [SerializeField] string backGroundMusicTitle = "Background Music", combatMusicTitle = "Combat Song Flute";
    // HealthBar
    public GameObject enemyHUD;
    public HealthBar healthbar;
    bool enemyDeathCompleted = false;
    //locations

    [SerializeField] string enemyType;
    //animations
    private MeleeAttacking meleeAttacking;
 
   public Animator animator;
    // private int locationIndex = 0;
    //Dissolve Death Effect
    [SerializeField] ParticleSystem dissolveParticleSystem;
    //Remove transform from list upon death
    private EnemyPresenceCheckSampleScene presenceCheck;
    //Sound
   public InsectAnimationEventHandler soundEffects;

    // Experience
    private LevelSystem levelSystem;
    [SerializeField] int experienceGiven = 10;
    //ItemDropping
    ItemDropping itemDropping;
    EnemyLockOn enemyLockOn;

    [SerializeField] int _lives = 60;
    public int enemyLives
    {
        get { return _lives; }
         set
        {
            soundEffects.DamageSoundFX();
            _lives = value;
            if (OnEnemyHealthChange!= null)
            {
                OnEnemyHealthChange(this, new OnEnemyHealthChangeEventArgs { enemyHealth = value });
            }
            if (_lives <= 0)
            {
               
                if (!enemyDeathCompleted)
                {
                    soundEffects.DeathSoundFX();
                    enemyDeathCompleted = true;
                   
                    itemDropping.RunPotionLottery();

                    ScoreBoardBehavior.enemiesKilled += 1;
                    levelSystem.AddExperience(experienceGiven);

                   switch (enemyType)
                    {
                        case ("Centipede"):

                           
                            animator.SetBool("Death", true);
                            Instantiate(dissolveParticleSystem, this.transform);
                            Destroy(this.transform.parent.gameObject, 1.7f);
                            break;

                        case ("Ant"):
                            Destroy(this.transform.parent.gameObject, 1.7f);
                            animator.SetBool("Death", true);
                            break;
                        case ("Grasshopper"):
                           
                            animator.SetBool("Death", true);
                       
                            Destroy(this.transform.parent.gameObject, 1.7f);
                            break;
                        default:
                            Destroy(this.transform.parent.gameObject, 1.7f);
                            break;
                    }
              
                }

            }
        }
    }
   void OnDestroy()
    {// presenceCheck.EnemyDeathEvent(); 

        // onEnemyDeath?.Invoke(this, EventArgs.Empty);
        if (presenceCheck.isActiveAndEnabled)
        presenceCheck.RemoveDeadEnemyFromTransformList();
     
    }
    void Start()
    {
         presenceCheck = GetComponentInParent<EnemyPresenceCheckSampleScene>();
        // _dissolve = GetComponent<DissolveEffectTrigger>();
        // TryGetComponent<InsectAnimationEventHandler>(out soundEffects);
        levelSystem = GameObject.Find("GameManager").GetComponent<LevelSystem>();
        itemDropping = GetComponent<ItemDropping>();
       
      //  itemDropping = GetComponent<ItemDropping>();
        meleeAttacking = GameObject.Find("Player").GetComponent<MeleeAttacking>();
        switch (enemyType)
            {
            case ("Centipede"):
                animator = GetComponent<Animator>();
                break;
            case ("Ant"):
                animator = GetComponentInChildren<Animator>();
                break;
            case ("Grasshopper"):
                animator = GetComponentInParent<Animator>();
                break;
        }
      
      //  healthbar.SetMaxHealth(_lives);
    }
   

    
    /*
    void OnTriggerEnter(Collider other)
    {
       
             if (other.name == "Player")
            {
            agent.destination = player.position;
            Debug.Log("Enemy detected- Attack!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        //4
        if (other.name == "Player")
        {
            Debug.Log("Player out of range; resume patrol");

            
            
        }
    }
    */
}