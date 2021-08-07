using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelWindow : MonoBehaviour
{
    [SerializeField] LevelSystem levelSystem;
   [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] LevelSystemAnimated levelSystemAnimated;
    //XP slider
    public Slider slider;
   
    
   
    // Start is called before the first frame update
    void Start()
    {
        //update the starting values
        SetLevelNumber(levelSystem.GetLevelNumber());
        SetExperienceBarSize(levelSystem.GetExperienceNormalized());
        //subscribe to changed events
        levelSystemAnimated.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        levelSystemAnimated.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SetExperienceBarSize(float experienceNormalized)
    {
        slider.value = experienceNormalized;
    }
    private void SetLevelNumber (int levelNumber)
    {
        levelText.text = "LEVEL \n" + (levelNumber);
    }
    public void SetLevelSystemAnimated (LevelSystemAnimated levelSystemAnimated)
    {
        //Set the lvelsystem object
        this.levelSystemAnimated = levelSystemAnimated;
        //update the starting values
        SetLevelNumber(levelSystem.GetLevelNumber());
        SetExperienceBarSize(levelSystem.GetExperienceNormalized());
        //subscribe to changed events
        levelSystemAnimated.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        levelSystemAnimated.OnLevelChanged += LevelSystem_OnLevelChanged;
    }
    public void SetLevelSystem (LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;
    }
    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
    {//level changed, updating text here
        SetLevelNumber(levelSystemAnimated.GetLevelNumber());
    }
    private void LevelSystem_OnExperienceChanged(object sender, System.EventArgs e)
    {
        //experience changed, updating bar size
        SetExperienceBarSize(levelSystemAnimated.GetExperienceNormalized());
    }


   

}
