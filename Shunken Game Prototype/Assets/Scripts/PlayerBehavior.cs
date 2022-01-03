using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class PlayerBehavior : MonoBehaviour
{
   
    //Particle Effects
    [SerializeField] ParticleSystem levelUpParticleSystem;
    //Sounds
    public AudioClip jump,jumpPlayerSound, jumpLanding, levelUpSound;
   new AudioSource audio;
    public float walkingSoundTempo;
    [SerializeField] bool walkingSoundPlaying = false;
    //Turning Check
    [SerializeField] float turnAnimationThreshold;
    Vector2 look;

    //InputControls
    PlayerControls controls;
    Vector2 move;
    //MovementStats
    public float moveSpeed = 10f;
    public float rotateSpeed = 120f;
    public float strafeSpeed = 5f;
    public float jumpTime;
    public float disanceToGround = 0.1f;
    public LayerMask groundLayer;
    public float jumpVelocity = 5f;

    //otherscripts
    StaminaBar stamina;
    MagicCasting magic;
    [SerializeField] ThirdPersonController thirdPersonController;
    //Animation
    [SerializeField] Animator animator;
    public static bool jumping;

  
    // movement checks
    public bool isMoving;
    public bool stayInPlace = false;

    


    //endgame lock
    bool gameRunning;

    public bool jumpInput;

    public bool enemyMeleeEnabled;

    private Rigidbody _rb;
    private GameBehavior _gameManager;
    private CapsuleCollider _col;
    // Start is called before the first frame update
    void Awake()
    {
        levelSystemAnimated.OnLevelChanged += LevelSystem_OnLevelChanged;

        controls = new PlayerControls();

        controls.Player.Move.performed += context => move = context.ReadValue<Vector2>();
         
        controls.Player.Move.canceled += context => move = Vector2.zero;
        controls.Player.Jump.performed += context => JumpAudio();
      //  controls.Player.Look.performed += context => look = context.ReadValue<Vector2>();
       // controls.Player.Look.canceled += context => look = Vector2.zero;

        controls.Player.Interact.performed += context => Interact();
      //  stamina = GameObject.Find("GameManager").GetComponent<StaminaBar>();
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    void Start()
    {
        audio = GetComponent<AudioSource>();
        jumping = false;
    
        magic = GetComponent<MagicCasting>();

      

        _gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();

       
    }
    public void Interact()
    {
        if (stayInPlace)
        {
            _gameManager.buttonTapCount++;
            Debug.LogFormat("Button Taps: {0}", _gameManager.buttonTapCount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (move != Vector2.zero && ThirdPersonController.Grounded&& ! MeleeAttacking.jumpAttack && !PauseMenuFunctionality.gameIsPaused)
        {
            animator.SetFloat("MoveY", move.y);
            animator.SetFloat("MoveX", move.x);
          
        }
       
        if (ThirdPersonController.Grounded && jumping && !MeleeAttacking.jumpAttack)
        {
          //  Debug.Log("Landing");

            jumping = false;
            audio.PlayOneShot(jumpLanding, 1.0f);
        }
        else if (MeleeAttacking.jumpAttack && jumping)
        {
              Debug.Log("Landing");

            jumping = false;
            audio.PlayOneShot(jumpLanding, 1.0f);
        }
    }
 
  


public void JumpAudio()
{

    if (ThirdPersonController.Grounded && !PauseMenuFunctionality.gameIsPaused)
    {

        audio.PlayOneShot(jump, 1.0f);
            audio.PlayOneShot(jumpPlayerSound, 1.0f);

            Invoke("MidAirJump", jumpTime);

    }
}

private void MidAirJump()
{
    jumping = true;
   
}

   
    void OnCollisionEnter(Collision collision)
        {
            if (enemyMeleeEnabled)
            {
                if (collision.gameObject.name == "Enemy" || collision.gameObject.name == "Bullet(Clone)")
                {
                    _gameManager.HP -= 1;
                  
                }
            }
        }
    //Level System
    [SerializeField] private LevelSystemAnimated levelSystemAnimated;
    //call Set LevelSsystem in in script with the instantiated level system class (the one with " LevelSystem levelSystem = new LevelSystem();' in Awake
    public void SetLevelSystemAnimated(LevelSystemAnimated levelSystemAnimated)
    {
        this.levelSystemAnimated = levelSystemAnimated;

        levelSystemAnimated.OnLevelChanged += LevelSystem_OnLevelChanged;
    }
    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
    {//Put in level change effects here ie particle efffect or aniamtion or sound etc. can also put in code to increase stats
        Debug.Log("Player levelChanged to" + levelSystemAnimated.levelPublic);
        Instantiate(levelUpParticleSystem, this.transform.position, Quaternion.identity);
        audio.PlayOneShot(levelUpSound, 1f);
    }

}
