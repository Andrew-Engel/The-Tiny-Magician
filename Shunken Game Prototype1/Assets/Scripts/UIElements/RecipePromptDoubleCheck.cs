using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipePromptDoubleCheck : MonoBehaviour
{
    [SerializeField] CraftingSystemUIElementBehavior[] recipeButtons;
   public void TurnOffInactiveRecipes(string currentRecipe)
    {
        Debug.Log("DoubleCheck");
        for (int i = 0;  i< recipeButtons.Length; i++)
        {
            if (recipeButtons[i].whatDoesThisCraft != currentRecipe)
            {
                recipeButtons[i].TurnOffCanvasGroup();
            }
        }
    }
}
