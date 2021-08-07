using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExplosionOnContact : MonoBehaviour
{
    public float explosionRadius;
    public float explosionForce;
    public float upwardsModifier;
    public ParticleSystem explosion;
    Rigidbody _rb;
    private NavMeshAgent agent;
    
    EnemyBehavior enemyHealth;
 
    public bool isExploding = false;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter (Collision collision)
    {
        isExploding = true;
        Instantiate(explosion, this.transform.position, Quaternion.identity);
         FindObjectOfType<AudioManager>().Play("BombArrowExplosion");
       Explosion();
        
    }
    private void Explosion()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, explosionRadius);
        
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.tag == "Enemy")
            {
                
                
                agent = nearbyObject.gameObject.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.enabled = false;
                 //  int lives = nearbyObject.gameObject.GetComponent<EnemyBehavior>().enemyLives;
                   // lives -= 2;
                }
                
               
                
                
            }
           
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
             //  if (rb.gameObject.tag == "Enemy")
                rb.AddExplosionForce(explosionForce, this.transform.position, explosionRadius, upwardsModifier, ForceMode.Impulse);
                
                if (rb.gameObject.tag == "Enemy")
               {
                    rb.isKinematic = false;
                                   }

              }
        }


        Destroy(this.gameObject);
        
    }

    
}
