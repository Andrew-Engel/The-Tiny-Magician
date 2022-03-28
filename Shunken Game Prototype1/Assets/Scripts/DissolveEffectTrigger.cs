using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DissolveEffectTrigger : MonoBehaviour
{
    private Renderer rend;
    private float dissolveFloat = 0;
    bool runtest;
    // Start is called before the first frame update
    void Awake()
    {
        DOTween.Init();
       if( gameObject.TryGetComponent(out Renderer renderer))
        {
            rend = renderer;
        }
       else
            rend = GetComponentInChildren<Renderer>();
        dissolveFloat = 0.2f;
        rend.material.SetFloat("_Dissolve", dissolveFloat);
    }
    private void Update()
    {
        if (runtest)
        {
            rend.material.SetFloat("_Dissolve", dissolveFloat);
            Debug.Log(rend.material.GetFloat("_Dissolve"));
        }
    }
    // Update is called once per frame
    public void DeathDissolve(float tweenTimer)
    {//make sure tween time is same as destroy timer in enembehaviour!
        runtest = true;
        DOTween.To(() => dissolveFloat, x => dissolveFloat = x, 1f, tweenTimer);
        Debug.Log("deathDissolve");
        /*
        do
        { rend.material.SetFloat("_Dissolve", dissolveFloat); }
        while (dissolveFloat < 1f);*/
    }
}
