using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CampingBaseBehavior : MonoBehaviour
{
    StaminaBar staminaBar;
    ManaBarSystem manaBarSystem;
    GameBehavior gameBehavior;
    bool playerNearby, modalWindowOpen;
    [SerializeField] CanvasGroup interactPrompt, modalWindow,blackScreen;
    //InputControls
    PlayerControls controls;
    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Interact.performed += context => Interact();
    }
    // Start is called before the first frame update
    void Start()
    {
        staminaBar = GameObject.Find("GameManager").GetComponent<StaminaBar>();
        manaBarSystem = GameObject.Find("GameManager").GetComponent<ManaBarSystem>();
        gameBehavior = GameObject.Find("GameManager").GetComponent<GameBehavior>();

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
        
    }
    private void Interact()
    {
        if (playerNearby && !modalWindowOpen)
        {
            ShowModalWindow();

        }
        else if (playerNearby && modalWindowOpen)
        {
            StartCoroutine(SleepThroughNight());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            DOTween.To(() => interactPrompt.alpha, x => interactPrompt.alpha = x, 1, 0.2f);
        }
    }
    private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerNearby = false;
                DOTween.To(() => interactPrompt.alpha, x => interactPrompt.alpha = x, 0, 0.2f);
        
            if (modalWindowOpen) HideModalWindow();
            }
        }
    private IEnumerator SleepThroughNight()
    {
        DOTween.To(() => blackScreen.alpha, x => blackScreen.alpha = x, 1, 3f).SetUpdate(true);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(3f);
        HideModalWindow();
        DOTween.To(() => blackScreen.alpha, x => blackScreen.alpha = x, 0, 1f).SetUpdate(true);
        EnviroSkyMgr.instance.SetTimeOfDay(7f);
        Time.timeScale = 1f;
        RegenerateAllStats();
    }
    private void ShowModalWindow()
    {
        modalWindow.alpha = 1f;
        modalWindowOpen = true;
    }
    private void HideModalWindow()
    {
        modalWindow.alpha = 0f;
        modalWindowOpen = false;
    }
    private void RegenerateAllStats()
    {
        staminaBar.stamina = StaminaBar.maxStamina;
        gameBehavior.HP = GameBehavior.maxPlayerHP;
        manaBarSystem.mana = ManaBarSystem.maxMana;
    }
    
}
