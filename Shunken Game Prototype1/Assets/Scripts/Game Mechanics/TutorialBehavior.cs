using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;


public class TutorialBehavior : MonoBehaviour
{
    GameBehavior levelGameManager;
    public PauseMenuFunctionality tutorialGameManagerPauseMenuFunctionality;
    public static bool promptOpen = false;
    public GameObject promptGameObject;
    public Transform hUDParent;
    PlayerControls controls;
    private PlayerBehavior player;
    private GameObject currentPrompt;

    void Awake()
    {

        controls = new PlayerControls();
        controls.Player.Interact.performed += context => Interact();
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
    private void Start()
    {/*
        levelGameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameBehavior>();
        levelGameManager.gameObject.SetActive(false);
        tutorialGameManagerPauseMenuFunctionality.OnMainMenuLoad += TutorialGameManagerPauseMenuFunctionality_OnMainMenuLoad;*/
        if (!ThirdPersonController.movementEnabled)ThirdPersonController.movementEnabled = true;
    }

    private void TutorialGameManagerPauseMenuFunctionality_OnMainMenuLoad(object sender, System.EventArgs e)
    {
        levelGameManager.gameObject.SetActive(true);
        throw new System.NotImplementedException();
    }

    private void Interact()
    {
      
        if (promptOpen)
        {
            Unpause();
        }
    }
    private void Pause()
    {
        Time.timeScale = 0f;
        PauseMenuFunctionality.gameIsPaused = true;
    }
    private void Unpause()
    {
        PauseMenuFunctionality.gameIsPaused = false;
        Time.timeScale = 1f;
        Destroy(currentPrompt);
        promptOpen = false;
    }

    public void TriggerPrompt(Vector3 location, string messageText)
    {
        promptOpen = true;
       
        currentPrompt = Instantiate(promptGameObject, location, Quaternion.identity, hUDParent);
        currentPrompt.transform.localPosition = location;
        TextMeshProUGUI promptMessageText = currentPrompt.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
       
        promptMessageText.text = messageText;
        Pause();
        Debug.Log("Trigger");
    }
    public void DestroyPrompt()
    {
        Destroy(currentPrompt);
        promptOpen = false;
    }

}
