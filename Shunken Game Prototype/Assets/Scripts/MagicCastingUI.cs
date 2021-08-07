using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MagicCastingUI : MonoBehaviour
{
    private MagicCasting magicCasting;
     
    [SerializeField] private Image manaBar;
    [SerializeField] CanvasGroup  noSpellIcon,fireBallIcon, flameThrowerIcon, earthShatterIcon, iceLanceIcon;
    [SerializeField] AudioClip fireBallSound, flameThrowerSound, earthShatterSound, iceLanceSound, noSpellSound;
    private AudioSource audioSource;
        private CanvasGroup currentIcon;
    Dictionary<string, CanvasGroup> spellIcons = new Dictionary<string, CanvasGroup>();
    Dictionary<string, AudioClip> spellSounds = new Dictionary<string, AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        ManaBarSystem _manaBar = GetComponent<ManaBarSystem>();
        _manaBar.OnManaUse += ManaBar_OnManaUse;
        audioSource = GetComponent<AudioSource>();
        magicCasting = GameObject.Find("Player").GetComponent<MagicCasting>();
        magicCasting.OnSpellChange += MagicCasting_OnSpellChange;
        SetUpIconDictionary();
        SetUpSoundDictionary();
        currentIcon = noSpellIcon;
    }
    private void SetUpIconDictionary()
    {
        spellIcons.Add(Spells.FireBall.ToString(), fireBallIcon);
        spellIcons.Add(Spells.Nothing.ToString(), noSpellIcon);
        spellIcons.Add(Spells.IceShards.ToString(), iceLanceIcon);
        spellIcons.Add(Spells.FlameThrower.ToString(), flameThrowerIcon);
        spellIcons.Add(Spells.EarthWave.ToString(), earthShatterIcon);

    }
    private void SetUpSoundDictionary()
    {
        spellSounds.Add(Spells.FireBall.ToString(), fireBallSound);
        spellSounds.Add(Spells.Nothing.ToString(), noSpellSound);
        spellSounds.Add(Spells.IceShards.ToString(), iceLanceSound);
        spellSounds.Add(Spells.FlameThrower.ToString(), flameThrowerSound);
        spellSounds.Add(Spells.EarthWave.ToString(), earthShatterSound);
    }
    public void UpdateManaAnimated(float manaAmount, float tweenTime)
    {
     
        DOTween.To(() => manaBar.fillAmount, x => manaBar.fillAmount = x, manaAmount, tweenTime);
    }
    private void SwitchSpellIcon(string currentSpell)
    {
        
        CanvasGroup iconToSwitchTo;
        Debug.Log($"IconToSwitchTo: " + currentSpell);
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
        SwitchSpellIcon(e.currentSpell);
        PlaySpellSound(e.currentSpell);

    }
    private void ManaBar_OnManaUse(object sender, ManaBarSystem.OnManaUseEventArgs e)
    {
        UpdateManaAnimated(e.manaLevelNormalized, 0.5f);
    }

}
