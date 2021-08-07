using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    void Start ()
    {
        FindObjectOfType<AudioManager>().Play("AmbientSound");
        if (Time.timeScale != 1f)
            Time.timeScale = 1f;
    }
    public void PlayGame()
    {
        FindObjectOfType<AudioManager>().Play("MenuClick");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
