using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Animations.Rigging;


public class Sneaking : MonoBehaviour
{
    [SerializeField] Animator animator;
    PlayerControls controls;
    [SerializeField] MultiAimConstraint ChestRig, headRig;
    public static bool playerSneaking = false;
    private bool isAnimatingToSneak = false;
    private bool isAnimatingToBase = false;
    private float baseLayerWeight, sneakLayerWeight;
    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Crouch.performed += context => Crouch();
        controls.Player.Melee.performed += context => MeleeSwitch();
    }
    void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    // Update is called once per frame
    public void Crouch()
    {
        if (!playerSneaking)
        {
            playerSneaking = true;
            isAnimatingToSneak = true;
            DOTween.To(() => baseLayerWeight, x => baseLayerWeight = x, 0f, 0.3f);
            DOTween.To(() => sneakLayerWeight, x => sneakLayerWeight = x, 1f, 0.3f);
        }
        else if (playerSneaking)
        {
            isAnimatingToBase = true;
            playerSneaking = false;
            DOTween.To(() => baseLayerWeight, x => baseLayerWeight = x, 1f, 0.3f);
            DOTween.To(() => sneakLayerWeight, x => sneakLayerWeight = x, 0f, 0.3f);
        }
        if (ChestRig.weight != 0)
        {
            ChestRig.weight = 0;
            headRig.weight = 0;
        }
    }
    void MeleeSwitch()
    {
        if (playerSneaking) Crouch();
    }
    private void Update()
    {
        if (isAnimatingToSneak || isAnimatingToBase)
        {
            animator.SetLayerWeight(0, baseLayerWeight);
            animator.SetLayerWeight(4, sneakLayerWeight);
            if (sneakLayerWeight == 1f && isAnimatingToSneak) isAnimatingToSneak = false;
            if (baseLayerWeight == 1f && isAnimatingToBase) isAnimatingToBase = false;
        }

    }
}
