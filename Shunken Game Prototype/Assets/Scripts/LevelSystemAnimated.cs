using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystemAnimated : MonoBehaviour
{
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;
    [SerializeField] private LevelSystem levelSystem;
    private bool isAnimating;
    private float updateTimer;
    private float updateTimerMax;


    private int level;
    private int experience;
    public int levelPublic
    {
        get { return level; }
    }

    public LevelSystemAnimated (LevelSystem levelSystem)
    {
        SetLevelSystem(levelSystem);
        updateTimerMax = 0.016f;
    }
    private void Awake()
    {
        level = levelSystem.GetLevelNumber();
        experience = levelSystem.GetExperience();


        //subscribe to changed events
        levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    public void SetLevelSystem (LevelSystem levelSystem)
    {

        this.levelSystem = levelSystem;
        level = levelSystem.GetLevelNumber();
        experience = levelSystem.GetExperience();


        //subscribe to changed events
        levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
    {//level changed, updating text here
        isAnimating = true;
    }
    private void LevelSystem_OnExperienceChanged(object sender, System.EventArgs e)
    {
        //experience changed, updating bar size
        isAnimating = true;
    }
    private void Update()
    {
        if (isAnimating)
        {
            //Check if it's time to update
            updateTimer += Time.deltaTime;
            if (updateTimer>updateTimerMax)
            //TImetoUpdate
                updateTimer -= updateTimerMax;
            UpdateAddExperience();
            
        }
    }
    private void AddExperience()
    {
        experience++;
        if (experience >= levelSystem.GetExperienceToNextLevel(level))
        {
            level++;
            experience = 0;
            if (OnLevelChanged != null)
            {
                OnLevelChanged(this, EventArgs.Empty);
            }
        }
        if (OnExperienceChanged != null)
        {
            OnExperienceChanged(this, EventArgs.Empty);

        }
    }
    private void UpdateAddExperience()
    {
    if (level < levelSystem.GetLevelNumber())
    {
        //local level under target level
        AddExperience();
    }
    else
    {
        // local level equals the target level
        if (experience < levelSystem.GetExperience())
        {
            AddExperience();
        }
        else
            isAnimating = false;
    }
    }
    public int GetExperience()
    {
        return experience;
    }
 
    public int GetLevelNumber()
    { return level; }
    public float GetExperienceNormalized()
    {
        return (float)experience / levelSystem.GetExperienceToNextLevel(level);
    }
}


