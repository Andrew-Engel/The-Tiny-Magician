using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;
using Cinemachine;

public class IcePathSpells : MonoBehaviour
{
    public CinemachineVirtualCamera zoomedOutCam;
    public PuppetMaster puppetMaster;
    public LayerMask ragdollLayer;
    Cinemachine.CinemachineImpulseSource impulseSource;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Transform playerTransform;
    public static int iceBlastDamage=15;
    public LayerMask enemyLayer;
    public  float blastDuration;
    public float enemyAggroDelayTime;
    public float blastRadius,explosionForce,upwardsModifier;
    
    private void Start()
    {
        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
    }
    public IEnumerator IceBlast()
    {
        zoomedOutCam.Priority = 13;
        playerAnimator.SetTrigger("IceBlast");
        puppetMaster.mode= PuppetMaster.Mode.Kinematic;
       
        yield return new WaitForSeconds(blastDuration);
        /*Collider[] colliders = Physics.OverlapSphere(playerTransform.position, blastRadius);
        impulseSource.GenerateImpulse(Camera.main.transform.forward);
        
        
        foreach(Collider nearbyObject in colliders)
        {
            if(nearbyObject.CompareTag("Enemy"))
            {
                EnemyHitBox component = nearbyObject.GetComponent<EnemyHitBox>();
                component.DamageAbsorbed("IceBlast");
            }
            /*Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null && rb.gameObject.layer!=ragdollLayer)
            {
                //  if (rb.gameObject.tag == "Enemy")
                rb.AddExplosionForce(explosionForce, this.transform.position, blastRadius, upwardsModifier, ForceMode.Impulse);

                if (rb.gameObject.tag == "Enemy")
                {
                    rb.isKinematic = false;
                }

            }
        }*/
       
        puppetMaster.mode = PuppetMaster.Mode.Active;
    }
   
}
