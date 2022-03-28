using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;


public class IncreaseHeightOnPointerOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private RectTransform rectTransform;
    Vector3 shrunkSize = new Vector3(1f, 0.7f, 1f);

    // Start is called before the first frame update
    void Start()
    {

        rectTransform = GetComponent<RectTransform>();
     
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      
            DOTween.To(() => rectTransform.localScale, x => rectTransform.localScale = x, Vector3.one, 0.2f).SetUpdate(true);

        
    }

    // Called when the pointer exits our GUI component.

    public void OnPointerExit(PointerEventData eventData)
    {

        DOTween.To(() => rectTransform.localScale, x => rectTransform.localScale = x, shrunkSize, 0.2f).SetUpdate(true);
     

    }

}
