using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionOnAwake : MonoBehaviour
{
    public float blastRadius, explosionForce, upwardsModifier;
    public float enemyFreezeTime;
    public bool freezeEnemy;
    public LayerMask ragdollLayer;
    public string damageType = "IceBlast";
    Cinemachine.CinemachineImpulseSource impulseSource;
    // Start is called before the first frame update
    void Start()
    {
        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        Explosion();
    }

    IEnumerator FreezeEnemies(Collider col)
    {
        EnemyAi _ai = col.GetComponentInParent<EnemyAi>();
      
        _ai.stayInPlace = true;
        yield return new WaitForSeconds(enemyFreezeTime);
     
            _ai.stayInPlace = false;
    }
    void Explosion()
    {
        impulseSource.GenerateImpulse(Camera.main.transform.forward);
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, blastRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy"))
            {
                EnemyHitBox component = nearbyObject.GetComponent<EnemyHitBox>();
                component.DamageAbsorbed(damageType);
                StartCoroutine(FreezeEnemies(nearbyObject));
            }
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null && rb.gameObject.layer!=ragdollLayer)
            {
                  if (rb.gameObject.tag != "Enemy")
                rb.AddExplosionForce(explosionForce, this.transform.position, blastRadius, upwardsModifier, ForceMode.Impulse);
                  /*
                if (rb.gameObject.tag == "Enemy")
                {
                    rb.isKinematic = false;
                }*/

            }
        }

    }
}
