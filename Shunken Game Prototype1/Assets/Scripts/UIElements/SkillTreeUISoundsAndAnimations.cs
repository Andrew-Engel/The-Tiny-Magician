using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


public class SkillTreeUISoundsAndAnimations : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
   AudioSource audioSource;
    [SerializeField] Button craftingMenuButton;
    [SerializeField] AudioClip skillUnlockedButtonSound, skillUnlockedSliderSound;
    [SerializeField] Color unlockedColor;
    [SerializeField] Slider transitionSlider;
    [SerializeField] GameObject explainationBox;
    private CanvasGroup explainationBoxCanvasGroup;

 

  
    private Button button;
    private Image buttonImage;
    private SkillTreeSystem skillTree;
    public string whatSpellDoesThisButtonUnlock;
    
  
    // Start is called before the first frame update
    void Start()
    {
        explainationBoxCanvasGroup = explainationBox.GetComponent<CanvasGroup>();
        
  
        audioSource = GetComponentInParent<AudioSource>();
        skillTree = GameObject.FindWithTag("GameManager").GetComponent<SkillTreeSystem>();
        
        skillTree.OnSpellUnlock += SkillTree_OnSpellUnlock;

        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
      //  if (transitionSlider != null) emptySlider = transitionSlider.gameObject;
        
    }
   
    private void SkillTree_OnSpellUnlock(object sender, SkillTreeSystem.OnSpellUnlockEventArgs e)
    {
        if (e.spellToBeUnlocked == whatSpellDoesThisButtonUnlock)
        {
            if (button.interactable)
            {
                
                audioSource.PlayOneShot(skillUnlockedButtonSound, 1f);
                button.interactable = false;
                button.enabled = false;
                buttonImage.color = unlockedColor;

                
                if (transitionSlider != null)
                {

                    SliderAnimation();
                }
                if (whatSpellDoesThisButtonUnlock == Spells.Crafting.ToString()) UnlockCraftingMenu();

            }
        }
        else if (e.nextSpell == whatSpellDoesThisButtonUnlock)
        {
            
           
            if (!button.interactable)
            {
                button.interactable = true;
            }
        }
    }
    private void SliderAnimation()
    {


        DOTween.To(() => transitionSlider.value, x => transitionSlider.value = x, 1f, 0.5f).SetUpdate(true);
        audioSource.PlayOneShot(skillUnlockedSliderSound, 1.0f);
        

    }
    private void UnlockCraftingMenu()
    {
        craftingMenuButton.interactable = true;
        Debug.Log("CraftingUnlocked");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        explainationBoxCanvasGroup.alpha = 0.01f;
        DOTween.To(() => explainationBoxCanvasGroup.alpha, x => explainationBoxCanvasGroup.alpha = x, 1f, 0.5f).SetUpdate(true);
        explainationBox.SetActive(true);
       
    }

    // Called when the pointer exits our GUI component.
  
    public void OnPointerExit(PointerEventData eventData)
    {
       
        explainationBox.SetActive(false);
        explainationBoxCanvasGroup.alpha = 0.01f;
    }
 
}
