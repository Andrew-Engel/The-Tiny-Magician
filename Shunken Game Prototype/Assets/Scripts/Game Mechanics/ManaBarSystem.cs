using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ManaBarSystem : MonoBehaviour
{
    private MagicCasting playerMagic;
    public float manaRegenerationRate;
    float manaRegenerationTimer = 0f;
    private int _mana = 50;
    public int mana
    {
        get { return _mana; }
        set { _mana = value;
            if (_mana > maxMana) _mana = maxMana;
            UpdateMana();
        }
    }
    public static int maxMana = 50;
    public static int absoluteMaxMana = 150;
    public event EventHandler <OnManaUseEventArgs> OnManaUse;
    public class OnManaUseEventArgs : EventArgs
    {
        public float manaLevelNormalized;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerMagic = GameObject.Find("Player").GetComponent<MagicCasting>();
        UpdateMana();

    }

    // Update is called once per frame
    void Update()
    {
        if (mana < maxMana)
        {
            RegenerateMana();
        }
        if (mana == 0) playerMagic.StopMagic();
    }
    private void RegenerateMana()
    {
   
      
        manaRegenerationTimer += (manaRegenerationRate * Time.deltaTime);
        if (manaRegenerationTimer >= 1f)
        {
            
            mana++;
            UpdateMana();
            manaRegenerationTimer = 0f;
        }
       
    }
    public void UpdateMana()
    {

       

        float manaAmountNormalized = ((float)mana / (float)absoluteMaxMana);
 
        if (OnManaUse != null)
        {
          
            OnManaUse(this, new OnManaUseEventArgs { manaLevelNormalized = manaAmountNormalized });
        }
    }
  
}
