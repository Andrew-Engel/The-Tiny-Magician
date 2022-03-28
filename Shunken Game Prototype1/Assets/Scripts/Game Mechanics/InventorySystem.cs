using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    private AudioManager musicManager;
    [SerializeField] CanvasGroup inventoryCanvasGroup;
    public static bool inventoryOpen = false;
    [SerializeField] GameObject hUD;
    public event EventHandler OnInventoryOpen;
    public event EventHandler OnCraftingMaterialAddition;
    public event EventHandler<OnItemUseEventArgs> OnItemUse;
    public class OnItemUseEventArgs : EventArgs
    {
        public string itemUsed;
        public int itemLevel;
        public bool addingToInventory;
        public int quantity;
        public bool craftingMaterial;
    }
    public static int maxInventorySize= 6;
    //InputControls
    PlayerControls controls;
    //Health Item Stats (level, healthgiven)
    public Dictionary<int, int> healthItemStats = new Dictionary<int, int>
  {
      {1,5 },
      {2,10 },
      {3,15 }
  };
    //Mana Item Stats (level, healthgiven)
    public Dictionary<int, int> manaItemStats = new Dictionary<int, int>
  {
      {1,15 },
      {2,30 },
      {3,60 }
  };
    //Stamina Item Stats (level, healthgiven)
    public Dictionary<int, int> staminaItemStats = new Dictionary<int, int>
  {
      {1,15 },
      {2,30 },
      {3,60 }
  };
    //health item quantities
    public Dictionary<string, int> healthItemAmount = new Dictionary<string, int>
    {
        { "LevelOne",0 },
        { "LevelTwo",0},
        { "LevelThree",0 }

    };
    //mana item quantities
    public Dictionary<string, int> manaItemAmount = new Dictionary<string, int>
    {
        { "LevelOne",0 },
        { "LevelTwo",0},
        { "LevelThree",0 }

    };
    //stamina item quantities
    public Dictionary<string, int> staminaItemAmount = new Dictionary<string, int>
    {
        { "LevelOne",0 },
        { "LevelTwo",0},
        { "LevelThree",0 }

    };
    // amount of grenades
    public static int grenadeAmount = 0; 
    private int grenadeDummyAmount
    {
        get { return grenadeAmount; }
        set { if (grenadeAmount<1)
            {
                GameObject BombUI = GameObject.Find("BombUIitem");
                Destroy(BombUI);

            }
                    }
    }
    public static Dictionary<string, int> craftingMaterialAmounts = new Dictionary<string, int>
    {
        {CraftingMaterials.Fiber.ToString(),0 },
        {CraftingMaterials.Meat.ToString(),0 },
        {CraftingMaterials.Antiseptic.ToString(),0 },
        {CraftingMaterials.Blood.ToString(),0 },
        {CraftingMaterials.Tinder.ToString(),0 },
        {CraftingMaterials.Water.ToString(),0 },
         {CraftingMaterials.Chemicals.ToString(),0 }
    };
