using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using RootMotion.Dynamics;


public enum Spells
{
    Nothing = 0,
    IceShards = 1,
    FireBall   = 2,
    FlameThrower = 3,
    EarthWave  = 4,
    AirEscape = 5,
    AirDash = 6,
    IceBlast = 7,
    Agility = 8,
    Crafting = 9,


};


public class SkillTreeSystem : MonoBehaviour
{
 
    BehaviourPuppet behaviourPuppet;
    ManaBarSystem manaBar;
    GameBehavior _HP;
    StaminaBar stamina;
    public event EventHandler<OnSpellUnlockEventArgs> OnSpellUnlock;
    public EventHandler OnStatIncrease;
    public class OnSpellUnlockEventArgs : EventArgs
    {
        public string spellToBeUnlocked;
        public string nextSpell;
        public bool addToInventory;
    }
    private string _nextSpell = "Nothing";
    public List<string> spellsUnlocked = new List<string>();

    //Level System
     private LevelSystemAnimated levelSystemAnimated;
    //UI
    [SerializeField] TextMeshProUGUI skillPointsAvailableText;
    private void Awake()
    {
        levelSystemAnimated = GetComponent<LevelSystemAnimated>();
        levelSystemAnimated.OnLevelChanged += LevelSystem_OnLevelChanged;
    }
    private void Start()
    {
        behaviourPuppet = GameObject.Find("Puppet").GetComponent<BehaviourPuppet>();
        spellsUnlocked.Add("Nothing");
       
        SetSkillPointsText();
        manaBar = GetComponent<ManaBarSystem>();
        stamina = GetComponent<StaminaBar>();
        _HP = GetComponent<GameBehavior>();
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

    public int skillPoints =3;
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
        if (skillConstant < 8)
            return true;
        else return false;
    }
   public void UnlockSpell(string spell)
    {
        if (CanSpellBeUnlocked(((Spells)Enum.Parse(typeof(Spells),spell,true))) && skillPoints >0 && !spellsUnlocked.Contains(spell))
        { bool AddToInventory = false;
           
            if (IsThisAMagicSpell(spell))
            {
                spellsUnlocked.Add(spell.ToString());
                AddToInventory = true;
            }
            skillPoints--;
            SetSkillPointsText();
            if (OnSpellUnlock != null)
            {

                OnSpellUnlock(this, new OnSpellUnlockEventArgs { spellToBeUnlocked = spell.ToString(), nextSpell = _nextSpell, addToInventory = AddToInventory});
            }
        }
    }
    public bool CanSpellBeUnlocked( Spells spell)
    {
        switch (spell)
        {
            case Spells.IceShards:
                _nextSpell = Spells.IceBlast.ToString();
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
            case Spells.AirEscape:
                return true;
            case Spells.AirDash:
                if (spellsUnlocked.Contains(Spells.AirEscape.ToString()))
                {
                    _nextSpell = "Nothing";
                    return true;

                }
                else return false;
            case Spells.Crafting:
                _nextSpell = "Agility";
                return true;
            case Spells.Agility:
                
                    _nextSpell = "Nothing";
                    return true;
            case Spells.IceBlast:
                    if (spellsUnlocked.Contains(Spells.IceShards.ToString()))
                {
                    _nextSpell = "Nothing";
                    return true;
                }
                else return false;
            default:
                return false;
        }

        
    }
    public void IncreaseStat(string statType)
    {
        int portionStatIncrese = 5;
        if (skillPoints > 0)
        {
            switch (statType)
            {
                case ("Mana"):
                    if (ManaBarSystem.maxMana < ManaBarSystem.absoluteMaxMana)
                    {
                        int statIncrease = (ManaBarSystem.absoluteMaxMana / portionStatIncrese);
                        ManaBarSystem.maxMana += statIncrease;
                        manaBar.mana = ManaBarSystem.maxMana;
                        manaBar.UpdateMana();
                        skillPoints--;
                        SetSkillPointsText();
                        if (OnStatIncrease != null)
                            OnStatIncrease(this, EventArgs.Empty);
                    }
                    break;
                case ("Health"):
                    if (GameBehavior.maxPlayerHP < GameBehavior.absoluteMaxHP)
                    {
                        int statIncrease = (GameBehavior.absoluteMaxHP / portionStatIncrese);
                        GameBehavior.maxPlayerHP += statIncrease;
                        _HP.HP = GameBehavior.maxPlayerHP;
                        _HP.SendHealthToHealthBar(_HP.HP);
                        skillPoints--;
                        SetSkillPointsText();
                        if (OnStatIncrease != null)
                            OnStatIncrease(this, EventArgs.Empty);
                    }
                    break;
                case ("Stamina"):
                    if (StaminaBar.maxStamina < StaminaBar.absoluteMaxStamina)
                    {
                        int statIncrease = (StaminaBar.absoluteMaxStamina / portionStatIncrese);
                        StaminaBar.maxStamina += statIncrease;
                        stamina.stamina = StaminaBar.maxStamina;
                        stamina.UpdateStamina();
                        skillPoints--;
                        SetSkillPointsText();
                        if (OnStatIncrease != null)
                            OnStatIncrease(this, EventArgs.Empty);
                    }
                    break;
            }
        }
    }
    //UI
    private void SetSkillPointsText()
    {
        skillPointsAvailableText.text = "Skill Points: " + (skillPoints);
    }
    public void IncreaseAgility()
    {
        skillPoints--;
        Debug.Log("Agility");
        behaviourPuppet.collisionResistance = new Weight(2000);
        
    }
}
