using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyPresenceCheckSampleScene : MonoBehaviour
{
    [SerializeField] string backgroundMusicTitle, combatMusicTitle;
    public event EventHandler OnEnemyDeath;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0 && FindObjectOfType<AudioManager>().IsSoundPlaying(combatMusicTitle))
        {
            if (!FindObjectOfType<AudioManager>().IsSoundPlaying(backgroundMusicTitle))
            {
                FindObjectOfType<AudioManager>().StopPlaying(combatMusicTitle);
                AudioManager.instance.Play(backgroundMusicTitle);
            }
        }
       
        else if (!EnemyLockOn.enemiesNearby)
        { Debug.Log("NoEnemiesNearby");
            if (!FindObjectOfType<AudioManager>().IsSoundPlaying(backgroundMusicTitle))
            {
                FindObjectOfType<AudioManager>().StopPlaying(combatMusicTitle);
                AudioManager.instance.Play(backgroundMusicTitle);
            }
        }
    }
    public void EnemyDeathEvent()
    {
        OnEnemyDeath(this, EventArgs.Empty);
    }
}
