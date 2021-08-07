using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;
using StarterAssets;


public enum MeleeAttacks
{
   
  Jab = 0,
    Cross = 1,
    FlyingKnee =2


};
public class MeleeAttacking : MonoBehaviour
{//Sounds
    [SerializeField] AudioClip jumpAttackSound, landingSound;
    AudioSource audioSource;



    public static bool jumpAttack = false;
    public int meleeStaminaCost = 10;
    private StaminaBar stamina;
    public event EventHandler<OnMeleeAttackEventArgs> OnMeleeAttack;
    public class OnMeleeAttackEventArgs : EventArgs
    {
        public string meleeUsed;
    }

    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip[] hitSounds;
     AudioSource _audioSource;
    public  bool attacking = false;
    public static int meleeDamage = 5;
    PlayerBehavior playerBehavior;
    Animator animator;
    [SerializeField] private int attackIndex;
    [SerializeField] private float attackDuration, jumpAttackDuration, jumpLandingDuration;
    public List<string> meleesUnlocked = new List<string>();
    //InputControls
    PlayerControls controls;
    Vector2 move;
    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Melee.performed += context => Melee();
        controls.Player.Move.performed += context => move = context.ReadValue<Vector2>();

        controls.Player.Move.canceled += context => move = Vector2.zero;
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        stamina = GameObject.Find("GameManager").GetComponent<StaminaBar>();
        _audioSource = GetComponent<AudioSource>();
        //Initialize MeleesUnlocked list
        meleesUnlocked.Add(MeleeAttacks.Jab.ToString());
        meleesUnlocked.Add(MeleeAttacks.Cross.ToString());
        meleesUnlocked.Add(MeleeAttacks.FlyingKnee.ToString());
        playerBehavior = GetComponent<PlayerBehavior>();
        animator = GetComponentInChildren<Animator>();
    }
    void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
  private void Melee()
    {
        if (!attacking && stamina.stamina > meleeStaminaCost)
        StartCoroutine(MeleeCoroutine());
    }
   private IEnumerator MeleeCoroutine()
    {

        attacking = true;
        
      if (OnMeleeAttack != null)
        {
            OnMeleeAttack(this, new OnMeleeAttackEventArgs { meleeUsed = meleesUnlocked[attackIndex] });
        }
       
        _audioSource.PlayOneShot(attackSound, 1f);
        if (attackIndex < (meleesUnlocked.Count - 2))
            { attackIndex++; }
        else if (move != Vector2.zero)
        {
            attackIndex = 2;
        }
        else attackIndex = 0;
        animator.SetTrigger(meleesUnlocked[attackIndex]);
        stamina.stamina -= meleeStaminaCost;
        float duration;
        if (attackIndex == 2)
        {
            duration = jumpAttackDuration;
            jumpAttack = true;
            ThirdPersonController.Grounded = false;
            audioSource.PlayOneShot(jumpAttackSound, 1f);
            playerBehavior.JumpAudio();
        }
        else duration = attackDuration;


            yield return new WaitForSeconds(duration);
        if (attackIndex == 2)
        {
            audioSource.PlayOneShot(landingSound, 1f);
            jumpAttack = false;
            ThirdPersonController.Grounded = true;
            //PlayerBehavior.jumping = false;
            StartCoroutine( StayInPlace.StandStill(jumpLandingDuration, false));
        }

        attacking = false;
       
    }
   
    public void PickAndPlayRandomHitSound()
    {
        AudioClip clipToPlay;
        int arrayIndex;
        arrayIndex = UnityEngine.Random.Range(0, hitSounds.Length - 1);
        clipToPlay = hitSounds[arrayIndex];
        _audioSource.PlayOneShot(clipToPlay, 1f);
    }
}
