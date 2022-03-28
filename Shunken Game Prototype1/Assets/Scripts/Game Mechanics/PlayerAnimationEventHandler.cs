using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Cinemachine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    public CinemachineVirtualCamera zoomedOutCam;
    public GameObject icePlastParticleSystem;
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
    public void InstantiateIceBlastParticleSystem()
    {
        Instantiate(icePlastParticleSystem, this.transform.position, Quaternion.identity);
    }
    public void HideZoomCam()
    {
        zoomedOutCam.Priority = 7;
    }
}
