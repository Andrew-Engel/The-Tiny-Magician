using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using System.Linq;

public class CraftingSystemUIElementBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    [SerializeField] CanvasGroup recipeImageCanvasGroup;
    [SerializeField] TextMeshProUGUI[] ingredientQuantatityTexts = new TextMeshProUGUI[2];
    Button button;
    [SerializeField] string whatDoesThisCraft;
    InventorySystem inventory;
    CraftingSystem craftingSystem;
    // Start is called before the first frame update
    void Start()
    {
        SetQuantityTexts(whatDoesThisCraft);
        button = GetComponent<Button>();
        inventory = GameObject.Find("GameManager").GetComponent<InventorySystem>();
        craftingSystem = GameObject.Find("GameManager").GetComponent<CraftingSystem>();
        inventory.OnCraftingMaterialAddition += On_Material_Addition;
    }
    private void On_Material_Addition(object sender, EventArgs e)
    {
       if ( craftingSystem.CheckForSufficientMaterials(whatDoesThisCraft))
        {
            button.interactable = true;
        }
       else if (!craftingSystem.CheckForSufficientMaterials(whatDoesThisCraft))
        {
            button.interactable = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

        DOTween.To(() => recipeImageCanvasGroup.alpha, x => recipeImageCanvasGroup.alpha = x, 1f, 0.5f).SetUpdate(true);


    }

    // Called when the pointer exits our GUI component.

    public void OnPointerExit(PointerEventData eventData)
    {


        recipeImageCanvasGroup.alpha = 0f;
    }
    private void SetQuantityTexts(string recipeProduct)
    {
        int numberOfIngredients = 0;
        switch (recipeProduct)
        {
            case ("Health"):
                numberOfIngredients = 2;
                for (int i = 0; i < numberOfIngredients; i++)
                {
                    ingredientQuantatityTexts[i].text = CraftingSystem.healthPotionlevelOneRecipe.ElementAt(i).Value.ToString();
                }
                break;
            case ("Mana"):
                numberOfIngredients = 3;
                for (int i = 0; i < numberOfIngredients; i++)
                {
                    ingredientQuantatityTexts[i].text = CraftingSystem.manaPotionlevelOneRecipe.ElementAt(i).Value.ToString();
                }
                break;
            case ("Stamina"):
                numberOfIngredients = 2;
                for (int i = 0; i < numberOfIngredients; i++)
                {
                    ingredientQuantatityTexts[i].text = CraftingSystem.staminaPotionlevelOneRecipe.ElementAt(i).Value.ToString();
                }
                break;
            case ("Bomb"):
                numberOfIngredients = 3;
                for (int i = 0; i < numberOfIngredients; i++)
                {
                    ingredientQuantatityTexts[i].text = CraftingSystem.bombRecipe.ElementAt(i).Value.ToString();
                }
                break;

        }

    }
}
