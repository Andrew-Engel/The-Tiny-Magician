using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;
using StarterAssets;



public class StaminaBar : MonoBehaviour
{// Sprinting Check and depletion of stamina
    //InputControls
    PlayerControls controls;
    Vector2 move;
    public float sprintingStaminaRate;
    private float staminaSprintDepletionTimer = 0f;


    public float staminaRegenerationRate = 1f;
    float staminaRegenerationTimer = 0f;
    private int _stamina = 50;
    public int stamina
    {
        get { return _stamina; }
        set
        {
            _stamina = value;
            if (_stamina > maxStamina) _stamina = maxStamina;
            UpdateStamina();

        }
    }
    public static int absoluteMaxStamina = 150;
    public int maxStamina = 50;
    public event EventHandler <OnStaminaUseEventArgs> OnStaminaUse;
    public class OnStaminaUseEventArgs : EventArgs
    {
        public float staminaLevelNormalized;
    }
    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += context => move = context.ReadValue<Vector2>();

        controls.Player.Move.canceled += context => move = Vector2.zero;

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
    void Update()
    {
        if (_stamina < maxStamina)
        {
            RegenerateStamina();
        }
        if (move != Vector2.zero && ThirdPersonController.sprinting && _stamina >0)
        {
            staminaSprintDepletionTimer += (sprintingStaminaRate * Time.deltaTime);
            if (staminaSprintDepletionTimer >= 1f)
            {

                _stamina-= 2;
                UpdateStamina();
                staminaSprintDepletionTimer = 0f;
            }
        }
    }
    private void RegenerateStamina()
    {


        staminaRegenerationTimer += (staminaRegenerationRate * Time.deltaTime);
        if (staminaRegenerationTimer >= 1f)
        {

            _stamina++;
            UpdateStamina();
            staminaRegenerationTimer = 0f;
        }

    }
    public void UpdateStamina()
    {


        float staminaAmountNormalized = ((float)stamina / (float)absoluteMaxStamina);

        if (OnStaminaUse != null)
        {
            OnStaminaUse(this, new OnStaminaUseEventArgs { staminaLevelNormalized = staminaAmountNormalized });
        }
    }
}
