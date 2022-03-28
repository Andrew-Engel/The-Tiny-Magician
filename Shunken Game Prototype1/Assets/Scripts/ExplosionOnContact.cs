using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ExplosionOnContact : MonoBehaviour
{
    public static int damage = 30;
    [SerializeField] AudioClip ImpactNoise;
    Cinemachine.CinemachineImpulseSource impulseSource;
    public bool isPuppemasterConfigured;
    public LayerMask interactableLayers;
    AudioSource _audio;
    public float explosionRadius;
    public float explosionForce;
    public float upwardsModifier;
    public ParticleSystem explosion;
    Rigidbody _rb;
    private NavMeshAgent agent;
   public Collider col;
    EnemyBehavior enemyHealth;
 
    public bool isExploding = false;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        if (isPuppemasterConfigured) _rb = GetComponentInParent<Rigidbody>();
        else _rb = GetComponent<Rigidbody>();

        _audio = GetComponent<AudioSource>();
       // col = GetComponentInChildren<Collider>();
        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
    }

    // Update is called once per frame
   
    void OnCollision (Collision collision)
    {
        if (collision.gameObject.layer == interactableLayers)
        {
            Impact();
        }
        
    }
    public void Impact()
    {
        impulseSource.GenerateImpulse(Camera.main.transform.forward);
        isExploding = true;
        Instantiate(explosion, this.transform.position, Quaternion.identity);
        _audio.PlayOneShot(ImpactNoise, 1f);
        Explosion();
    }

    private void Explosion()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, explosionRadius);
        
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy"))
            {

                Debug.Log($"Enemy hit: " + nearbyObject.gameObject.ToString());
                //    if (TryGetComponent<EnemyHitBox>(out EnemyHitBox component))
                //   {
                //     component.DamageAbsorbed("Bomb");
                //       Debug.Log("EnemyExplosionworks");
                //   }

                EnemyHitBox component = nearbyObject.GetComponent<EnemyHitBox>();
                    component.DamageAbsorbed("Bomb");


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

        if (isPuppemasterConfigured) Destroy(this.transform.parent.gameObject);
        else
        Destroy(this.gameObject);
        
    }
    public void AddRigidBody()
    {if (TryGetComponent<Rigidbody>(out Rigidbody rb) == false)
            this.gameObject.AddComponent<Rigidbody>();
        else rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
    
}