//other scripts containing player HP, mana, stamina
GameBehavior gameManager;
    ManaBarSystem manaBar;
    StaminaBar staminaBar;
    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.OpenInventory.performed += context => OpenInventory();
    }
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameBehavior>();
        manaBar = GetComponent<ManaBarSystem>();
        staminaBar = GetComponent<StaminaBar>();
        musicManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }
    void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
  
    public void AddItem(string itemTitle, int itemLevel)
    {
       
        switch (itemTitle)
        {
            case ("Health"):
                switch (itemLevel)
                {
                    case (1):
                      healthItemAmount["LevelOne"]++;
                        break;
                    case (2):
                        healthItemAmount["LevelTwo"]++;
                        break;
                    case (3):
                        healthItemAmount["LevelThree"]++;
                        break;
                }
                break;
            case ("Mana"):
                switch (itemLevel)
                {
                    case (1):
                        manaItemAmount["LevelOne"]++;
                        break;
                    case (2):
                        manaItemAmount["LevelTwo"]++;
                        break;
                    case (3):
                        manaItemAmount["LevelThree"]++;
                        break;
                }
                break;
            case ("Stamina"):
                switch (itemLevel)
                {
                    case (1):
                        staminaItemAmount["LevelOne"]++;
                        break;
                    case (2):
                        staminaItemAmount["LevelTwo"]++;
                        break;
                    case (3):
                        staminaItemAmount["LevelThree"]++;
                        break;
                }
                break;
            case ("Bomb"):
                grenadeAmount++;
                break;
        }
        Debug.Log("ItemAdded Test");
        if (OnItemUse != null)
        { OnItemUse(this, new OnItemUseEventArgs { itemUsed = itemTitle, itemLevel = itemLevel, addingToInventory = true }); }
    }
    public void AddCraftingMaterial(string itemTitle, int quantity)
    {
        switch (itemTitle)
        {
            case ("Fiber"):
                craftingMaterialAmounts["Fiber"] += quantity;
                break;
            case ("Meat"):
                craftingMaterialAmounts["Meat"] += quantity;
                break;
            case ("Antiseptic"):
                craftingMaterialAmounts["Antiseptic"] += quantity;
                break;
            case ("Blood"):
                craftingMaterialAmounts["Blood"] += quantity;
                break;
            case ("Tinder"):
                craftingMaterialAmounts["Tinder"] += quantity;
                break;
            case ("Water"):
                craftingMaterialAmounts["Water"] += quantity;
                break;
            case ("Chemicals"):
                craftingMaterialAmounts["Chemicals"] += quantity;
                break;
        }

        OnCraftingMaterialAdditionEvent();
        if (OnItemUse != null)
        { OnItemUse(this, new OnItemUseEventArgs { itemUsed = itemTitle, quantity = craftingMaterialAmounts[itemTitle], craftingMaterial = true }); }

    }
    public void OnCraftingMaterialAdditionEvent()
    {
        if (OnCraftingMaterialAddition != null)
        { OnCraftingMaterialAddition(this, EventArgs.Empty); }
    }
    public void UseHealthItem(int itemLevel)
    {
       
        int healthIncrease =0;

        switch (itemLevel)
        {

            case (1):
                healthItemAmount["LevelOne"]--; 
                healthIncrease = healthItemStats[1];
                Debug.Log("HealthLevelOneUsed");
                break;
            case (2):
                healthItemAmount["LevelTwo"]--; 
                healthIncrease = healthItemStats[2];
                Debug.Log("HealthLevelTwoUsed");
                break;
            case (3):
                healthItemAmount["LevelThree"]--;
                healthIncrease = healthItemStats[3];
                Debug.Log("HealthLevelThreeUsed");
                break;
        }
       
        gameManager.HP += healthIncrease;
        OnItemUse(this, new OnItemUseEventArgs { itemUsed = "Health", itemLevel = itemLevel , addingToInventory = false }); 

    }
    public void UseManaItem(int itemLevel)
    {

        int manaIncrease = 0;

        switch (itemLevel)
        {

            case (1):
                manaItemAmount["LevelOne"]--;
                manaIncrease = manaItemStats[1];
                Debug.Log("ManaLevelOneUsed");
                break;
            case (2):
                manaItemAmount["LevelTwo"]--;
                manaIncrease = manaItemStats[2];
                Debug.Log("ManaLevelTwoUsed");
                break;
            case (3):
                manaItemAmount["LevelThree"]--;
                manaIncrease = manaItemStats[3];
                Debug.Log("ManaLevelThreeUsed");
                break;
        }

        manaBar.mana += manaIncrease;
        OnItemUse(this, new OnItemUseEventArgs { itemUsed = "Mana", itemLevel = itemLevel, addingToInventory = false });

    }
    public void UseStaminaItem(int itemLevel)
    {

        int staminaIncrease = 0;

        switch (itemLevel)
        {

            case (1):
                staminaItemAmount["LevelOne"]--;
                staminaIncrease = staminaItemStats[1];
                Debug.Log("staminaLevelOneUsed");
                break;
            case (2):
                staminaItemAmount["LevelTwo"]--;
                staminaIncrease = staminaItemStats[2];
                Debug.Log("staminaLevelTwoUsed");
                break;
            case (3):
                staminaItemAmount["LevelThree"]--;
                staminaIncrease = staminaItemStats[3];
             
                break;
        }

        staminaBar.stamina += staminaIncrease;
        Debug.Log(staminaBar.stamina);
        OnItemUse(this, new OnItemUseEventArgs { itemUsed = "Stamina", itemLevel = itemLevel, addingToInventory = false });

    }
    private void OpenInventory()
    {
        Debug.Log("OpenInventory");
        if (!inventoryOpen && !PauseMenuFunctionality.gameIsPaused && !GameBehavior.showLossScreen)
        {
            if (OnInventoryOpen != null)
            OnInventoryOpen(this, EventArgs.Empty);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            inventoryOpen = true;
            Time.timeScale = 0f;
            hUD.SetActive(false);
            inventoryCanvasGroup.interactable = true;
            inventoryCanvasGroup.blocksRaycasts = true;
            DOTween.To(() => inventoryCanvasGroup.alpha, x => inventoryCanvasGroup.alpha = x, 1f, 0.3f).SetUpdate(true);
            foreach (Sound s in musicManager.sounds)
            {
                s.source.Pause();
            }
        }
        else if (inventoryOpen && !PauseMenuFunctionality.gameIsPaused)
        {
           
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            inventoryOpen = false;
            Time.timeScale = 1f;
            hUD.SetActive(true);
            inventoryCanvasGroup.interactable = false;
            inventoryCanvasGroup.blocksRaycasts = false;
            DOTween.To(() => inventoryCanvasGroup.alpha, x => inventoryCanvasGroup.alpha = x, 0f, 0.3f).SetUpdate(true);
            foreach (Sound s in musicManager.sounds)
            {
                s.source.UnPause();
            }
        }
    }
}
