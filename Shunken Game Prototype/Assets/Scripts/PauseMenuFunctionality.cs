using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenuFunctionality : MonoBehaviour
{
    //LoadingIcon
    public GameObject loadingIcon;

    //Sounds
    public AudioClip menuClick;
   new AudioSource audio;
    private AudioManager musicManager;
    //InputControls
    PlayerControls controls;
    public event EventHandler OnMainMenuLoad;

    public static bool gameIsPaused = false;
    //Controls
     //PlayerControls controls;
    [SerializeField] GameObject pauseMenuUI, hUD,menuBackground;
 void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Pause.performed += context =>PauseMenu();
    }
    void Start()
    {
        audio = GetComponent<AudioSource>();
        musicManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }
    void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    private void PauseMenu()
    {
        if (!InventorySystem.inventoryOpen && !GameBehavior.showLossScreen)
        {
            Debug.Log("Pause");
            audio.PlayOneShot(menuClick, 1f);
            if (gameIsPaused)
            {

                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        Cursor.visible = false;
        
        pauseMenuUI.SetActive(false);
        menuBackground.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        hUD.SetActive(true);
        foreach (Sound s in musicManager.sounds)
        {
            s.source.UnPause();
        }
     

    }
    public void PlayMenuClick()
    {
        audio.PlayOneShot(menuClick, 1f);
     

    }
   
   public void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
        menuBackground.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        hUD.SetActive(false);
        foreach (Sound s in musicManager.sounds)
        {
            s.source.Pause();
        }

    }
    public void CloseInventoryBool()
    {
        InventorySystem.inventoryOpen = false;
    }
    public void LoadMenu()
    {
        LoadingIconAnimation();
        SceneManager.LoadScene("MainMenu");
        if (OnMainMenuLoad != null)
        {
            OnMainMenuLoad(this, EventArgs.Empty);
        }
    }
  void LoadingIconAnimation()
    {
        loadingIcon.SetActive(true);
      
 
    }
 
    public void QuitGame()
    {
        Application.Quit();
    }
}
