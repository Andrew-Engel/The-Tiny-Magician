using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum CraftingMaterials
{
    Fiber = 0,
    Meat = 1,
    Antiseptic = 2,
    Blood = 3,
    Tinder = 4,
    Water = 5,
    Chemicals = 6

};
public class CraftingSystem : MonoBehaviour
{
    public event EventHandler<OnItemCraftingEventArgs> OnItemCrafting;
    public class OnItemCraftingEventArgs : EventArgs
    {
        public string itemCrafted;
       
    }
    private InventorySystem inventory;
    private InventorySystemUI inventoryUI;
    // Item recipes
    public static Dictionary<string, int> healthPotionlevelOneRecipe = new Dictionary<string, int>
    {
        {CraftingMaterials.Fiber.ToString(),4 },
        {CraftingMaterials.Antiseptic.ToString(), 2 },
    };
    public static Dictionary<string, int> manaPotionlevelOneRecipe = new Dictionary<string, int>
    {
        {CraftingMaterials.Blood.ToString(),2 },
        {CraftingMaterials.Meat.ToString(), 2 },
        {CraftingMaterials.Tinder.ToString(), 2 }
    };
    public static Dictionary<string, int> staminaPotionlevelOneRecipe = new Dictionary<string, int>
    {
        {CraftingMaterials.Water.ToString(),3 },
        {CraftingMaterials.Blood.ToString(), 1 },
    };
    
    public static Dictionary<string, int> bombRecipe = new Dictionary<string, int>
    {
        {CraftingMaterials.Tinder.ToString(),3 },
        {CraftingMaterials.Fiber.ToString(), 2 },
        {CraftingMaterials.Chemicals.ToString(), 1 },
    };
    private void Start()
    {
        inventory = GetComponent<InventorySystem>();
        inventoryUI = GetComponent<InventorySystemUI>();
    }
    //Crafting Methods
    public void CraftHealthPotion()
    {
       if ( CheckForSufficientMaterials("Health"))
        {
            Debug.Log("HealthCrafted");
            inventory.AddItem("Health", 1);
            InventorySystem.craftingMaterialAmounts[CraftingMaterials.Fiber.ToString()] -= healthPotionlevelOneRecipe[CraftingMaterials.Fiber.ToString()];
            InventorySystem.craftingMaterialAmounts[CraftingMaterials.Antiseptic.ToString()] -= healthPotionlevelOneRecipe[CraftingMaterials.Antiseptic.ToString()];
            
            
            inventoryUI.RefreshCraftingMaterialAmount(CraftingMaterials.Fiber.ToString(), InventorySystem.craftingMaterialAmounts[CraftingMaterials.Fiber.ToString()]);
            inventoryUI.RefreshCraftingMaterialAmount(CraftingMaterials.Antiseptic.ToString(), InventorySystem.craftingMaterialAmounts[CraftingMaterials.Antiseptic.ToString()]);


            if (OnItemCrafting != null)
            OnItemCrafting(this, new OnItemCraftingEventArgs { itemCrafted = "Health" });

        }

    }
    public void CraftManaPotion()
    {
        if (CheckForSufficientMaterials("Mana"))
        {
            inventory.AddItem("Mana", 1);
            InventorySystem.craftingMaterialAmounts[CraftingMaterials.Meat.ToString()] -= manaPotionlevelOneRecipe[CraftingMaterials.Meat.ToString()];
            InventorySystem.craftingMaterialAmounts[CraftingMaterials.Tinder.ToString()] -= manaPotionlevelOneRecipe[CraftingMaterials.Tinder.ToString()];
            InventorySystem.craftingMaterialAmounts[CraftingMaterials.Blood.ToString()] -= manaPotionlevelOneRecipe[CraftingMaterials.Blood.ToString()];

            inventoryUI.RefreshCraftingMaterialAmount(CraftingMaterials.Meat.ToString(), InventorySystem.craftingMaterialAmounts[CraftingMaterials.Meat.ToString()]);
            inventoryUI.RefreshCraftingMaterialAmount(CraftingMaterials.Tinder.ToString(), InventorySystem.craftingMaterialAmounts[CraftingMaterials.Tinder.ToString()]);
            inventoryUI.RefreshCraftingMaterialAmount(CraftingMaterials.Blood.ToString(), InventorySystem.craftingMaterialAmounts[CraftingMaterials.Blood.ToString()]);

            inventory.OnCraftingMaterialAdditionEvent();
            if (OnItemCrafting != null)
                OnItemCrafting(this, new OnItemCraftingEventArgs { itemCrafted = "Stamina" });

        }

    }
    public void CraftStaminaPotion()
    {
        if (CheckForSufficientMaterials("Stamina"))
        {
            inventory.AddItem("Stamina", 1);
            InventorySystem.craftingMaterialAmounts[CraftingMaterials.Water.ToString()] -= staminaPotionlevelOneRecipe[CraftingMaterials.Water.ToString()];
            InventorySystem.craftingMaterialAmounts[CraftingMaterials.Blood.ToString()] -= staminaPotionlevelOneRecipe[CraftingMaterials.Blood.ToString()];

            inventoryUI.RefreshCraftingMaterialAmount(CraftingMaterials.Water.ToString(), InventorySystem.craftingMaterialAmounts[CraftingMaterials.Water.ToString()]);
            inventoryUI.RefreshCraftingMaterialAmount(CraftingMaterials.Blood.ToString(), InventorySystem.craftingMaterialAmounts[CraftingMaterials.Blood.ToString()]);

            if (OnItemCrafting != null)
            OnItemCrafting(this, new OnItemCraftingEventArgs { itemCrafted = "Stamina" });
    

        }

    }
    public void CraftBomb()
    {
        if (CheckForSufficientMaterials("Bomb"))
        {
            inventory.AddItem("Bomb", 1);
            InventorySystem.craftingMaterialAmounts[CraftingMaterials.Tinder.ToString()] -= bombRecipe[CraftingMaterials.Tinder.ToString()];
            InventorySystem.craftingMaterialAmounts[CraftingMaterials.Fiber.ToString()] -= bombRecipe[CraftingMaterials.Fiber.ToString()];
            InventorySystem.craftingMaterialAmounts[CraftingMaterials.Chemicals.ToString()] -= bombRecipe[CraftingMaterials.Chemicals.ToString()];

            inventoryUI.RefreshCraftingMaterialAmount(CraftingMaterials.Tinder.ToString(), InventorySystem.craftingMaterialAmounts[CraftingMaterials.Tinder.ToString()]);
            inventoryUI.RefreshCraftingMaterialAmount(CraftingMaterials.Fiber.ToString(), InventorySystem.craftingMaterialAmounts[CraftingMaterials.Fiber.ToString()]);
            inventoryUI.RefreshCraftingMaterialAmount(CraftingMaterials.Chemicals.ToString(), InventorySystem.craftingMaterialAmounts[CraftingMaterials.Chemicals.ToString()]);
            if (OnItemCrafting != null)
                OnItemCrafting(this, new OnItemCraftingEventArgs { itemCrafted = "Bomb" });
        }
    }
   
