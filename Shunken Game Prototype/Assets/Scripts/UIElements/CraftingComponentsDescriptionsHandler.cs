using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingComponentsDescriptionsHandler : MonoBehaviour
{
    public CraftingComponentsUIBehavior[] DescriptionCanvasGroup;

    public void TurnOffInactiveDescriptions(string currentComponent)
    {
        for (int i = 0; i < DescriptionCanvasGroup.Length; i++)
        {
            if (DescriptionCanvasGroup[i].whatMaterialIsThis != currentComponent)
            {
                DescriptionCanvasGroup[i].HideDescription();
            }
        }
    }
}
