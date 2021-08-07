using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public enum Spells
{
    Nothing = 0,
    IceShards = 1,
    FireBall   = 2,
    FlameThrower = 3,
    EarthWave  = 4,
    Crafting = 5

};


public class SkillTreeSystem : MonoBehaviour
{
    public event EventHandler<OnSpellUnlockEventArgs> OnSpellUnlock;
    public class OnSpellUnlockEventArgs : EventArgs
    {
        public string spellToBeUnlocked;
        public string nextSpell;
    }
    private string _nextSpell = "Nothing";
    public List<string> spellsUnlocked = new List<string>();
    [SerializeField] MagicCasting playerMagicScript;
    //Level System
     private LevelSystemAnimated levelSystemAnimated;
    //UI
    [SerializeField] TextMeshProUGUI skillPointsAvailableText;
    private void Start()
    {
        spellsUnlocked.Add("Nothing");
        levelSystemAnimated = GetComponent<LevelSystemAnimated>();
        SetSkillPointsText();
    }
    //call Set LevelSsystem in in script with the instantiated level system class (the one with " LevelSystem levelSystem = new LevelSystem();' in Awake
    public void SetLevelSystemAnimated(LevelSystemAnimated levelSystemAnimated)
    {
        this.levelSystemAnimated = levelSystemAnimated;

        levelSystemAnimated.OnLevelChanged += LevelSystem_OnLevelChanged;
    }
    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
    {
        skillPoints++;
        SetSkillPointsText();
    }

    private int skillPoints =5;
    public int _skillPoints
    {
        get
        {
            return skillPoints;
        }
        set { skillPoints = value; }
    }
    private bool IsThisAMagicSpell(string spell)
    {
        Spells skill = (Spells)Enum.Parse(typeof(Spells), spell, true);
        int skillConstant = (int)skill;
        if (skillConstant < 5)
            return true;
        else return false;
    }
   public void UnlockSpell(string spell)
    {
        if (CanSpellBeUnlocked(((Spells)Enum.Parse(typeof(Spells),spell,true))) && skillPoints >0 && !spellsUnlocked.Contains(spell))
        { 
            if (IsThisAMagicSpell(spell))
            spellsUnlocked.Add(spell.ToString());
            skillPoints--;
            SetSkillPointsText();
            if (OnSpellUnlock != null)
            {

                OnSpellUnlock(this, new OnSpellUnlockEventArgs { spellToBeUnlocked = spell.ToString(), nextSpell = _nextSpell});
            }
        }
    }
    public bool CanSpellBeUnlocked( Spells spell)
    {
        switch (spell)
        {
            case Spells.IceShards:

                return true;
            case Spells.FireBall:
                _nextSpell = Spells.FlameThrower.ToString();
                return true;
               
            case Spells.FlameThrower:
                if (spellsUnlocked.Contains(Spells.FireBall.ToString()))
                {
                    _nextSpell = Spells.EarthWave.ToString();
                    return true;
                }
                else return false;
                
            case Spells.EarthWave:
                if (spellsUnlocked.Contains(Spells.FlameThrower.ToString()))
                {
                    _nextSpell = "Nothing";
                    return true;

                }
                else return false;
            case Spells.Crafting:
                _nextSpell = "Nothing";
                return true;
                break;
            default:
                return false;
        }

        
    }
    //UI
    private void SetSkillPointsText()
    {
        skillPointsAvailableText.text = "Skill Points: " + (skillPoints);
    }
}
