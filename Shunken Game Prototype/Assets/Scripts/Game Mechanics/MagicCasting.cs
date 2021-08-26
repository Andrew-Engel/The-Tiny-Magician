using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Animations.Rigging;
using StarterAssets;
using UnityEngine.UI;
using DG.Tweening;
using RootMotion.Dynamics;

public class MagicCasting : MonoBehaviour
{
    public PuppetMaster puppet;
    //eventsystems
    public event EventHandler<OnSpellChangeEventArgs> OnSpellChange;
    public class OnSpellChangeEventArgs : EventArgs
    {
        public string currentSpell;
        public string nextSpell;
    }
    public event EventHandler<OnSpellUseAndCancelEventArgs> OnSpellUseAndCancel;
    public class OnSpellUseAndCancelEventArgs : EventArgs
    {
        public string currentSpell;
        
    }
    //Other Spells
    AirPathSpells airPathSpells;
    FirePathSpells firePathSpells;
    //Mana Bar System
    private ManaBarSystem manaBarSystem;
    // Equipped Spells
    public static List<string> spellsEquipped = new List<string>();
    //enemy reticle
    [SerializeField] GameObject aimingReticle;
    [SerializeField] Image aimingReticleImageComponent;
    [SerializeField] Color enemyTargetedReticleColor, normalReticleColor;
    //DamageStats
    public static int iceDamage = 5;
    public static int earthWaveDamage = 15;
    public static int flamethrowerDamage = 1;
    public static int fireBallDamage = 8;
    //ManaConsumption Stats
    Dictionary<string, int> spellManaCost = new Dictionary<string, int>
    {
        {Spells.FireBall.ToString(),10 },
        {Spells.IceShards.ToString(),5 },
        {Spells.FlameThrower.ToString(),3 },
        {Spells.EarthWave.ToString(),20 },
        {Spells.AirEscape.ToString(),10 }
    };
    //preventing walking backwards and spells while not strafing
    private EnemyLockOn enemyLockOn;
    private Vector2 move;
    //preventing switching spell when spell in progress
    public static bool spellInProgress = false;
   public static bool casting = false;
    //other variables
    [SerializeField] GameObject castingEffect;
    
