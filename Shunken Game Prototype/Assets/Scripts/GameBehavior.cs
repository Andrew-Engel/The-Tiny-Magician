using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomExtensions;
using RootMotion.Dynamics;
using System;
using DG.Tweening;

public class GameBehavior : MonoBehaviour //Imanager
{ Transform screenCanvasTransform;
    public PuppetMaster puppet;
    public event EventHandler OnPlayerDeath;
    public event EventHandler<OnHealthChangeEventArgs> OnHealthChange;
    public class OnHealthChangeEventArgs : EventArgs
    {
        public float healthLevelNormalized;
    }

    public string backgroundMusicTitle;
    public GameObject lossScreen, hud;
    
    PlayerBehavior player;

    
    public bool showWinScreen = false;
    public static bool showLossScreen = false;
   
    public int maxItems = 4;
   
    Animator animator;

    private int _itemsCollected = 0;
    public int _playerHP = 20;
    public static int maxPlayerHP = 20;
   public static int absoluteMaxHP = 42;
    //Cam Sensitivity
    //public float camSensitivity;
    private AudioManager audioManager;
    //health bar
    public HealthBar healthBar;
    // CentipedeEscapeStuff
    public int buttonTapCount;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start ()
    {
        hud = GameObject.Find("PlayTime HUD");
        screenCanvasTransform = GameObject.Find("Screen Canvas").GetComponent<Transform>();
        puppet = GameObject.Find("PuppetMaster").GetComponent<PuppetMaster>();
        buttonTapCount = 0;
       // Cursor.visible = false;
        player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
    
        animator = GameObject.Find("Player").GetComponent<Animator>();
         FindObjectOfType<AudioManager>().Play(backgroundMusicTitle);
        SendHealthToHealthBar(_playerHP);
    }
    

    public int Items
    {
        get { return _itemsCollected; }
        set
        {
            _itemsCollected = value;
            Debug.LogFormat("Items: {0}", _itemsCollected);
         

        }
    }
    private void EndOfGame()
    {
        hud.SetActive(false);
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, 3.5f).SetUpdate(true);
        Instantiate(lossScreen, screenCanvasTransform);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public int HP
    {
        get { return _playerHP; }
        set
        {
            //healthBar.SetHealth(_playerHP);
            _playerHP = value;
            Debug.LogFormat("Lives: {0}", _playerHP);
            SendHealthToHealthBar(_playerHP);
            if (_playerHP > maxPlayerHP)
            {
                _playerHP = maxPlayerHP;
            }
            if (_playerHP <= 0)
            {


                puppet.Kill();
                showLossScreen = true;

                Debug.Log("PLayerDeath");
                if (!showLossScreen)
                AudioManager.instance.Play("PlayerDeath");

                EndOfGame();
                
                
                
            }
            
        }

    }
   
    void OnGUI()
    {


        if (showLossScreen)
        {
            Cursor.visible = true;
           
        }
    }
    public void SendHealthToHealthBar(int currentLives)
    {
        float normalizedValue = (float)currentLives / (float)absoluteMaxHP;
        Debug.Log(normalizedValue);
        OnHealthChange(this, new OnHealthChangeEventArgs { healthLevelNormalized = normalizedValue });
    }
}

