using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LossScreenBehavior : MonoBehaviour
{
    [SerializeField] GameObject loadingIcon;
    public void ReturnToMainMenu()
    {
        loadingIcon.SetActive(true);
        SceneManager.LoadScene("MainMenu");
        Invoke("TurnOffLossScreenBool", 1f);
    }
    void TurnOffLossScreenBool()
    {
        GameBehavior.showLossScreen = false;
    }
    private void Start()
    {
        TurnOffPreviousMusic();
    }
    void TurnOffPreviousMusic()
    {
        AudioManager audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        Debug.Log("SoundStoppedOnLoss");
        foreach (Sound s in audioManager.sounds)
        {
            s.source.Stop();
        }
       
    
    }
}
