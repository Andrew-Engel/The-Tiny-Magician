using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInPlace : MonoBehaviour
{
    public static bool stayInPlace = false;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static IEnumerator StandStill(float stayDuration, bool isMagic)
    {
        stayInPlace = true;
        
        yield return new WaitForSeconds(stayDuration);

        stayInPlace = false;
    }
}
