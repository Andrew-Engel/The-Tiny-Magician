using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DemoObjective : MonoBehaviour
{
    public static int fragmentsCollected = 0;
    TextMeshProUGUI objectiveText;

    // Start is called before the first frame update
    void Start()
    {
        objectiveText = GameObject.Find("Objectives Text").GetComponent<TextMeshProUGUI>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
