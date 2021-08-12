using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWaveBehavior : MonoBehaviour
{
    Cinemachine.CinemachineImpulseSource impulseSource;
    // Start is called before the first frame update
    void Start()
    {
        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        impulseSource.GenerateImpulse(Camera.main.transform.forward);
        Debug.Log("Impulse");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
