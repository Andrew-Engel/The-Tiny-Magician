using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
public class MagicCastingCameraEffects : MonoBehaviour
{
    private MagicCasting playerMagicCasting;
    [SerializeField] CinemachineVirtualCamera shakyCam;
    private bool effectsCameraActive = false;
    // Start is called before the first frame update
    void Start()
    {
        playerMagicCasting = GameObject.Find("Player").GetComponent<MagicCasting>();
        playerMagicCasting.OnSpellUseAndCancel += OnSpellUseAndCancel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnSpellUseAndCancel(object sender, MagicCasting.OnSpellUseAndCancelEventArgs e)
    {
        
        switch (e.currentSpell)
        {
            case ("FireBall"):
                if (!effectsCameraActive)
                {
                    shakyCam.Priority = 12;
                  
                    effectsCameraActive = true;
                 }
                else
                {
                    shakyCam.Priority = 8;
                    
                    effectsCameraActive = false;
                }
                break;
         }
    }
}
