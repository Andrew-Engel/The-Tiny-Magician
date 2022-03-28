using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DemoObjective : MonoBehaviour
{
    
    public TimelineManager timelineManager;
    InventorySystemUI inventoryUI;
    bool finishLevel = false;
    bool openConfirmWindow = false;
    AudioSource audioSource;
    [Tooltip("1/2: Not enough shards; 3/4: Enough Shards; 5: Confirm Clear")]
    public List<AudioClip> interactionAudio = new List<AudioClip>();
    bool playerNearby;
    PlayerControls controls;
    [SerializeField] int fragmentsNeeded;
    public static bool levelCompleted = false;
  public static int fragments = 0;
    public  int  fragmentsCollected
    {
        get { return fragments; }
        set { fragments = value;
            if (fragmentsCollected >= fragmentsNeeded) levelCompleted = true;
             inventoryUI.AddItem("Shard", 1);
       
        }
    }
    [SerializeField] TextMeshProUGUI shardNumber, shardsNeeded;
        [SerializeField] GameObject notEnoughShardsTextParent, enoughShardsTextParent, confirmClearParent;
    [SerializeField] GameObject interactPrompt;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        shardsNeeded.text = fragmentsNeeded.ToString();
        shardNumber.text = fragments.ToString();
        inventoryUI = GameObject.Find("GameManager").GetComponent<InventorySystemUI>();
    }
    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Interact.performed += context => TryToClearRock();
      
    }
    void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void TryToClearRock()
    {
        if (playerNearby && !PauseMenuFunctionality.gameIsPaused)
        {
            if (levelCompleted)
            {
                ClearRock();
            }
            else
            {
                CanNotClearRock();
            }


        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            ShowInteractPrompt();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            CloseAllUI();
            ShowInteractPrompt();
        }
    }
    private void CanNotClearRock()
    {
        notEnoughShardsTextParent.SetActive(true);
        shardNumber.text = fragments.ToString();
        audioSource.PlayOneShot(interactionAudio[0]);
        audioSource.PlayOneShot(interactionAudio[1]);
    }
    private void ClearRock()
    {
        if (!openConfirmWindow)
        {
            enoughShardsTextParent.SetActive(true);
            openConfirmWindow = true;
        }
        else if (openConfirmWindow && !finishLevel)
        {
            confirmClearParent.SetActive(true);
            finishLevel = true;
        }
        else StartEndGameCutScene();
    
    }
    private void CloseAllUI()
    {
        notEnoughShardsTextParent.SetActive(false);
        enoughShardsTextParent.SetActive(false);
        confirmClearParent.SetActive(false);
        finishLevel = false;
        openConfirmWindow = false;
    }
    private void StartEndGameCutScene()
    {
        TurnOffPreviousMusic();
        Debug.Log("RockCleared");
        CloseAllUI();
        timelineManager.StartTimeline();
       
    }
    private void ShowInteractPrompt()
    {
        if (playerNearby)
        {
            interactPrompt.SetActive(true);
        }
        else interactPrompt.SetActive(false);
    }
    void TurnOffPreviousMusic()
    {
        GameObject[] audioMangerPresent = GameObject.FindGameObjectsWithTag("AudioManager");
        if (audioMangerPresent.Length > 0)
        {
            foreach (GameObject g in audioMangerPresent)
            {
                AudioManager audioManager = g.GetComponent<AudioManager>();
                foreach (Sound s in audioManager.sounds)
                {
                    s.source.Pause();
                }
            }
        }
    }
}
