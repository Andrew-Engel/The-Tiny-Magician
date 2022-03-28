using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectAnimationEventHandler : MonoBehaviour
{
    Cinemachine.CinemachineImpulseSource impulseSource;
    public string enemytype;
    public AntAttackSequence antAttackSequence;
    public CentipedeAttackSequence centipedeAttackSequence;
    public GrasshopperAttackSequence grasshopperAttackSequence;
    public EnemyAi enemyAI;
    public float foostepSoundLevel = 1f, attackVolumeLevel=2f;
    AudioSource _audio;
    public AudioClip attackSound,threatenSound,damageSound,deathSound,footStepSound,additionalSound1,additionalSound2;
    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

   public void AttackSoundFX()
    {
        _audio.PlayOneShot(attackSound, attackVolumeLevel);
    }
    public void ThreatenSoundFX()
    {
        _audio.PlayOneShot(threatenSound);
    }
    public void FootStepSoundFX()
    {
        if (enemyAI.isMoving)
        { _audio.PlayOneShot(footStepSound, foostepSoundLevel); }
    }
    public void DeathSoundFX()
    {
        _audio.PlayOneShot(deathSound);
    }
    public void DamageSoundFX()
    {
        _audio.PlayOneShot(damageSound);
    }
    public void AdditionalSoundFX1()
    {
        _audio.PlayOneShot(additionalSound1);
    }
    public void AdditionalSoundFX2()
    {
        _audio.PlayOneShot(additionalSound2);
    }
    public void SendCMImpulse()
    {
        switch (enemytype)
        {
            case "Ant":
                {
                    if (antAttackSequence.playerInRange)
                    {
                        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
                        impulseSource.GenerateImpulse(Camera.main.transform.forward);
                    }
                }
                break;
            case "Centipede":
                {
                    if (centipedeAttackSequence.playerInRange)
                    {
                        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
                        impulseSource.GenerateImpulse(Camera.main.transform.forward);
                    }
                }
                break;
            case "Grasshopper":
                {
                    if (grasshopperAttackSequence.playerWithinAttackRange)
                    {
                        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
                        impulseSource.GenerateImpulse(Camera.main.transform.forward);
                    }
                }
                break;
        }

         
    }
}
