using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StaminaBarUI : MonoBehaviour
{
    [SerializeField] private Image staminaBar;
    private StaminaBar staminaSystem;
    // Start is called before the first frame update
    void Start()
    {
        staminaSystem = GetComponent<StaminaBar>();
        staminaSystem.OnStaminaUse += staminaSystem_OnStaminaUse;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void staminaSystem_OnStaminaUse(object sender, StaminaBar.OnStaminaUseEventArgs e)
    {
        UpdateStaminaAnimated(e.staminaLevelNormalized, 0.5f);
    }
    public void UpdateStaminaAnimated(float staminaAmount, float tweenTime)
    {

        DOTween.To(() => staminaBar.fillAmount, x => staminaBar.fillAmount = x, staminaAmount, tweenTime);
    }
}
