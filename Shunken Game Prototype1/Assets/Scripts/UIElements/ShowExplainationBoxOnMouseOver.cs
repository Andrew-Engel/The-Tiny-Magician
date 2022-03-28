using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ShowExplainationBoxOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] CanvasGroup explainationBoxCanvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        DOTween.To(() => explainationBoxCanvasGroup.alpha, x => explainationBoxCanvasGroup.alpha = x, 0f, 0.5f).SetUpdate(true);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("PointerEnter");
     
        DOTween.To(() => explainationBoxCanvasGroup.alpha, x => explainationBoxCanvasGroup.alpha = x, 1f, 0.5f).SetUpdate(true);
       

    }
}
