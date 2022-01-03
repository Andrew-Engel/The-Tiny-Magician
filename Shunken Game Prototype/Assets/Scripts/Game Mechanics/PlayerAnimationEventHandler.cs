using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

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
   public void PlayWalkingSound()
    {
        if (ThirdPersonController.Grounded)
        AudioManager.instance.Play("Gravel Footsteps Slow");
    }
    public void PlayRunningSound()
    {
        if (ThirdPersonController.Grounded)
            AudioManager.instance.Play("Gravel Footsteps");
    }
 
}
