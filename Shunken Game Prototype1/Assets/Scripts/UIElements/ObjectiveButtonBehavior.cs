using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveButtonBehavior : MonoBehaviour
{
    TextMeshProUGUI descriptionText;
    public  string descriptionString;
    // Start is called before the first frame update
    void Start()
    {
        descriptionText = GameObject.Find("Mission description").GetComponent<TextMeshProUGUI>();
    }
    public void SetDescription()
    {
        descriptionText.text = descriptionString;
    }
}
