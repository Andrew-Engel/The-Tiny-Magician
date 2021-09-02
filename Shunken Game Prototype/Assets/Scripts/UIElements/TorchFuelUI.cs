using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TorchFuelUI : MonoBehaviour
{
     Slider torchFuelMeter;
    CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        torchFuelMeter = GameObject.Find("TorchFuelMeter").GetComponent<Slider>();
        canvasGroup = GameObject.Find("TorchFuelMeter").GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateFuelMeter (float normalizedLevel)
    {
        if (canvasGroup.alpha < 1f) canvasGroup.alpha = 1f;
        torchFuelMeter.value = normalizedLevel;
    }
    public void TurnOffMeter()
    {
        canvasGroup.alpha = 0f;
    }
}
