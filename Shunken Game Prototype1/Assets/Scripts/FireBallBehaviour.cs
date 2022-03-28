using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallBehaviour : MonoBehaviour
{
    Cinemachine.CinemachineImpulseSource impulseSource;
    private Collider _col;
        [SerializeField] ParticleSystem impactEffect;
    [SerializeField] float damageRadius;
 
    [SerializeField] AudioClip explosionSound;
    // Start is called before the first frame update
    void Start()
    {
        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        _col = GetComponent<Collider>();
    }
void OnCollisionEnter (Collision collision)
    {
        Instantiate(impactEffect, this.transform.position, Quaternion.identity);
        impulseSource.GenerateImpulse(Camera.main.transform.forward);
        ApplyDamage();
        Destroy(this.gameObject, 0.1f);
    }
    public void ApplyDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, damageRadius);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.tag == "Enemy")
            {

                EnemyHitBox enemyHitBox = nearbyObject.GetComponent<EnemyHitBox>();
                enemyHitBox.DamageAbsorbed("FireBall");

            }
        }
    }
}
