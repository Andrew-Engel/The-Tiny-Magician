using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
  //  public static Transform pfDamagePopup;
    public static DamagePopup Create(Vector3 position , int damageAmount, bool isCriticalHit)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position , Quaternion.identity);
     //   Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, popupParent, true );
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.SetUp(damageAmount, isCriticalHit);

        return damagePopup;

    }
    private static int sortingOrder;
    private const float disappearTimerMax = 1f;
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
   [SerializeField] private Color normalTextColor;
    [SerializeField] private Color criticalTextColor;
    private Vector3 moveVector;


    void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
     
    }

   public void SetUp (int damageAmount, bool isCriticalHit)
    {
        textMesh.SetText(damageAmount.ToString());
        if (!isCriticalHit)
        { textMesh.fontSize = 36;
           // textColor = new Color(250, 245, 0, 255);
            
            textMesh.color = normalTextColor;
        }
       else
        {
            //Critical hit
            textMesh.fontSize = 45;
           // textColor = new Color(250, 83, 0, 255);
            textMesh.color = criticalTextColor;
        }
         
        disappearTimer = disappearTimerMax;
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        moveVector = new Vector3(1, 1,0) * 30f;
    }
    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;
        if (disappearTimer > disappearTimerMax * 0.5f)
        {
            //first half of popup lifetime
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            //second half of popuplifetime
            if (Mathf.Sign(transform.localScale.x)>0)
            {
                float decreaseScaleAmount = 1f;
                transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
            }
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer <0)
        {
            //start disappearing
            float disapperSpeed = 3f;
            textColor.a -= disapperSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a <0)
            {
                Destroy(this.gameObject);
            }
        }

    }
}
