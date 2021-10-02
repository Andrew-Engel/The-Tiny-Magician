using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    GrenadeThrowing grenade;
    // Start is called before the first frame update
    void Start()
    {
        grenade = GetComponentInParent<GrenadeThrowing>();
    }
    public void ThrowGrenadeEvent()
    {
        StartCoroutine(grenade.CallLaunchMethod());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
