using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CraftingComponentsUIBehavior : MonoBehaviour
{
    [SerializeField] CanvasGroup materialImageCanvasGroup;
    CraftingComponentsDescriptionsHandler CraftingComponentsDescriptionsHandler;
    public string whatMaterialIsThis;
    private void Start()
    {
        CraftingComponentsDescriptionsHandler = GetComponentInParent<CraftingComponentsDescriptionsHandler>();
    }
 

    public void ShowDescription()
    {
        
        DOTween.To(() => materialImageCanvasGroup.alpha, x => materialImageCanvasGroup.alpha = x, 1f, 0.5f).SetUpdate(true);
        CraftingComponentsDescriptionsHandler.TurnOffInactiveDescriptions(whatMaterialIsThis);

    }

    // Called when the pointer exits our GUI component.

    public void HideDescription()
    {


        materialImageCanvasGroup.alpha = 0f;
    }
}
