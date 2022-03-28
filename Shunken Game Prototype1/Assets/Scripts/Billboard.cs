using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
     private Transform cam;
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Transform>();
    }
    void LateUpdate()
    {
      transform.rotation = Quaternion.LookRotation((transform.position - cam.position).normalized);
    }
}
