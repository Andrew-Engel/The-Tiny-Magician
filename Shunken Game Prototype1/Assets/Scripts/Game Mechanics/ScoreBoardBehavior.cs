using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoardBehavior : MonoBehaviour
{
    public CanvasGroup scoreBoardCanvasGroup;
    public int totalEnemies=8;
    public int Athreshold = 750;
    public int Bthreshold = 500;
    public int Cthreshold = 200;

   public static int daysSpent =1;
    int playerPoints;
    public static int enemiesKilled=0;
    GameBehavior gameManager;
    LevelSystem levelSystem;
    public TextMeshProUGUI daysSpentText;
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI playerGradeText,playerAchievementText;
    public TextMeshProUGUI finalLevelText;
    public TextMeshProUGUI playerPointText;
    PlayerControls controls;
    //Interpolating Scores on Games end
    bool interpolateScore = false;
    private int  playerPointsDisplayScore;
    // Start is called before the first frame update
    void Start()
    {
        
        gameManager = GetComponent<GameBehavior>();
        gameManager.OnPlayerDeath += GameManager_OnPlayerDeath;
        levelSystem = GetComponent<LevelSystem>();
    }
    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Interact.performed += context =>Continue();
        EnviroSkyMgr.instance.OnDayPassed += () =>
        {
            AddToDays();
        };

    }
    void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    private void GameManager_OnPlayerDeath(object sender, System.EventArgs e)
    {
        LoadStats();
        
    }
    private string PlayerGrading()
    {
        if (playerPoints >= Athreshold)
        { return "A"; }
        else if (playerPoints >= Bthreshold)
        { return "B"; }
        else if (playerPoints >= Cthreshold)
        { return "C"; }
        else return "D";
    }
    private string PlayerAchievement()
    {
        bool achievement1 = false;
        bool achievement2 = false;
        if (!GameBehavior.showLossScreen)
        {
            if (daysSpent < 2 )
            {
                achievement1 = true;
            }
            else if (enemiesKilled >= totalEnemies )
            {
                achievement2 = true;
            }
            if (achievement1 && !achievement2)
            {
                return "Speedrunner!\n(Complete the level in less than 2 days!)";
            }
            else if (!achievement1 && achievement2)
            { return "Demolition Man!\n(Eliminate all enemies)"; }
            else if (achievement2 && achievement1)
            {
                return "Demolition Man!" + "\n" + "(Eliminate all enemies)/nSpeedrunner!\n(Complete the level in less than 2 days!)";
            }
            else return "";
        }
        else
            return "Doofus!";
    }
    public void LoadStats()
    {
        finalLevelText.text = levelSystem.GetLevelNumber().ToString();
   
        daysSpentText.text = daysSpent.ToString();
        enemiesKilledText.text = enemiesKilled.ToString();
        ShowPlayerPoints();
        playerGradeText.text = PlayerGrading();
        playerAchievementText.text = PlayerAchievement();
        scoreBoardCanvasGroup.alpha = 1f;
       
       
    }
    void IncreasePointsGradually()
    {
       
        if (playerPointsDisplayScore < playerPoints)
        {
            playerPointsDisplayScore += 1;
            playerPointText.text = playerPointsDisplayScore.ToString();
        }
        else { interpolateScore = false; }
       
       
    }
   void ShowPlayerPoints()
    {
        playerPoints = PlayerPointCalculation();
        interpolateScore = true;

    }
    int PlayerPointCalculation()
    {
        int Score = 0;
        Score += (600 / daysSpent);
        Score += 80 * enemiesKilled;
        Score += 30 * levelSystem.GetLevelNumber();
        return Score;
    }
    private void Update()
    {
        if(interpolateScore)
        {
            IncreasePointsGradually();
        }
    }
    void Continue()
    {
        if (scoreBoardCanvasGroup.alpha ==1f)
        {
            scoreBoardCanvasGroup.alpha = 0f;
        }
    }
 void AddToDays()
    {
        daysSpent++;
    }
}
