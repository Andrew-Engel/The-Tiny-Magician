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

    [SerializeField] bool isCentipede;
    //animations
    private MeleeAttacking meleeAttacking;
 
    Animator animator;
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

                    animator.SetBool("Death", true);

                    levelSystem.AddExperience(experienceGiven);

                   
                    if (!isCentipede)
                    {
                      
                        //_dissolve.DeathDissolve(1.7f);
                        Destroy(this.transform.parent.gameObject, 1.7f);
                        Instantiate(dissolveParticleSystem, this.transform);

                    }
                    else
                    {
                      
                        // _dissolve.DeathDissolve(1.7f);
                        Destroy(this.transform.gameObject, 1.7f);

                    }
                }

            }
        }
    }
    void OnDestroy()
    { presenceCheck.EnemyDeathEvent(); }
    void Start()
    {
        presenceCheck = GameObject.Find("Enemies").GetComponent<EnemyPresenceCheckSampleScene>();
        // _dissolve = GetComponent<DissolveEffectTrigger>();
        levelSystem = GameObject.Find("GameManager").GetComponent<LevelSystem>();
        itemDropping = GetComponent<ItemDropping>();
        audio = GetComponent<AudioSource>();
      //  itemDropping = GetComponent<ItemDropping>();
        meleeAttacking = GameObject.Find("Player").GetComponent<MeleeAttacking>();
        animator = GetComponentInChildren<Animator>();
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