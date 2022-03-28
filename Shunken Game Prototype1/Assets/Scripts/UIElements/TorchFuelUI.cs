using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TorchFuelUI : MonoBehaviour
{
     public Slider torchFuelMeter;
    public CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Awake()
    {
        torchFuelMeter = GameObject.Find("TorchFuelMeter").GetComponent<Slider>();
        canvasGroup = GameObject.Find("TorchFuelMeter").GetComponent<CanvasGroup>();
    }


    public void UpdateFuelMeter (float normalizedLevel)
    {
        if (canvasGroup == null) canvasGroup = GameObject.Find("TorchFuelMeter").GetComponent<CanvasGroup>();
        if (canvasGroup.alpha < 1f) canvasGroup.alpha = 1f;
        torchFuelMeter.value = normalizedLevel;
    }
    public void TurnOffMeter()
    {
        canvasGroup.alpha = 0f;
    }
}
