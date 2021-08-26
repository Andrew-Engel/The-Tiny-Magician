using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;

public class TogglePuppetMaster : MonoBehaviour
{
    public PuppetMaster puppetMaster;
    [SerializeField] LayerMask collidableLayers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == collidableLayers)
        {
            puppetMaster.mode = PuppetMaster.Mode.Active;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == collidableLayers)
        {
            puppetMaster.mode = PuppetMaster.Mode.Kinematic;
        }
    }
}
