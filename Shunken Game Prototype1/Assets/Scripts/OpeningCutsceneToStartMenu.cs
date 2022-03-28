using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using EnviroSamples;

public class OpeningCutsceneToStartMenu : MonoBehaviour
{
  
    public float endingEnviroSoundFXVolume = 0.5f;
    public GameObject videoPlayerObject,UIParent;
    public VideoPlayer video;
    public EnviroSkyLite enviroSkyLite;
    PlayerControls controls;
    private void Awake()
    {
        controls = new PlayerControls();
        enviroSkyLite.Audio.ambientSFXVolume = 0f;
        enviroSkyLite.Audio.weatherSFXVolume = 0f;
        controls.Player.AnyKey.performed += context => SkipCutScene();
        PauseMenuFunctionality.gameIsPaused = true;
        Time.timeScale = 0f;
    }

    //turn off music manager, if present
    void TurnOffPreviousMusic()
    {
        GameObject audioMangerPresent = GameObject.FindGameObjectWithTag("AudioManager");
        if (audioMangerPresent!=null)
        {
            AudioManager audioManager = audioMangerPresent.GetComponent<AudioManager>();
            foreach (Sound s in audioManager.sounds)
            {
                s.source.Pause();
            }
            /*
            foreach (GameObject g in audioMangerPresent)
            {
                AudioManager audioManager = g.GetComponent<AudioManager>();
                foreach (Sound s in audioManager.sounds)
                {
                    s.source.Pause();
                }
            }*/
        }
    }
    void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    private void Start()
    {
        video.loopPointReached += EndOfCutScene;
        TurnOffPreviousMusic();
        UIParent.SetActive(false);
    }
    void SkipCutScene()
    {
        videoPlayerObject.SetActive(false);
        UIParent.SetActive(true);
        enviroSkyLite.Audio.ambientSFXVolume = endingEnviroSoundFXVolume;
        enviroSkyLite.Audio.weatherSFXVolume = 1f;
        PauseMenuFunctionality.gameIsPaused = false;
        Time.timeScale = 1f;
    }
    
    void EndOfCutScene(UnityEngine.Video.VideoPlayer vp)
    {
        SkipCutScene();
    }
}