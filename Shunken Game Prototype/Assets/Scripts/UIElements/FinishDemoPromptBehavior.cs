using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class FinishDemoPromptBehavior : MonoBehaviour
{
    PauseMenuFunctionality pauseMenu;
    private Collider col;
    [SerializeField] float walkingTime1 =3f;
    [SerializeField] float walkingTime2 = 5f;
    public Transform targetlocation1, targetlocation2, characterTransform;
    public CanvasGroup blackOutCanvasGroup;
    public GameObject loadingIcon;
    [SerializeField] GameObject promptUIParent,UI;
    bool promptOpen;
    PlayerControls controls;
    private PlayableDirector director;
    Vector2 move;
    void Awake()
    {

        controls = new PlayerControls();
        controls.Player.Interact.performed += context => Interact();
        controls.Player.Move.performed += context => move = context.ReadValue<Vector2>();
        controls.Player.Move.canceled += context => move = Vector2.zero;
        director = GetComponent<PlayableDirector>();
        director.played += Director_Played;
        director.stopped += Director_Stopped;
        col = GetComponent<Collider>();
    }
    private void Start()
    {
        pauseMenu = GameObject.Find("GameManager").GetComponent<PauseMenuFunctionality>();
    }

    private void Director_Stopped(PlayableDirector obj)
    {
        UI.SetActive(true);
    }
    private void Director_Played(PlayableDirector obj)
    {
        UI.SetActive(false);
    }
    void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    private void Interact()
    {
        if (promptOpen)
        {
            promptUIParent.SetActive(false);
            col.enabled = false;
            
            StartTimeline();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Pause();
        promptOpen = true;
        promptUIParent.SetActive(true);
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
        promptUIParent.SetActive(false);

    }
    void StartTimeline()
    {
        Time.timeScale = 1f;
        director.Play();
    }
    private void Update()
    {
        if (promptOpen)
        {
            if (move.y <0)
            {
                ClosePrompt();
            }
        }
    }
    void ClosePrompt()
    {
        promptOpen = false;
        Unpause();
    }
    public void LoadMenu()
    {
        LoadingIconAnimation();
        pauseMenu.LoadMainMenuEvent();
        SceneManager.LoadScene("MainMenu");
     
    }
    void LoadingIconAnimation()
    {
        loadingIcon.SetActive(true);
    }
    public void BlackOut()
    {
        DOTween.To(() => blackOutCanvasGroup.alpha, x => blackOutCanvasGroup.alpha = x, 1f, 0.5f);
    }
 
    public void CallMovementMethod1()
    {
        characterTransform.DOLookAt(targetlocation1.position, 0.5f);
        characterTransform.DOMove(targetlocation1.position, walkingTime1);
    }
    public void CallMovementMethod2()
    {
        characterTransform.DOLookAt(targetlocation2.position, 0.5f);
        characterTransform.DOMove(targetlocation2.position, walkingTime2);
    }
}
