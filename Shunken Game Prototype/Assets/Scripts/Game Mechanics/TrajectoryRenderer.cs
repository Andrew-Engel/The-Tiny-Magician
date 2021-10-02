using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//[RequireComponent(typeof(Rigidbody), typeof(LineRenderer))]
public class TrajectoryRenderer : MonoBehaviour
{
   // public LayerMask[] interactableLayers;
    GrenadeThrowing grenade;
    ExplosionOnContact explosion;
    Rigidbody rb;
    private Transform lookTarget;
    public Vector3 throwOffset;
    public float throwSpeed = 10f;
    private Vector3 localDirection;
   [SerializeField] Transform PlayerTransform; 
   // public Vector3 initialVelocity;
    LineRenderer lineRenderer;
    public bool grenadeImpact;

    private void Awake()
    {
        PlayerTransform = GameObject.Find("PlayerCameraRoot").GetComponent<Transform>();
    }
    void Start()
    {
        grenade = GameObject.Find("Player").GetComponent<GrenadeThrowing>();
        lookTarget = GameObject.Find("ThrowingTarget").GetComponent<Transform>();
        lineRenderer = GetComponent<LineRenderer>();
         rb = GetComponent<Rigidbody>();
        explosion = GetComponentInChildren<ExplosionOnContact>();
        //rb.isKinematic = true;
       
    }
 
    private Vector3 AdjustRealWorldVelocity()
    {
        //this is to adjust the real world velocity, for what if the player looks up and throws? the y component will be different than that listed. Try to omplement a cosine between this and a vector pointing up in world space.
        //Or yould make it a local vector3 (make this a child transform) that only has z component and use a transform.transform point or other matrix to get real world space velocity
        Vector3 worldDirection = PlayerTransform.forward *  throwSpeed;

        return worldDirection;
    }
    public void RenderTrajectory()
    {
        
        Debug.Log("RenderingTrajectory");
        //Time of flight calculation
        transform.LookAt(lookTarget);

       Vector3 initialVelocity = (transform.forward * grenade.throwPower);
        Debug.Log(initialVelocity);
     //  initialVelocity =Vector3.forward * throwSpeed + throwOffset;
        Debug.DrawRay(this.transform.position, initialVelocity, Color.blue);
        float t;
        t = (-1f * initialVelocity.y) / Physics.gravity.y;
        t = 2f * t;

        //Trajectory calculation

        lineRenderer.positionCount = 350;
        Vector3 trajectoryPoint;

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            //
            float time = t * i / (float)(lineRenderer.positionCount);
            trajectoryPoint = grenade.releasePoint.position + initialVelocity * time + 0.5f * Physics.gravity * time * time;
            lineRenderer.SetPosition(i, trajectoryPoint);
        }
    }
    public void LaunchProjectile()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        // rb.transform.parent = null;
        // Vector3 flightVector = transform.TransformDirection(initialVelocity);
        rb.velocity = Vector3.zero;
        transform.LookAt(lookTarget);
        //rb.velocity =transform.forward * grenade.throwPower;
        rb.AddForce(transform.forward * grenade.throwPower);
       
    }
    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Collision detected");
        lineRenderer.positionCount = 0;
        //explosion.AddRigidBody();
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Collidable")  || collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            grenadeImpact = true;

            explosion.Impact();
        }
        /*
        foreach (LayerMask _LM in interactableLayers)
        {
            Debug.Log($"Collided layers mayne: " + LayerMask.LayerToName(collision.gameObject.layer).ToString());
            Debug.Log($"Collidable Layers: " + LayerMask.LayerToName(_LM).ToString());
            if (collision.gameObject.layer == _LM)
            {
               
                grenadeImpact = true;
               
                explosion.Impact();
            }
        }*/
       
    }


}
