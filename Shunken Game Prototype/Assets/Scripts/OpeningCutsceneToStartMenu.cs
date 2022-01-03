using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using EnviroSamples;

public class OpeningCutsceneToStartMenu : MonoBehaviour
{
    public GameObject videoPlayerObject,UIParent;
    public VideoPlayer video;
    public EnviroSkyLite enviroSkyLite;
    PlayerControls controls;
    private void Awake()
    {
        controls = new PlayerControls();
        enviroSkyLite.Audio.ambientSFXVolume = 0f;
        controls.Player.AnyKey.performed += context => SkipCutScene();
    }
   
    //turn off music manager, if present
    void TurnOffPreviousMusic()
    {
        GameObject[] audioMangerPresent = GameObject.FindGameObjectsWithTag("AudioManager");
        if (audioMangerPresent.Length>0)
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
    }
    void SkipCutScene()
    {
        videoPlayerObject.SetActive(false);
        UIParent.SetActive(true);
        enviroSkyLite.Audio.ambientSFXVolume = 0.5f;
    }
    
    void EndOfCutScene(UnityEngine.Video.VideoPlayer vp)
    {
        SkipCutScene();
    }
}