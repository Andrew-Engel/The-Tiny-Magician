using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CraftingComponentsUIBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] CanvasGroup materialImageCanvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        DOTween.To(() => materialImageCanvasGroup.alpha, x => materialImageCanvasGroup.alpha = x, 1f, 0.5f).SetUpdate(true);
   

    }

    // Called when the pointer exits our GUI component.

    public void OnPointerExit(PointerEventData eventData)
    {


        materialImageCanvasGroup.alpha = 0f;
    }
}
