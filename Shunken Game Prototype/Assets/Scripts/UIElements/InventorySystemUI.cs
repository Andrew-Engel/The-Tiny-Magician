using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class InventorySystemUI : MonoBehaviour
{
 
    //Sounds
    [SerializeField] AudioClip healthAdditionSound,healthUseSound;
    [SerializeField] AudioClip manaAdditionSound, manaUseSound;
    [SerializeField] AudioClip staminaAdditionSound, staminaUseSound;
    [SerializeField] AudioClip bombAdditionSound;
    [SerializeField] AudioClip inventoryOpenSound;
    private AudioSource _audio;
    InventorySystem inventorySystem;
    //UI texts
    private TextMeshProUGUI quantityText, potionStat;
   
    private int statIndex;
    //CraftingMaterialsQuantityTexts
    [SerializeField] TextMeshProUGUI[] craftingMaterialQuantityTexts;
    //health UI texts
    private TextMeshProUGUI healthLevelOneQuantityText, healthLevelOneHealthStat;
    private TextMeshProUGUI healthLevelTwoQuantityText, healthLevelTwoHealthStat;
    private TextMeshProUGUI healthLevelThreeQuantityText, healthLevelThreeHealthStat;
    //mana UI texts
    private TextMeshProUGUI manaLevelOneQuantityText, manaLevelOneManaStat;
    private TextMeshProUGUI manaLevelTwoQuantityText, manaLevelTwoManaStat;
    private TextMeshProUGUI manaLevelThreeQuantityText, manaLevelThreeManaStat;
    //stamina UI texts
    private TextMeshProUGUI staminaLevelOneQuantityText, staminaLevelOneManaStat;
    private TextMeshProUGUI staminaLevelTwoQuantityText, staminaLevelTwoManaStat;
    private TextMeshProUGUI staminaLevelThreeQuantityText, staminaLevelThreeManaStat;
    //prefabs
    [SerializeField] GameObject[] healthItemUIPrefabs;
    [SerializeField] GameObject[] manaItemUIPrefabs;
    [SerializeField] GameObject[] staminaItemUIPrefabs;
    [SerializeField] GameObject[] bombItemUIPrefabs;
    [SerializeField] GameObject[] specialItemUIPrefabs;
    [SerializeField] Transform itemListParent;
   public Dictionary<string, GameObject> itemUI = new Dictionary<string, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
     
        inventorySystem = GetComponent<InventorySystem>();
        inventorySystem.OnInventoryOpen += OnInventoryOpen;
        inventorySystem.OnItemUse += On_Item_Use;
        SetUpUIDictionaryHealth();
        SetUpUIDictionaryMana();
        SetUpUIDictionaryStamina();
        SetUPUIDictionaryBomb();
        SetUpDictionarySpecialItem();
        _audio = GetComponent<AudioSource>();
    }
    private void SetUpUIDictionaryHealth()
    {
        for (int i = 1; i <= (healthItemUIPrefabs.Length); i++)
        {
            itemUI.Add("Health"+i.ToString(), healthItemUIPrefabs[i-1]);
            
        }
    }
    private void SetUpUIDictionaryMana()
    {
        for (int i = 1; i <= (manaItemUIPrefabs.Length); i++)
        {
            itemUI.Add("Mana" + i.ToString(), manaItemUIPrefabs[i - 1]);
          
        }
    }
    private void SetUpUIDictionaryStamina()
    {
        for (int i = 1; i <= (staminaItemUIPrefabs.Length); i++)
        {
            itemUI.Add("Stamina" + i.ToString(), staminaItemUIPrefabs[i - 1]);

        }
    }
    private void SetUPUIDictionaryBomb()
    {
        itemUI.Add("Bomb", bombItemUIPrefabs[0]);
    }
    private void SetUpDictionarySpecialItem()
    {
        itemUI.Add("Shard", specialItemUIPrefabs[0]);
    }
    private void On_Item_Use(object sender, InventorySystem.OnItemUseEventArgs e)
    {
        if (e.addingToInventory)
        { AddItem(e.itemUsed, e.itemLevel); }
        else if (!e.addingToInventory && !e.craftingMaterial)
        {
            UseItem(e.itemUsed, e.itemLevel);
        }
        else if (e.craftingMaterial)
        {
            RefreshCraftingMaterialAmount(e.itemUsed, e.quantity);
        }
    }
    public void RefreshCraftingMaterialAmount(string materialType, int quantity)
    {
        switch (materialType)
        {
            case ("Fiber"):
                craftingMaterialQuantityTexts[0].text = quantity.ToString();
                break;
            case ("Meat"):
                craftingMaterialQuantityTexts[1].text = quantity.ToString();
                break;
            case ("Antiseptic"):
                craftingMaterialQuantityTexts[2].text = quantity.ToString();
                break;
            case ("Blood"):
                craftingMaterialQuantityTexts[3].text = quantity.ToString();
                break;
            case ("Tinder"):
                craftingMaterialQuantityTexts[4].text = quantity.ToString();
                break;
            case ("Water"):
                craftingMaterialQuantityTexts[5].text = quantity.ToString();
                break;
            case ("Chemicals"):
                craftingMaterialQuantityTexts[6].text = quantity.ToString();
                break;
        }
    }
    public void AddItem(string item, int itemLevel)
    {
        string statTextToBeAdded = "default";
        string quantityTextToBeAdded = "default";
        string quantityTextName = "default";
        string potionStatTextName = "default";
        string itemIndex = "default";
        bool newItem = true;
        switch (item)
        {
            case ("Health"):
                {
                    _audio.PlayOneShot(healthAdditionSound, 1f);
                    switch (itemLevel)
                    {
                        case (1):
                            if (GameObject.Find("Healthlvl1UIItem(Clone)") == null)
                            {
                                newItem = true;
                                itemIndex = "Health1";
                                quantityTextName = "Item Quantity(Health1)";
                                potionStatTextName = "Item Stats(Health1)";
                                statTextToBeAdded = inventorySystem.healthItemStats[1].ToString();
                                quantityTextToBeAdded = inventorySystem.healthItemAmount["LevelOne"].ToString();
                              
                            }
                            else
                            {
                                newItem = false;
                                quantityTextName = "Item Quantity(Health1)";
                                quantityTextToBeAdded = inventorySystem.healthItemAmount["LevelOne"].ToString();
                            }
                            break;
                        case (2):
                            if (GameObject.Find("Healthlvl2UIItem(Clone)") == null)
                            {
                                newItem = true;
                                itemIndex = "Health2";
                                quantityTextName = "Item Quantity(Health2)";
                                potionStatTextName = "Item Stats(Health2)";
                                statTextToBeAdded = inventorySystem.healthItemStats[2].ToString();
                                quantityTextToBeAdded = inventorySystem.healthItemAmount["LevelTwo"].ToString();

                            }
                            else
                            {
                                newItem = false;
                                quantityTextName = "Item Quantity(Health2)";
                                quantityTextToBeAdded  = inventorySystem.healthItemAmount["LevelTwo"].ToString();
                            }
                            break;
                        case (3):
                            if (GameObject.Find("Healthlvl3UIItem(Clone)") == null)
                            {
                                newItem = true;
                                itemIndex = "Health3";
                                quantityTextName ="Item Quantity(Health3)";
                                potionStatTextName = "Item Stats(Health3)";
                                statTextToBeAdded = inventorySystem.healthItemStats[3].ToString();
                                quantityTextToBeAdded = inventorySystem.healthItemAmount["LevelThree"].ToString();

                            }
                            else
                            {
                                newItem = false;
                                quantityTextName = "Item Quantity(Health3)";
                                quantityTextToBeAdded = inventorySystem.healthItemAmount["LevelThree"].ToString();
                            }
                            break;

                    }



                }
                break;
            case ("Mana"):
                {
                    _audio.PlayOneShot(manaAdditionSound, 1f);
                    switch (itemLevel)
                    {
                        case (1):
                            if (GameObject.Find("Manalvl1UIItem(Clone)") == null)
                            {
                                newItem = true;
                                itemIndex = "Mana1";
                                quantityTextName = "Item Quantity(Mana1)";
                                potionStatTextName = "Item Stats(Mana1)";
                                statTextToBeAdded = inventorySystem.manaItemStats[1].ToString();
                                quantityTextToBeAdded = inventorySystem.manaItemAmount["LevelOne"].ToString();

                            }
                            else
                            {
                                newItem = false;
                                quantityTextName = "Item Quantity(Mana1)";
                                quantityTextToBeAdded = inventorySystem.manaItemAmount["LevelOne"].ToString();
                            }
                            break;
                        case (2):
                            if (GameObject.Find("Manalvl2UIItem(Clone)") == null)
                            {
                                newItem = true;
                                itemIndex = "Mana2";
                                quantityTextName ="Item Quantity(Mana2)";
                                potionStatTextName = "Item Stats(Mana2)";
                                statTextToBeAdded = inventorySystem.manaItemStats[2].ToString();
                                quantityTextToBeAdded = inventorySystem.manaItemAmount["LevelTwo"].ToString();

                            }
                            else
                            {
                                newItem = false;
                                quantityTextName = "Item Quantity(Mana2)";
                                quantityTextToBeAdded = inventorySystem.manaItemAmount["LevelTwo"].ToString();
                            }
                            break;
                        case (3):
                            if (GameObject.Find("Manalvl3UIItem(Clone)") == null)
                            {
                                newItem = true;
                                itemIndex = "Mana3";
                                quantityTextName ="Item Quantity(Mana3)";
                                potionStatTextName ="Item Stats(Mana3)";
                                statTextToBeAdded = inventorySystem.manaItemStats[3].ToString();
                                quantityTextToBeAdded = inventorySystem.manaItemAmount["LevelThree"].ToString();

                            }
                            else
                            {
                                newItem = false;
                                quantityTextName = "Item Quantity(Mana3)";
                                quantityTextToBeAdded = inventorySystem.manaItemAmount["LevelThree"].ToString();
                            }
                            break;

                    }
                }
                break;
            case ("Stamina"):
                {
                    _audio.PlayOneShot(staminaAdditionSound, 1f);
                    switch (itemLevel)
                    {
                        case (1):
                            if (GameObject.Find("Staminalvl1UIItem(Clone)") == null)
                            {
                                newItem = true;
                                itemIndex = "Stamina1";
                                quantityTextName = "Item Quantity(Stamina1)";
                                potionStatTextName = "Item Stats(Stamina1)";
                                statTextToBeAdded = inventorySystem.staminaItemStats[1].ToString();
                                quantityTextToBeAdded = inventorySystem.staminaItemAmount["LevelOne"].ToString();

                            }
                            else
                            {
                                newItem = false;
                                quantityTextName = "Item Quantity(Stamina1)";
                                quantityTextToBeAdded = inventorySystem.staminaItemAmount["LevelOne"].ToString();
                              //  Debug.Log($"QuantityTextToBe ADded:" + quantityTextToBeAdded);
                            }
                            break;
                        case (2):
                            if (GameObject.Find("Staminalvl2UIItem(Clone)") == null)
                            {
                                newItem = true;
                                itemIndex = "Stamina2";
                                quantityTextName = "Item Quantity(Stamina2)";
                                potionStatTextName = "Item Stats(Stamina2)";
                                statTextToBeAdded = inventorySystem.staminaItemStats[2].ToString();
                                quantityTextToBeAdded = inventorySystem.staminaItemAmount["LevelTwo"].ToString();

                            }
                            else
                            {
                                newItem = false;
                                quantityTextName = "Item Quantity(Stamina2)";
                                quantityTextToBeAdded = inventorySystem.staminaItemAmount["LevelTwo"].ToString();
                                Debug.Log($"QuantityTextToBe ADded:" + quantityTextToBeAdded);
                            }
                            break;
                        case (3):
                            if (GameObject.Find("Staminalvl3UIItem(Clone)") == null)
                            {
                                newItem = true;
                                itemIndex = "Stamina3";
                                quantityTextName = "Item Quantity(Stamina3)";
                                potionStatTextName = "Item Stats(Stamina3)";
                                statTextToBeAdded = inventorySystem.staminaItemStats[3].ToString();
                                quantityTextToBeAdded = inventorySystem.staminaItemAmount["LevelThree"].ToString();

                            }
                            else
                            {
                                newItem = false;
                                quantityTextName = "Item Quantity(Stamina3)";
                                quantityTextToBeAdded = inventorySystem.staminaItemAmount["LevelThree"].ToString();
                               // Debug.Log($"QuantityTextToBe ADded:" + quantityTextToBeAdded);
                            }
                            break;

                    }
                }
                break;
            case ("Bomb"):
                {
                    _audio.PlayOneShot(staminaAdditionSound, 1f);

                    if (GameObject.Find("BombUIitem(Clone)") == null)
                    {
                        newItem = true;
                        itemIndex = "Bomb";
                        quantityTextName = "Item Quantity(Bomb)";
                        potionStatTextName = "Item Stats(Bomb)";
                        statTextToBeAdded = " ";
                        quantityTextToBeAdded = InventorySystem.grenadeAmount.ToString();

                    }
                    else
                    {
                        newItem = false;
                        quantityTextName = "Item Quantity(Bomb)";
                        quantityTextToBeAdded = InventorySystem.grenadeAmount.ToString();
                        //  Debug.Log($"QuantityTextToBe ADded:" + quantityTextToBeAdded);
                    }

                }
                break;
            case ("Shard"):
                {
                    if (DemoObjective.fragments==1)
                    {
                        Debug.Log("ShardAdded2");
                        newItem = true;
                        itemIndex = "Shard";
                        quantityTextName = "Item Quantity(Shard)";
                        potionStatTextName = "Item Stats(Shard)";
                        statTextToBeAdded = " ";
                        quantityTextToBeAdded = DemoObjective.fragments.ToString();

                    }
                    else
                    {
                        newItem = false;
                        quantityTextToBeAdded = DemoObjective.fragments.ToString();
                        quantityTextName = "Item Quantity(Shard)";
                        Debug.Log("ShardAdded");
                    }
                    break;
                }
        }
        if (newItem)
        {
            Instantiate(itemUI[itemIndex], itemListParent.position, Quaternion.identity, itemListParent);
            quantityText = GameObject.Find(quantityTextName).GetComponent<TextMeshProUGUI>();
            if (itemIndex != "Bomb" && itemIndex != "Shard" )
            {
                potionStat = GameObject.Find(potionStatTextName).GetComponent<TextMeshProUGUI>();
                potionStat.text = statTextToBeAdded;
            }
            quantityText.text = quantityTextToBeAdded;
            //Debug.Log($"Text: " + quantityText)
        }
        else
        {
            quantityText = GameObject.Find(quantityTextName).GetComponent<TextMeshProUGUI>();
            quantityText.text = quantityTextToBeAdded;
        }
    }
    private void UseItem(string item, int itemLevel)
    {
        switch (item)
        {
            case ("Health"):
                {
                    _audio.PlayOneShot(healthUseSound, 1f);
                    switch (itemLevel)
                    {
                        case (1):
                            if (inventorySystem.healthItemAmount["LevelOne"] > 0)
                            {
                                if (healthLevelOneQuantityText == null) healthLevelOneQuantityText = GameObject.Find("Item Quantity(Health1)").GetComponent<TextMeshProUGUI>();
                                healthLevelOneQuantityText.text = inventorySystem.healthItemAmount["LevelOne"].ToString();
                            }
                            else Destroy(GameObject.Find("Healthlvl1UIItem(Clone)"));
                            
                            
                            break;
                        case (2):
                            if (inventorySystem.healthItemAmount["LevelTwo"] > 0)
                            {if (healthLevelTwoQuantityText == null) healthLevelTwoQuantityText = GameObject.Find("Item Quantity(Health2)").GetComponent<TextMeshProUGUI>();
                                healthLevelTwoQuantityText.text = inventorySystem.healthItemAmount["LevelTwo"].ToString();
                            }
                            else Destroy(GameObject.Find("Healthlvl2UIItem(Clone)"));


                            break;
                        case (3):
                            if (inventorySystem.healthItemAmount["LevelThree"] > 0)
                            {
                                if (healthLevelThreeQuantityText == null) healthLevelThreeQuantityText = GameObject.Find("Item Quantity(Health3)").GetComponent<TextMeshProUGUI>();
                                healthLevelThreeQuantityText.text = inventorySystem.healthItemAmount["LevelThree"].ToString();
                            }
                            else Destroy(GameObject.Find("Healthlvl3UIItem(Clone)"));


                            break;
                    }



                }
                break;
            case ("Mana"):
                {
                    _audio.PlayOneShot(manaUseSound, 1f);
                    switch (itemLevel)
                    {
                        case (1):
                            if (inventorySystem.manaItemAmount["LevelOne"] > 0)
                            {
                                if (manaLevelOneQuantityText == null) manaLevelOneQuantityText = GameObject.Find("Item Quantity(Mana1)").GetComponent<TextMeshProUGUI>();
                                manaLevelOneQuantityText.text = inventorySystem.manaItemAmount["LevelOne"].ToString();
                            }
                            else Destroy(GameObject.Find("Manalvl1UIItem(Clone)"));


                            break;
                        case (2):
                            if (inventorySystem.manaItemAmount["LevelTwo"] > 0)
                            {
                                if (manaLevelTwoQuantityText == null) manaLevelTwoQuantityText = GameObject.Find("Item Quantity(Mana2)").GetComponent<TextMeshProUGUI>();
                                manaLevelTwoQuantityText.text = inventorySystem.manaItemAmount["LevelTwo"].ToString();
                            }
                            else Destroy(GameObject.Find("Manalvl2UIItem(Clone)"));


                            break;
                        case (3):
                            if (inventorySystem.manaItemAmount["LevelThree"] > 0)
                            {
                                if (manaLevelThreeQuantityText == null) manaLevelThreeQuantityText = GameObject.Find("Item Quantity(Mana3)").GetComponent<TextMeshProUGUI>();
                                manaLevelThreeQuantityText.text = inventorySystem.manaItemAmount["LevelThree"].ToString();
                            }
                            else Destroy(GameObject.Find("Manalvl3UIItem(Clone)"));


                            break;
                    }



                }
                break;
            case ("Stamina"):
                {
                    _audio.PlayOneShot(staminaUseSound, 1f);
                    switch (itemLevel)
                    {
                        case (1):
                            if (inventorySystem.staminaItemAmount["LevelOne"] > 0)
                            {
                                if (staminaLevelOneQuantityText == null) staminaLevelOneQuantityText = GameObject.Find("Item Quantity(Stamina1)").GetComponent<TextMeshProUGUI>();
                                staminaLevelOneQuantityText.text = inventorySystem.staminaItemAmount["LevelOne"].ToString();
                            }
                            else Destroy(GameObject.Find("Staminalvl1UIItem(Clone)"));


                            break;
                        case (2):
                            if (inventorySystem.staminaItemAmount["LevelTwo"] > 0)
                            {
                                if (staminaLevelTwoQuantityText == null) staminaLevelTwoQuantityText = GameObject.Find("Item Quantity(Stamina2)").GetComponent<TextMeshProUGUI>();
                                staminaLevelTwoQuantityText.text = inventorySystem.staminaItemAmount["LevelTwo"].ToString();
                            }
                            else Destroy(GameObject.Find("Staminalvl2UIItem(Clone)"));


                            break;
                        case (3):
                            if (inventorySystem.staminaItemAmount["LevelThree"] > 0)
                            {
                                Debug.Log("Testing");
                                if (staminaLevelThreeQuantityText == null) staminaLevelThreeQuantityText = GameObject.Find("Item Quantity(Stamina3)").GetComponent<TextMeshProUGUI>();
                                staminaLevelThreeQuantityText.text = inventorySystem.staminaItemAmount["LevelThree"].ToString();
                            }
                            else Destroy(GameObject.Find("Staminalvl3UIItem(Clone)"));


                            break;
                    }



                }
                break;
          
        }
    }
    private void OnInventoryOpen(object sender, EventArgs e)
    {
        _audio.PlayOneShot(inventoryOpenSound, 1f);
    }
}
