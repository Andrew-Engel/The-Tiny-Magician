using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    [SerializeField] Vector3 rotationVector;

    // Update is called once per frame
 
      
              
 
    private void Update()
    {
        transform.Rotate(rotationVector * Time.unscaledDeltaTime);
    }
}
