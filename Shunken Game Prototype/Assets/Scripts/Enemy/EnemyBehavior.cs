using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyBehavior : MonoBehaviour
{//Audio
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
    [SerializeField] AudioClip damageSound;
   new AudioSource audio;

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
            if (!audio.isPlaying)
            audio.PlayOneShot(damageSound, 1f);
            _lives = value;
            if (_lives <= 0)
            {
               
                if (!enemyDeathCompleted)
                {
                  
                    enemyDeathCompleted = true;
                    Debug.Log("EnemyDEath");
                    itemDropping.RunPotionLottery();

                   

                    levelSystem.AddExperience(experienceGiven);

                   switch (enemyType)
                    {
                        case ("Centipede"):
                            Destroy(this.transform.parent.gameObject, 1.7f);
                            animator.SetBool("Death", true);
                            Instantiate(dissolveParticleSystem, this.transform);
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
    }
    void Start()
    {
       // presenceCheck = GameObject.Find("Enemies").GetComponent<EnemyPresenceCheckSampleScene>();
        // _dissolve = GetComponent<DissolveEffectTrigger>();
        levelSystem = GameObject.Find("GameManager").GetComponent<LevelSystem>();
        itemDropping = GetComponent<ItemDropping>();
        audio = GetComponent<AudioSource>();
      //  itemDropping = GetComponent<ItemDropping>();
        meleeAttacking = GameObject.Find("Player").GetComponent<MeleeAttacking>();
        switch (enemyType)
            {
            case ("Centipede"):
            case ("Ant"):
                animator = GetComponentInChildren<Animator>();
                break;
            case ("Grasshopper"):
                animator = GetComponentInParent<Animator>();
                break;
        }
      
      //  healthbar.SetMaxHealth(_lives);
    }
   


    
   
   
   
    
    
    void Update()
    {
 
        
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