    [SerializeField] float magicAnimationDuration;
    private ThirdPersonController thirdPersonController;
    //spell effect origins
    public Transform earthWavePoint, flameThrowerPoint, castingPoint;
    //Animation and Camera variables
    public Animator animator;
    public Rig aim;
    public Rig throwingRig;
    [SerializeField] TwoBoneIKConstraint throwingArmRig;
    [SerializeField] MultiAimConstraint throwingChestRig;
    public static bool castingAimedSpells;
    public CinemachineVirtualCamera aimCam;
    //Choosing Spells
    public string spellUsed = "Nothing";
    public int spellIndex;
    //Targets
    private Vector3 castingTarget;
    private Transform castingTargetBackup;
    [SerializeField] Transform aimRayOrigin;
    [SerializeField] Transform castingRunningPoint, castingSneakPoint;
    [SerializeField] Transform aimCamTransform, mainCamTransform, spellsCast;
    [SerializeField] Vector3 flameThrowerOffset;
    //Spells
    public ParticleSystem iceShards;
    public ParticleSystem earthWave;
    public ParticleSystem flameThrower;
        //fireball
    public GameObject fireBall, fireBallInHand;
    [SerializeField] float fireBallDelay;
    [SerializeField] float fireBallVelocity;
    [SerializeField] Transform fireBallSpawnPoint;
    // sneak system, to turn off sneak during certain spells
    Sneaking sneakingSystem;
    //SkillTree
    private SkillTreeSystem skillTree;
    //Sounds
    public AudioClip earthWaveRocks, earthWaveFire, iceLanceSound,fireBallSound,flameThrowerSound;
    [SerializeField] float iceLanceTempo;
   new  AudioSource audio;
    //InputControls
    PlayerControls controls;
    private PlayerBehavior player;
    void Awake()
    {

        controls = new PlayerControls();
        controls.Player.MagicExecution.performed += context => CastMagic();
     
        controls.Player.MagicExecution.canceled += context => StopMagic();

        controls.Player.SwitchSpell.performed += context => SwitchSpells();


        controls.Player.Move.performed += context => move = context.ReadValue<Vector2>();
        controls.Player.Move.canceled += context => move = Vector2.zero;
    }
    private void SwitchSpells()
    {
        if (!spellInProgress)
        {
           
           /* if (spellUsed == "FireBall")
            {
                puppet.mode = PuppetMaster.Mode.Disabled;
                DOTween.To(() => throwingRig.weight, x => throwingRig.weight = x, 0f, 0.3f);
                fireBallInHand.SetActive(false);
                puppet.mode = PuppetMaster.Mode.Active;

            }*/

            spellIndex++;
            string nextSpell;
            
            if (spellIndex > (SpellEquipping.spellsEquiped.Length - 1))
            {
                spellIndex = 0;
                nextSpell = SpellEquipping.spellsEquiped[spellIndex + 1];
                spellUsed = SpellEquipping.spellsEquiped[spellIndex];
                if (OnSpellChange != null)
                    OnSpellChange(this, new OnSpellChangeEventArgs { currentSpell = spellUsed, nextSpell = nextSpell });
            }
            else
            {
                spellUsed = SpellEquipping.spellsEquiped[spellIndex];
                if (spellIndex >= (SpellEquipping.spellsEquiped.Length - 1))
                {
                    nextSpell = SpellEquipping.spellsEquiped[0];
                  
                }
                else nextSpell = SpellEquipping.spellsEquiped[spellIndex + 1];
                if (OnSpellChange != null)
                    OnSpellChange(this, new OnSpellChangeEventArgs { currentSpell = spellUsed, nextSpell = nextSpell });
                if (spellUsed == "FireBall")
                {
                    puppet.mode = PuppetMaster.Mode.Disabled;
                    DOTween.To(() => throwingRig.weight, x => throwingRig.weight = x, 1f, 0.3f);
                    Debug.Log("throwing rig set to 1");
                    puppet.mode = PuppetMaster.Mode.Active;
                }
            }
           
          

        }
    }
    // set up starting values and variables
    void Start()
    {
        airPathSpells = GetComponent<AirPathSpells>();
        firePathSpells = GetComponent<FirePathSpells>();
        sneakingSystem = GetComponent<Sneaking>();
        manaBarSystem = GameObject.Find("GameManager").GetComponent<ManaBarSystem>();
        skillTree = GameObject.Find("GameManager").GetComponent<SkillTreeSystem>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        enemyLockOn = GetComponent<EnemyLockOn>();
        FindCastTarget();
        player = GetComponent<PlayerBehavior>();
        
        audio = GetComponent<AudioSource>();
        aim.weight = 0f;
        castingTargetBackup = GameObject.Find("castingTargetBackup").GetComponent<Transform>();
    }
    void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    private void IceLanceAudio()
    {
        audio.PlayOneShot(iceLanceSound);

    }
    private void IceLance()
    {
        spellInProgress = true;
        Vector3 IceLanceOrigin;
        if (move.y > 0)
        {
            IceLanceOrigin = castingRunningPoint.position;
        }
        else if (Sneaking.playerSneaking)
        {
            IceLanceOrigin = castingSneakPoint.position;
        }
        else
        { IceLanceOrigin = castingPoint.position; }
        Instantiate(iceShards, IceLanceOrigin, castingPoint.rotation);
        TriggerOnSpellUseEvent();
        spellInProgress = false;
    }

