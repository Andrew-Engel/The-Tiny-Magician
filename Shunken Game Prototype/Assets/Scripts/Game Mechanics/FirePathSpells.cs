using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class FirePathSpells : MonoBehaviour
{
   
    MagicCasting magicCasting;
    private void Start()
    {
        magicCasting = GetComponent<MagicCasting>();
    }
    public void Fireball()
    {
        DOTween.To(() => magicCasting.throwingRig.weight, x => magicCasting.throwingRig.weight = x, 1f, 0.3f);
        MagicCasting.casting = true;
        MagicCasting.castingAimedSpells = true;
        magicCasting.aimCam.Priority = 11;
        magicCasting.fireBallInHand.SetActive(true);
        StartCoroutine(magicCasting.ThrowFireBall());
    }
    public void EarthWave()
    {

    }
    public void FlameThrower()
    {

    }

}
