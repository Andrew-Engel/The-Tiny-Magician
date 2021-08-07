using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraAim : MonoBehaviour
{
    public GameObject mainCamera, aimCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetAxis("Fire1") ==1f)
        {
            mainCamera.SetActive(false);
            aimCamera.SetActive(true);

           
        }
        else if (Input.GetAxis("Fire1") != 1f)
        {
            mainCamera.SetActive(true);
            aimCamera.SetActive(false);
        }
    }
}