    public bool CheckForSufficientMaterials(string productType)
    {switch (productType)
        {
            case ("Health"):
                if (InventorySystem.craftingMaterialAmounts[CraftingMaterials.Fiber.ToString()] >= healthPotionlevelOneRecipe[CraftingMaterials.Fiber.ToString()] && InventorySystem.craftingMaterialAmounts[CraftingMaterials.Antiseptic.ToString()] >= healthPotionlevelOneRecipe[CraftingMaterials.Antiseptic.ToString()])
                    return true;
                else return false;
                    
                    break;
            case ("Mana"):
                if (InventorySystem.craftingMaterialAmounts[CraftingMaterials.Blood.ToString()] >= manaPotionlevelOneRecipe[CraftingMaterials.Blood.ToString()] && InventorySystem.craftingMaterialAmounts[CraftingMaterials.Meat.ToString()] >= manaPotionlevelOneRecipe[CraftingMaterials.Meat.ToString()] &&  InventorySystem.craftingMaterialAmounts[CraftingMaterials.Tinder.ToString()] >= manaPotionlevelOneRecipe[CraftingMaterials.Tinder.ToString()])
                    return true;
                else return false;
            case ("Stamina"):
                if (InventorySystem.craftingMaterialAmounts[CraftingMaterials.Water.ToString()] >= staminaPotionlevelOneRecipe[CraftingMaterials.Water.ToString()] && InventorySystem.craftingMaterialAmounts[CraftingMaterials.Blood.ToString()] >= staminaPotionlevelOneRecipe[CraftingMaterials.Blood.ToString()])
                    return true;
                else return false;
            case ("Bomb"):
                if (InventorySystem.craftingMaterialAmounts[CraftingMaterials.Tinder.ToString()] >= bombRecipe[CraftingMaterials.Tinder.ToString()] && InventorySystem.craftingMaterialAmounts[CraftingMaterials.Tinder.ToString()] >= bombRecipe[CraftingMaterials.Tinder.ToString()] && InventorySystem.craftingMaterialAmounts[CraftingMaterials.Chemicals.ToString()] >= bombRecipe[CraftingMaterials.Chemicals.ToString()])
                    return true;
                else return false;
            default:
                return false;
                break;

        }
    }
 

}
