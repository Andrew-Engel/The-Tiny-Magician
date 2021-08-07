using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float onscreenDelay = 3f;
   
   
    // Start is called before the first frame update
    void Start()
    {

        Destroy(this.gameObject, onscreenDelay);
    }
    /*void OnCollisionEnter(Collision impact)
    {

        Debug.Log("It works!");
            Instantiate(explosion, this.transform.position, Quaternion.identity);
        
    }*/
}
