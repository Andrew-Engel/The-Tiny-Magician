using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUpTest : MonoBehaviour
{
    Transform playerSightTarget;
    // Start is called before the first frame update
    void Start()
    {
        playerSightTarget = GameObject.Find("playerSightTarget").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
       // Vector3 relativePos = playerSightTarget.position - this.transform.position;
      //  Quaternion rotation = Quaternion.LookRotation(relativePos);
       // Debug.DrawRay(this.transform.position, relativePos, Color.red);
    }
    private void OnParticleCollision(GameObject part)
    {

        switch (part.name)
        {
            case "IceLance(Clone)":
                int iceLanceDamageAmount;
                iceLanceDamageAmount = UnityEngine.Random.Range(MagicCasting.iceDamage - 5, MagicCasting.iceDamage + 5);
                bool isIceLanceCritical = UnityEngine.Random.Range(0, 100) < 30;
                if (isIceLanceCritical) iceLanceDamageAmount *= 2;


              

                DamagePopup.Create(this.transform.position, iceLanceDamageAmount, isIceLanceCritical);
                //healthbar.SetHealth(_lives);
                Debug.Log("ICELANCEHIT");

                break;
            case "EarthShatter(Clone)":

                int earthWaveDamageAmount;
                earthWaveDamageAmount = UnityEngine.Random.Range(MagicCasting.earthWaveDamage - 5, MagicCasting.earthWaveDamage + 5);
                bool isEarthWaveCritical = UnityEngine.Random.Range(0, 100) < 20;
                if (isEarthWaveCritical) earthWaveDamageAmount *= 2;


               

                DamagePopup.Create(this.transform.position, earthWaveDamageAmount, isEarthWaveCritical);
                //healthbar.SetHealth(_lives);
                Debug.Log("EARTHWAVE");
                break;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MeleeAttack")
        {
            DamagePopup.Create(this.transform.position, 10, false);
        }
    }
}
