using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class LootBoxBehavior : MonoBehaviour
{
    bool disableDisplay = false;
    Animation anim;
    FindClosestLootBox findClosestLootBox;
    [SerializeField] float destroyDelay = 1f;
    [SerializeField] Transform parentTransform;
    [SerializeField] private LootBoxUIAndContents _lootBoxUI;
   [SerializeField] private CanvasGroup displayCanvasGroup;
   [SerializeField] private CanvasGroup interactPromptCanvasGroup;
    private bool mouseOver = false;
    private bool lootDisplayVisible = false;
    //InputControls
    PlayerControls controls;
    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Interact.performed += context => Interact();
    }
    void Start()
    {
        anim = GetComponent<Animation>();
        findClosestLootBox = GetComponentInParent<FindClosestLootBox>();
    }
    void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    void Interact()
    {
        findClosestLootBox.GetClosestLootBox();
        if (FindClosestLootBox.closestLootBoxTransform == parentTransform)
        {
            if (mouseOver && !lootDisplayVisible)
            {
                anim.Play("Open Box");
                DOTween.To(() => displayCanvasGroup.alpha, x => displayCanvasGroup.alpha = x, 1f, 0.3f);
                lootDisplayVisible = true;
            }
            else if (lootDisplayVisible && !mouseOver)
            {
                lootDisplayVisible = false;
                DOTween.To(() => displayCanvasGroup.alpha, x => displayCanvasGroup.alpha = x, 0f, 0.3f);

            }
            else if (lootDisplayVisible && mouseOver)
            {
               
                _lootBoxUI.CollectMaterials();
               
                if (_lootBoxUI.destroyObject)
                {
                    anim.Play("Close Box");
                    anim.PlayQueued("LootBoxDissolve");
                    Destroy(parentTransform.gameObject, destroyDelay);
                }
                else if (disableDisplay)
                {
                    DOTween.To(() => displayCanvasGroup.alpha, x => displayCanvasGroup.alpha = x, 0f, 0.3f);
                    lootDisplayVisible = false;
                    disableDisplay = false;
                }
                disableDisplay = true;
            }
        }
         else if (lootDisplayVisible)
            {
                lootDisplayVisible = false;
                DOTween.To(() => displayCanvasGroup.alpha, x => displayCanvasGroup.alpha = x, 0f, 0.3f);

            }
    }
 private void FadeAnimation()
    {
        anim.Play();
        Destroy(parentTransform.gameObject, destroyDelay);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mouseOver = true;
            DOTween.To(() => interactPromptCanvasGroup.alpha, x => interactPromptCanvasGroup.alpha = x, 1f, 0.3f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
      
        mouseOver = false;
        DOTween.To(() => interactPromptCanvasGroup.alpha, x => interactPromptCanvasGroup.alpha = x, 0f, 0.3f);
        if (lootDisplayVisible)
        { Interact(); }
        if (_lootBoxUI.destroyObject)
        {
            FadeAnimation();
        }
    }

}
