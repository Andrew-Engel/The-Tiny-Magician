using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;
using System;
using DG.Tweening;

public class TorchBehavior : MonoBehaviour
{
    [SerializeField] GameObject flamesParticlesSystem;
    AudioSource _audio;
    TorchFuelUI UI;
    [SerializeField] CanvasGroup interactPrompt;
    public static float maxFuelLevel = 20f;
    public static float torchFuelConsumptionRate = 0.1f;
    public float fuelLevel;
    private bool useTorch = false;
    Light pointLight;
    PropMuscle propMuscleLeftHand;
    PuppetMasterProp puppetMasterProp;
    Animator playerAnimator;
    bool playerNearby;
    //InputControls
    PlayerControls controls;
    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Interact.performed += context => Interact();
    }
    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        fuelLevel = maxFuelLevel;
        propMuscleLeftHand = GameObject.Find("Prop Muscle hand.L").GetComponent<PropMuscle>();
        puppetMasterProp = GetComponent<PuppetMasterProp>();
        playerAnimator = GameObject.Find("PlayerModel").GetComponent<Animator>();
        pointLight = GetComponentInChildren<Light>();
        UI = GameObject.Find("GameManager").GetComponent<TorchFuelUI>();

       
    }
    void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        if (useTorch) UseFuel();
    }
    void Interact()
    {
        if (playerNearby && propMuscleLeftHand.currentProp == null)
        {
            StartCoroutine(PickUp());
           
        }
        else if (propMuscleLeftHand.currentProp == puppetMasterProp)
        {
            StartCoroutine(DropItem());
            _audio.Stop();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            if (!useTorch)
            {
                DOTween.To(() => interactPrompt.alpha, x => interactPrompt.alpha = x, 1, 0.2f);
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            DOTween.To(() => interactPrompt.alpha, x => interactPrompt.alpha = x, 0, 0.2f);
        }
    }
    private IEnumerator PickUp()
    {
        flamesParticlesSystem.SetActive(true);
        playerAnimator.SetTrigger("ItemPickUp");
        yield return new WaitForSeconds(0.8f);
        propMuscleLeftHand.currentProp = puppetMasterProp;
        
        pointLight.enabled = true;
        useTorch = true;
        DOTween.To(() => interactPrompt.alpha, x => interactPrompt.alpha = x, 0, 0.2f);

        Invoke("PlaySound", 0.2f);

    }
    private IEnumerator DropItem()
    {
        flamesParticlesSystem.SetActive(false);
        playerAnimator.SetTrigger("ItemDrop");
        yield return new WaitForSeconds(0.5f);
        propMuscleLeftHand.currentProp = null;
        pointLight.enabled = false;
        useTorch = false;
      
        UI.TurnOffMeter();
    }
    private void UseFuel()
    {
        if (fuelLevel > 0)
        {
            fuelLevel -= torchFuelConsumptionRate * Time.deltaTime;

            float normalizedFuelLevelToBeSentOut = fuelLevel / maxFuelLevel;
            UI.UpdateFuelMeter(normalizedFuelLevelToBeSentOut);

        }
        else
        {
           _audio.Stop();
            UI.TurnOffMeter();
            Debug.Log("OutOfFuel");
            StartCoroutine(DropItem());
            Destroy(this.gameObject,0.5f);
            
        }
    }
   void PlaySound()
    {
        _audio.Play();
    }
}