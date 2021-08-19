using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class StatIncreaseUIBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] string statType;
    Button button;
    private RectTransform rectTransform;
    Vector3 shrunkSize = new Vector3(0.7f, 0.7f, 0.7f);
    [SerializeField] Color highlightedImageColor, offPointerColor;
    [SerializeField] Image logoImage;
    private SkillTreeSystem _ST;
    // Start is called before the first frame update
    void Start()
    {

        rectTransform = GetComponent<RectTransform>();
        _ST = GameObject.Find("GameManager").GetComponent<SkillTreeSystem>();
        _ST.OnStatIncrease += OnStatIncrease;
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
        {
            DOTween.To(() => rectTransform.localScale, x => rectTransform.localScale = x, Vector3.one, 0.5f).SetUpdate(true);
            logoImage.DOColor(highlightedImageColor, 0.8f).SetUpdate(true);
        }
    }

    // Called when the pointer exits our GUI component.

    public void OnPointerExit(PointerEventData eventData)
    {

        DOTween.To(() => rectTransform.localScale, x => rectTransform.localScale = x, shrunkSize, 0.5f).SetUpdate(true);
        logoImage.DOColor(offPointerColor, 0.8f).SetUpdate(true);

    }
    private void OnStatIncrease(object sender, EventArgs e)
    {
        switch (statType)
        {
            case ("Mana"):

                if (ManaBarSystem.maxMana >= ManaBarSystem.absoluteMaxMana)
                {
                    button.interactable = false;
                    logoImage.DOColor(offPointerColor, 0.8f).SetUpdate(true);
                }
                break;
            case ("Health"):

                if (GameBehavior.maxPlayerHP >= GameBehavior.absoluteMaxHP)
                {
                    button.interactable = false;
                    logoImage.DOColor(offPointerColor, 0.8f).SetUpdate(true);
                }
                break;

            case ("Stamina"):

                if (StaminaBar.maxStamina >= StaminaBar.absoluteMaxStamina)
                {
                    Debug.Log("button disabled");
                    button.interactable = false;
                }

                break;

        }

    }
}
  
