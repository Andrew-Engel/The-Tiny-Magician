using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class EnemyBossBehavior : MonoBehaviour
{
    GameObject instantiatedHealthBar;
    [SerializeField] string enemyTitle;
    TextMeshProUGUI enemyHealthBarText;
    [SerializeField] GameObject healthBarPrefab;
    Transform healthBarParent;
    EnemyBehavior enemyBehavior;
    [SerializeField] EnemyAi enemyAi;
    int maxLives;
    Slider popUpHealthBarSlider;
    private void Awake()
    {
        enemyBehavior = GetComponent<EnemyBehavior>();
        maxLives = enemyBehavior.enemyLives;
        enemyBehavior.OnEnemyHealthChange += EnemyBehavior_OnEnemyHealthChange;
        enemyAi.onInitialAggro += EnemyAi_onInitialAggro;
    }

    private void EnemyAi_onInitialAggro(object sender, System.EventArgs e)
    {
        InstantiateHealthBar();
    }

    private void EnemyBehavior_OnEnemyHealthChange(object sender, EnemyBehavior.OnEnemyHealthChangeEventArgs e)
    {
        UpdateHealthBar(e.enemyHealth);
       
    }

    // Start is called before the first frame update
    void Start()
    {
       
        healthBarParent = GameObject.Find("BossHealthBarParent").GetComponent<Transform>();
    }

   void InstantiateHealthBar()
    {

        instantiatedHealthBar = Instantiate(healthBarPrefab, healthBarParent);
        popUpHealthBarSlider = instantiatedHealthBar.GetComponentInChildren<Slider>();
        enemyHealthBarText = instantiatedHealthBar.GetComponentInChildren<TextMeshProUGUI>();
        enemyHealthBarText.text = enemyTitle;
    }
    void UpdateHealthBar(float currentHealth)
    {
        if (currentHealth <= 0)
        {
            Destroy(instantiatedHealthBar);
        }
        else {
            float healthAmountNormalized = currentHealth / maxLives;
            DOTween.To(() => popUpHealthBarSlider.value, x => popUpHealthBarSlider.value = x, healthAmountNormalized, 0.2f);
        }
    }
}
