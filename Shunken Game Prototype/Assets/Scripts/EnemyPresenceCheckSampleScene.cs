using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyPresenceCheckSampleScene : MonoBehaviour
{
    [SerializeField] string backgroundMusicTitle, combatMusicTitle;
    public event EventHandler OnEnemyDeath;
    public EnemyLockOn enemyLockOn;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (!GameBehavior.showLossScreen)
        {
            Debug.Log("MusicResumed");
            if (transform.childCount == 0 && FindObjectOfType<AudioManager>().IsSoundPlaying(combatMusicTitle))
            {
                if (!FindObjectOfType<AudioManager>().IsSoundPlaying(backgroundMusicTitle))
                {
                    FindObjectOfType<AudioManager>().StopPlaying(combatMusicTitle);
                    AudioManager.instance.Play(backgroundMusicTitle);
                }
            }

            else if (!EnemyLockOn.enemiesNearby)
            {
                Debug.Log("NoEnemiesNearby");
                if (!FindObjectOfType<AudioManager>().IsSoundPlaying(backgroundMusicTitle))
                {
                    FindObjectOfType<AudioManager>().StopPlaying(combatMusicTitle);
                    AudioManager.instance.Play(backgroundMusicTitle);
                }
            }
        }
    }
    public void RemoveDeadEnemyFromTransformList()
    {
        //OnEnemyDeath(this, EventArgs.Empty);
        StartCoroutine(ClearEmptyTransforms());
        // enemyLockOn.enemyTransformsList.Remove(enemyTransform);
        //enemyLockOn.enemyTransformsList.TrimExcess();
        
    }
    IEnumerator ClearEmptyTransforms()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < enemyLockOn.enemyTransformsList.Count; i++)
        {
            if (enemyLockOn.enemyTransformsList[i] == null)
            {
                enemyLockOn.enemyTransformsList.Remove(enemyLockOn.enemyTransformsList[i]);
                enemyLockOn.enemyTransformsList.TrimExcess();
            }
        }
    }
}
