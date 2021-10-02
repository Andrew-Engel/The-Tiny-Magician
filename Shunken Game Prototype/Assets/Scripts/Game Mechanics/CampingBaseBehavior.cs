using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CampingBaseBehavior : MonoBehaviour
{
    Animator playerAnimator;
    StaminaBar staminaBar;
    ManaBarSystem manaBarSystem;
    GameBehavior gameBehavior;
    bool playerNearby, modalWindowOpen;
    [SerializeField] CanvasGroup interactPrompt, modalWindow,blackScreen;
    [SerializeField] GameObject enemiesNearbyMessage;
    CanvasGroup enemyNearbyCG;
    RectTransform enemyNearbyRT;
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
        playerAnimator = GameObject.Find("PlayerModel").GetComponent<Animator>();
        staminaBar = GameObject.Find("GameManager").GetComponent<StaminaBar>();
        manaBarSystem = GameObject.Find("GameManager").GetComponent<ManaBarSystem>();
        gameBehavior = GameObject.Find("GameManager").GetComponent<GameBehavior>();
        enemyNearbyCG = enemiesNearbyMessage.GetComponent<CanvasGroup>();
        enemyNearbyRT = enemiesNearbyMessage.GetComponent<RectTransform>();
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
        else if (playerNearby && modalWindowOpen && !EnemyLockOn.enemiesNearby)
        {
            StartCoroutine(SleepThroughNight());
        }
        else if (playerNearby && modalWindowOpen && EnemyLockOn.enemiesNearby)
        {
            StartCoroutine(ShowEnemyWarning());
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
            if (enemyNearbyCG.alpha > 0) HideEnemyWarning();
            }
        }
    private IEnumerator SleepThroughNight()
    {
        playerAnimator.SetTrigger("WakeUp");
        DOTween.To(() => blackScreen.alpha, x => blackScreen.alpha = x, 1, 2f).SetUpdate(true);

        Invoke("SetTime", 2f);
        yield return new WaitForSecondsRealtime(3f);
        HideModalWindow();
        EnviroSkyMgr.instance.SetTimeOfDay(7f);
        Time.timeScale = 1f;
        DOTween.To(() => blackScreen.alpha, x => blackScreen.alpha = x, 0, 2f).SetUpdate(true);
     
        
        
        
        RegenerateAllStats();
    }
    private void SetTime()
    {
        if (Time.timeScale >0f) Time.timeScale = 0f;
        else Time.timeScale = 1f;
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
    private IEnumerator ShowEnemyWarning()
    {
        enemyNearbyCG.alpha = 1f;

        enemyNearbyRT.DOScale(1.3f, 0.8f);
        yield return new WaitForSeconds(0.5f);
        enemyNearbyRT.DOScale(1f, 0.8f);
    }
    private void HideEnemyWarning()
    {
        enemyNearbyCG.alpha = 0f;
    }
    
}