    private void FindCastTarget()
    {
        RaycastHit hit;
       
        bool rayCastHasContact = Physics.Raycast(aimRayOrigin.position, aimRayOrigin.forward, out hit, Mathf.Infinity);
        if (rayCastHasContact)
        { 
         castingTarget = hit.point;
          
                if (hit.transform.tag == "Enemy")
                {
                    aimingReticleImageComponent.color = enemyTargetedReticleColor;
                }
            

            else if (enemyLockOn.lockedOnEnemy)
            {
                aimingReticleImageComponent.color = enemyTargetedReticleColor;
            }
            else
            {
                aimingReticleImageComponent.color = normalReticleColor;
            }
         }
         else
        {
            castingTarget = castingTargetBackup.position;
        }
        Debug.DrawRay(castingPoint.position, castingTarget- castingPoint.position, Color.red);
    }
    private IEnumerator StartMagic(string spell)
    {//Use this for spells that use animation rigging, it seems puppet master must be disabled before a couple seconds the spells is made for animation rigging to work
        
        puppet.mode = PuppetMaster.Mode.Disabled;
       // float regularTime = 0.1f;
    //    float extendedTime = 0.5f;
        yield return new WaitForSeconds(0.1f);
       switch (spell)
        {
            case "Fireball":
                firePathSpells.Fireball();
                break;
            case "IceShards":
                StartIceLance();
                break;
            case "FlameThrower":
                FlameThrower();
                break;
        }
    }
    private void CastMagic()
    {
        if (Time.timeScale > 0)
        {
          
            //the top if statement ensures that you can't shoot spells while sprinting towards camera, for this create a buggy feeling
            if ((enemyLockOn.lockedOnEnemy) || (move.y >= 0 && !enemyLockOn.lockedOnEnemy))
            {
               
                switch (spellUsed)
                {
                    case "IceShards":
                        if (manaBarSystem.mana >= spellManaCost["IceShards"])
                        {
                           StartCoroutine( StartMagic("IceShards"));

                        }


                        break;
                    case "EarthWave":
                        if (ThirdPersonController.Grounded && manaBarSystem.mana >= spellManaCost["EarthWave"])
                        {
                            EarthWave();

                        }
                        break;
                    case "FireBall":
                        if (manaBarSystem.mana >= spellManaCost["FireBall"])
                        {
                           StartCoroutine( StartMagic("Fireball"));

                        }
                        break;
                    case "FlameThrower":

                        if (manaBarSystem.mana >= spellManaCost["FlameThrower"])
                        {
                            StartCoroutine(StartMagic("FlameThrower"));
                        }
                        break;
                    case "AirEscape":
                         if (manaBarSystem.mana >= spellManaCost["AirEscape"])
                        {
                            casting = true;
                            if (Sneaking.playerSneaking) sneakingSystem.Crouch();
                           StartCoroutine( airPathSpells.AirEscapeBackwards());
                        }
                        break;
                }
            }
        }
    }
    private IEnumerator StandStill()
    {
        StayInPlace.stayInPlace = true;
        yield return new WaitForSeconds(magicAnimationDuration);
        animator.SetLayerWeight(1, 0f);
        StayInPlace.stayInPlace = false;
    }
    public void StopMagic()
    {
        
        audio.Stop();
        aimingReticle.SetActive(false);
        CancelInvoke("IceLanceAudio");
        CancelInvoke("IceLance");
        CancelInvoke("TriggerOnSpellUseEvent");

        aim.weight = 0f;
        castingAimedSpells = false;
        aimCam.Priority = 9;
        if (spellUsed == "IceShards" || spellUsed == "FlameThrower")
        {
            if (castingPoint.childCount != 0)
            {
                castingEffect.SetActive(false);
                //animator.SetLayerWeight(1, 0f);
                animator.SetBool("OneHandSpell", false);
               Destroy(castingPoint.transform.GetChild(0).gameObject);
            }
        }
        if (spellUsed == "EarthWave")
        {
            animator.SetLayerWeight(2, 0f);
            animator.SetBool("EarthWave", false);
            if (castingPoint.transform.childCount != 0)
            Destroy(castingPoint.transform.GetChild(0).gameObject, 3f);
        }
        ResetPuppetMode();
        casting = false;
    }
    public IEnumerator ThrowFireBall()
    {
        MagicCasting.spellInProgress = true;
        if (OnSpellUseAndCancel != null)
        { OnSpellUseAndCancel(this, new OnSpellUseAndCancelEventArgs { currentSpell = spellUsed }); }
        DOTween.To(() => throwingChestRig.weight, x => throwingChestRig.weight = x, 0f, 0.3f);
        // DOTween.To(() => throwingArmRig.weight, x => throwingArmRig.weight = x, 1, 0.8f);
        throwingArmRig.weight = 1;
        yield return new WaitForSeconds(fireBallDelay);
        TriggerOnSpellUseEvent();
        audio.PlayOneShot(fireBallSound, 0.7f);
        //Determine player state, and decide which rpeset point fireball should be isntantiated in
        Transform currentCastingPoint;
        if (Sneaking.playerSneaking) currentCastingPoint = castingSneakPoint;
        else currentCastingPoint = castingPoint;

        GameObject fireBallClone = Instantiate(fireBall, currentCastingPoint.position, Quaternion.identity);
        Rigidbody fireBall_rb = fireBallClone.GetComponent<Rigidbody>();
        fireBall_rb.AddForce(currentCastingPoint.forward * fireBallVelocity);
        if (OnSpellUseAndCancel != null)
        { OnSpellUseAndCancel(this, new OnSpellUseAndCancelEventArgs { currentSpell = spellUsed }); }
        if (!enemyLockOn.lockedOnEnemy && !Sneaking.playerSneaking)
            DOTween.To(() => throwingChestRig.weight, x => throwingChestRig.weight = x, 1, 0.2f);
        // DOTween.To(() => throwingArmRig.weight, x => throwingArmRig.weight = x, 0, 0.8f);
        throwingArmRig.weight = 0;
        spellInProgress = false;
    }
    private void FlameThrower()
    {
        casting = true;
        if (Sneaking.playerSneaking)
            sneakingSystem.Crouch();
        aim.weight = 1f;
        castingAimedSpells = true;
        aimCam.Priority = 11;
        if (castingPoint.childCount == 0)
        {
            castingEffect.SetActive(true);
            Instantiate(flameThrower, flameThrowerPoint.position + flameThrowerOffset, castingPoint.rotation, castingPoint);
            audio.PlayOneShot(flameThrowerSound, 1f);
            InvokeRepeating("TriggerOnSpellUseEvent", 0, 0.5f);
        }
    }
    private void EarthWave()
    {
        casting = true;
        if (Sneaking.playerSneaking)
            sneakingSystem.Crouch();
        TriggerOnSpellUseEvent();
        StartCoroutine(StandStill());
        animator.SetLayerWeight(1, 1f);
        animator.SetBool("EarthWave", true);
        audio.PlayOneShot(earthWaveFire, 1f);
        audio.PlayOneShot(earthWaveRocks, 1f);

        // aim.weight = 1f;
        castingAimedSpells = true;
        aimCam.Priority = 11;

        Instantiate(earthWave, earthWavePoint.position, earthWavePoint.rotation);


    }
    private void StartIceLance()
    {
        casting = true;
        aimingReticle.SetActive(true);

        // animator.SetLayerWeight(1, 1f);

        aim.weight = 1f;
        castingAimedSpells = true;
        aimCam.Priority = 11;


        castingEffect.SetActive(true);
        InvokeRepeating("IceLance", 0, iceLanceTempo);
        InvokeRepeating("IceLanceAudio", 0, iceLanceTempo);
        // iceShards.transform.position = castingPoint.transform.position;
    }
    void ResetPuppetMode()
    {
        if (casting)
        {
            Debug.Log("Mode reset");
            puppet.mode = PuppetMaster.Mode.Active;
        }
    }
    private IEnumerator ResetAimCamAndMagicWithDelay(float delay)
    {
       // aimCamTransform.localEulerAngles = new Vector3 (0,180,0);
        aimCam.Priority = 9;
        aim.weight = 0f;
        yield return new WaitForSeconds(delay);
        aimCamTransform.localEulerAngles = Vector3.zero;
       // Debug.Log("Camera Angles: " + aimCamTransform.eulerAngles);
        StopMagic();
       
    }
    // Update is called once per frame
    void Update()
    {
        castingPoint.LookAt(castingTarget);
        if (castingAimedSpells)
        {
            
            FindCastTarget();
        }
        if (move.y <0 && castingAimedSpells && !enemyLockOn.lockedOnEnemy)
        {
            StartCoroutine(ResetAimCamAndMagicWithDelay(0.48f));
            
        }
    }
    private void TriggerOnSpellUseEvent()
    {
        //Debug.Log($"Mana: " + manaBarSystem.mana);
        manaBarSystem.mana -= (spellManaCost[spellUsed]);
        if ( manaBarSystem.mana == 0) StopMagic();
    }
}
