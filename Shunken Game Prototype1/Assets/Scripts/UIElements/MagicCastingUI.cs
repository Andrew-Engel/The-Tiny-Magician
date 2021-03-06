using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MagicCastingUI : MonoBehaviour
{
    public ManaBarSystem _manaBar;
    private MagicCasting magicCasting;
    [SerializeField] RectTransform currentSpellIconTransform, nextSpellIconTransform;
    private Vector3 nextSpellIconOriginalPosition;
    [SerializeField] private Image manaBar;
    [SerializeField] CanvasGroup  noSpellIcon,fireBallIcon, flameThrowerIcon, earthShatterIcon, iceLanceIcon, airEscapeIcon,airDashicon, iceBlastIcon;
    [SerializeField] CanvasGroup noSpellIcon2, fireBallIcon2, flameThrowerIcon2, earthShatterIcon2, iceLanceIcon2, airEscapeIcon2, airDashicon2,iceBlastIcon2;
    [SerializeField] AudioClip fireBallSound, flameThrowerSound, earthShatterSound, iceLanceSound, windSound, noSpellSound;
    private AudioSource audioSource;
    private CanvasGroup currentIcon, nextIcon;
    Dictionary<string, CanvasGroup> spellIcons = new Dictionary<string, CanvasGroup>();
    Dictionary<string, CanvasGroup> nextSpellIcons = new Dictionary<string, CanvasGroup>();
    Dictionary<string, AudioClip> spellSounds = new Dictionary<string, AudioClip>();
   
    // Start is called before the first frame update
    void Start()
    {
        _manaBar = GetComponent<ManaBarSystem>();
        _manaBar.OnManaUse += ManaBar_OnManaUse;
        nextSpellIconOriginalPosition = nextSpellIconTransform.position;
        audioSource = GetComponent<AudioSource>();
        magicCasting = GameObject.Find("Player").GetComponent<MagicCasting>();
        magicCasting.OnSpellChange += MagicCasting_OnSpellChange;
        SetUpIconDictionary();
        SetUpSoundDictionary();
       
        currentIcon = noSpellIcon;
        SetUpIconDictionary2();
    }
    private void SetUpIconDictionary()
    {
        spellIcons.Add(Spells.FireBall.ToString(), fireBallIcon);
        spellIcons.Add(Spells.Nothing.ToString(), noSpellIcon);
        spellIcons.Add(Spells.IceShards.ToString(), iceLanceIcon);
        spellIcons.Add(Spells.FlameThrower.ToString(), flameThrowerIcon);
        spellIcons.Add(Spells.EarthWave.ToString(), earthShatterIcon);
        spellIcons.Add(Spells.AirEscape.ToString(), airEscapeIcon);
        spellIcons.Add(Spells.AirDash.ToString(), airDashicon);
        spellIcons.Add(Spells.IceBlast.ToString(), iceBlastIcon);
    }
    private void SetUpIconDictionary2()
    {
        nextSpellIcons.Add(Spells.FireBall.ToString(), fireBallIcon2);
        nextSpellIcons.Add(Spells.Nothing.ToString(), noSpellIcon2);
        nextSpellIcons.Add(Spells.IceShards.ToString(), iceLanceIcon2);
        nextSpellIcons.Add(Spells.FlameThrower.ToString(), flameThrowerIcon2);
        nextSpellIcons.Add(Spells.EarthWave.ToString(), earthShatterIcon2);
        nextSpellIcons.Add(Spells.AirEscape.ToString(), airEscapeIcon2);
        nextSpellIcons.Add(Spells.AirDash.ToString(), airDashicon2);
        nextSpellIcons.Add(Spells.IceBlast.ToString(), iceBlastIcon2);
    }
    private void SetUpSoundDictionary()
    {
        spellSounds.Add(Spells.FireBall.ToString(), fireBallSound);
        spellSounds.Add(Spells.Nothing.ToString(), noSpellSound);
        spellSounds.Add(Spells.IceShards.ToString(), iceLanceSound);
        spellSounds.Add(Spells.IceBlast.ToString(), iceLanceSound);
        spellSounds.Add(Spells.FlameThrower.ToString(), flameThrowerSound);
        spellSounds.Add(Spells.EarthWave.ToString(), earthShatterSound);
        spellSounds.Add(Spells.AirEscape.ToString(), windSound);
        spellSounds.Add(Spells.AirDash.ToString(), windSound);
    }
    public void UpdateManaAnimated(float manaAmount, float tweenTime)
    {
        Debug.Log("UpdateManaAnimated");
        DOTween.To(() => manaBar.fillAmount, x => manaBar.fillAmount = x, manaAmount, tweenTime);
    }
    private void SwitchSpellIcon(string currentSpell, string nextSpell)
    {
        StartCoroutine(SpellIconChangeAnimation(nextSpell, currentSpell));
        CanvasGroup iconToSwitchTo;
   
        spellIcons.TryGetValue(currentSpell, out iconToSwitchTo);
          //  DOTween.To(() => currentIcon.alpha, x => currentIcon.alpha = x, 0f, 0.2f).SetUpdate(true);
        currentIcon.alpha = 0f;
        iconToSwitchTo.alpha = 1.0f;
        // DOTween.To(() => iconToSwitchTo.alpha, x => iconToSwitchTo.alpha = x, 1f, 0.2f).SetUpdate(true);
        currentIcon =iconToSwitchTo;
    }
    private void PlaySpellSound(string currentSpell)
    {
        AudioClip clip;
        spellSounds.TryGetValue(currentSpell, out clip);
        audioSource.PlayOneShot(clip, 1f);
    }
    private void MagicCasting_OnSpellChange(object sender, MagicCasting.OnSpellChangeEventArgs e)
    {
        SwitchSpellIcon(e.currentSpell, e.nextSpell);
        PlaySpellSound(e.currentSpell);
        DoubleCheckNextSpellIcon(e.nextSpell);

    }
    private void ManaBar_OnManaUse(object sender, ManaBarSystem.OnManaUseEventArgs e)
    {
        
        UpdateManaAnimated(e.manaLevelNormalized, 0.5f);
    }
    private IEnumerator SpellIconChangeAnimation(string nextSpell, string currentSpell)
    {
        CanvasGroup iconToTurnOff, iconToTurnOn;
        nextSpellIcons.TryGetValue(currentSpell, out iconToTurnOff);
        
        nextSpellIcons.TryGetValue(nextSpell, out iconToTurnOn);
 
        float animationTime = 0.5f;
        Vector3 targetPosition = currentSpellIconTransform.position;
        nextSpellIconTransform.DOAnchorPos3D(targetPosition, animationTime, false);
        DOTween.To(() => nextSpellIconTransform.localScale, x => nextSpellIconTransform.localScale = x, new Vector3 (1.5f,1.5f,1f), animationTime);
        
        yield return new WaitForSeconds(animationTime);
      
        iconToTurnOff.alpha = 0f;


        nextSpellIconTransform.position = nextSpellIconOriginalPosition;


        
        
        DOTween.To(() => iconToTurnOn.alpha, x => iconToTurnOn.alpha = x,1f, 1f);
     
    }
    private void DoubleCheckNextSpellIcon(string nextSpell)
    {
       foreach (var key in nextSpellIcons)
        {
            if (key.Key != nextSpell)
            {
                nextSpellIcons[key.Key].alpha = 0f;
              
            }
        }
    }
}
