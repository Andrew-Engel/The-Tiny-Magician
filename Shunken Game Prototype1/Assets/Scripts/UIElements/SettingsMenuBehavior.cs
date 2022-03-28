using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;

public class SettingsMenuBehavior : MonoBehaviour
{
    [SerializeField] Slider lookSensitivitySlider;
    private void Start()
    {
        lookSensitivitySlider.onValueChanged.AddListener((v) => ChangeLookSensitivity(v));
        SetSliderValues();
    }
    void SetSliderValues()
    {
        lookSensitivitySlider.value = ThirdPersonController.lookSensitivity;
    }
    public void ChangeLookSensitivity(float value)
    {
        ThirdPersonController.lookSensitivity = value;
    }
}
