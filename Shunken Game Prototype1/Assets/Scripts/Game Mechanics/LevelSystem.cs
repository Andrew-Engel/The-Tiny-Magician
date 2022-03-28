using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;
    private int level;
    private int experience;
    [SerializeField] LevelSystemAnimated levelSystemAnimated;
    
    public LevelSystem()
    {
        level = 1;
        experience = 0;
       
    }

    public void AddExperience(int amount)
    {
        experience += amount;
     
        while (experience >= GetExperienceToNextLevel(level))
        {
            //Enough experience to level up
            experience -= GetExperienceToNextLevel(level);
            level++;
           
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
    public int GetExperience()
    {
        return experience;
    }
    public int GetExperienceToNextLevel(int level)
    {

        return ((level + 50)  * 10);
    }
    public int GetLevelNumber()
    { return level; }
    public float GetExperienceNormalized()
    {
        return(float) experience / GetExperienceToNextLevel(level);
    }
    // THis code block is to be added to the PlayerBehvior Script
    /*
    private LevelSystemAnimated levelSystemAnimated;
    //call Set LevelSsystem in in script with the instantiated level system class (the one with " LevelSystem levelSystem = new LevelSystem();' in Awake
    public void SetLevelSystemAnimated(LevelSystemAnimated levelSystemAnimated)
    {
        this.levelSystemAnimated = levelSystemAnimated;

        levelSystemAnimated.OnLevelChanged += LevelSystem_OnLevelChanged;
    }
    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {//Put in level change effects here ie particle efffect or aniamtion or sound etc. can also put in code to increase stats

    }
    */
}
