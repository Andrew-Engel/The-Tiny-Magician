using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using DG.Tweening;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    public CinemachineVirtualCamera animationCamera, mainCamera, optionsCamera;
    AudioSource _audio;
   public AudioClip buttonClick, cameraMovementSound;
    public float animationTime;
    public CanvasGroup blackOut;
    public GameObject mainMenu, optionsMenu;
    public Slider loadingSlider;
    private string sceneUpNext;
    void Start ()
    {
        _audio = GetComponent<AudioSource>();
       if (Time.timeScale != 1f) Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void PlayGame()
    {
        Debug.Log("PlayGame");
        sceneUpNext = "Demo Scene 1";
        _audio.PlayOneShot(buttonClick);
        StartCoroutine(CameraAnimation());
        
       
    }
    public void OpenTutorial()
    {
        sceneUpNext = "Tutorial";
        _audio.PlayOneShot(buttonClick);
        StartCoroutine(CameraAnimation());
    }
    private IEnumerator CameraAnimation()
    {
        Debug.Log("CameraAnimation");
        _audio.PlayOneShot(cameraMovementSound);
        animationCamera.Priority = 20;
        yield return new WaitForSeconds(animationTime);
        DOTween.To(() => blackOut.alpha, x => blackOut.alpha = x, 1f, 2f);
       Invoke( "InitiateCoroutine",2.5f);
    }
    public void OpenOptions()
    {
        Debug.Log("OpenOptions");
        _audio.PlayOneShot(buttonClick);
        optionsCamera.Priority = 20;
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void CloseOptions()
    {
        _audio.PlayOneShot(buttonClick);
        optionsCamera.Priority = 5;
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    void InitiateCoroutine()
    {
        Debug.Log("InitiateCoroutine");
        StartCoroutine(LoadAsynchronously());
    }
    private IEnumerator LoadAsynchronously()
    {
        Debug.Log("LoadAsynchronously");
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneUpNext);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;
            yield return null;
        }
    }
}
