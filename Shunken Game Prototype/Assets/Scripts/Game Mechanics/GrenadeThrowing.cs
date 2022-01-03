using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using RootMotion.Dynamics;
using UnityEngine.Animations.Rigging;
using DG.Tweening;
using  UnityEngine.SceneManagement;

public class GrenadeThrowing : MonoBehaviour
{
    Sneaking sneaking;
     AudioSource audioSource;
    [SerializeField] AudioClip throwWindSound;
    [SerializeField] GameObject reticle;
    public float throwPower = 1000f;
    public Transform releasePoint;
    public GameObject Terrain;
    private Scene parallelScene;
    private PhysicsScene parallelPhysicsScene;
    Transform playerModelTransform;
    CharacterController _controller;
    public float throwingAnimationDuration;
    [SerializeField] Transform grenadeParentTransform;
    InventorySystem inventory;
    MagicCasting magicCasting;
    public static bool holdingGrenade;
    [SerializeField] Transform rightHandTransform;
    [SerializeField] Animator playerAnimator;
    [SerializeField] GameObject bombPrefab;
    GameObject grenade;
    public CinemachineVirtualCamera aimCam;
    private bool renderTrajectoryEnabled = false;
    public TrajectoryRenderer trajectory;
   public PropMuscle propMuscleRightHand;
   public PuppetMasterProp puppetMasterProp;
    PlayerControls controls;
    [SerializeField] TwoBoneIKConstraint throwingArmRigConstraint;
    [SerializeField] Rig throwingRig;
    [SerializeField] CanvasGroup bombIcon,noSpellIcon;
    bool nothingIconOpen;
    void Awake()
    {

        controls = new PlayerControls();
        controls.Player.EquipThrowable.performed += context => Equip();
        controls.Player.MagicExecution.performed += context => AimGrenade();
        controls.Player.MagicExecution.canceled += context => ThrowGrenade();

        controls.Player.SwitchSpell.performed += context => OnSwitchSpells();


    }

    void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        sneaking = GetComponent<Sneaking>();
        audioSource = GetComponent<AudioSource>();
        propMuscleRightHand = GameObject.Find("Prop Muscle hand.R").GetComponent<PropMuscle>();
        puppetMasterProp = GetComponent<PuppetMasterProp>();
        magicCasting = GetComponent<MagicCasting>();
        inventory = GameObject.Find("GameManager").GetComponent<InventorySystem>();
      
    }

 
    public void ThrowGrenade()
    {
        if (holdingGrenade  && Time.timeScale == 1f)
        {
            if (Sneaking.playerSneaking) sneaking.Crouch();
             reticle.SetActive(false);
            playerModelTransform = GameObject.Find("PlayerModel").GetComponent<Transform>();
            playerAnimator.SetTrigger("Throw");
           
            MagicCasting.castingAimedSpells = false;
            renderTrajectoryEnabled = false;
            InventorySystem.grenadeAmount--;
          
            holdingGrenade = false;
            StartCoroutine(StandStill(throwingAnimationDuration));
        }
    }
    private void OnSwitchSpells()
    {
        if (nothingIconOpen && noSpellIcon.alpha>0)
        {
            noSpellIcon.alpha = 0f;
            nothingIconOpen = false;
        }
    }
    public IEnumerator CallLaunchMethod()
    {
        propMuscleRightHand.currentProp = null;
         yield return  new WaitForSeconds(0.1f);
        // yield return new WaitForEndOfFrame();
        FindObjectOfType<AudioManager>().Play("ThrowingSound");
        trajectory.LaunchProjectile();
    }
    public void AimGrenade()
    {
        if (holdingGrenade)
        {
            reticle.SetActive(true);
            MagicCasting.castingAimedSpells = true;
            renderTrajectoryEnabled = true;
            aimCam.Priority = 11;
        }
    }
    public void AssignGrenadeInstance()
    {
        trajectory = grenade.GetComponent<TrajectoryRenderer>();
        puppetMasterProp = grenade.GetComponent<PuppetMasterProp>();
    }
    public IEnumerator EquipGrenade()
    {
        
        playerAnimator.SetTrigger("EquipItem");
        holdingGrenade = true;
        magicCasting.StopMagic();
        if (magicCasting.spellUsed == "FireBall") magicCasting.TurnOffFireBall();
        magicCasting.spellUsed = "Nothing";
        DOTween.To(() => bombIcon.alpha, x => bombIcon.alpha = x, 1f, 0.5f);

        StartCoroutine(StandStill(0.8f));
        yield return new WaitForSeconds(0.8f);
        grenade = Instantiate(bombPrefab, rightHandTransform.position, Quaternion.identity);
        AssignGrenadeInstance();
        propMuscleRightHand.currentProp = puppetMasterProp;
        
        //DOTween.To(() => throwingRig.weight, x => throwingRig.weight = x, 1f, 0.3f);


    }
    private IEnumerator UnequipGrenade()
    {
        DOTween.To(() => bombIcon.alpha, x => bombIcon.alpha = x, 0f, 0.5f);
        DOTween.To(() => noSpellIcon.alpha, x => noSpellIcon.alpha = x, 1f, 0.5f);
        StartCoroutine(StandStill(0.5f));
        playerAnimator.SetTrigger("UnequipItem");
        GameObject currentGrenade = propMuscleRightHand.currentProp.gameObject;
        //Debug.Log(currentGrenade.name);
        //propMuscleRightHand.currentProp = null;
      //  currentGrenade.SetActive(false);
        
        reticle.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        // propMuscleRightHand.currentProp = null;
        GetRidOfModel();
        Destroy(currentGrenade);
  
        holdingGrenade = false;
        nothingIconOpen = true;
    }
    private void GetRidOfModel()
    {
        for (int i = 0; i < grenadeParentTransform.childCount; i++)
        {
            GameObject model = grenadeParentTransform.GetChild(1).gameObject;
            if (model.CompareTag("Shootable")) Destroy(model);
        }
    }
    private void Equip()
    {
        if (InventorySystem.grenadeAmount > 0 && !holdingGrenade)
        {
            StartCoroutine(EquipGrenade());
        }
        else if (holdingGrenade) StartCoroutine(UnequipGrenade());
    }

    private IEnumerator StandStill(float duration)
    {
       
        StayInPlace.stayInPlace = true;
        // _controller.Move(transform.forward);
       // transform.DOMove(FindStepDestination(), 1f);
        yield return new WaitForSeconds(duration);
   
        StayInPlace.stayInPlace = false;
    }
    private Vector3 FindStepDestination()
    {
      Vector3 localDestination =  playerModelTransform.localPosition + new Vector3(0, 0, 5f);
        Vector3 destination = transform.TransformPoint(localDestination);
        return destination;
    }

}
