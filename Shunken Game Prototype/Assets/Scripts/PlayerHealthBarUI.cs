using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    private GameBehavior gameManager;
    [SerializeField] Image healthBar;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameBehavior>();
        gameManager.OnHealthChange += OnHealthChange;
    }
private void OnHealthChange(object sender, GameBehavior.OnHealthChangeEventArgs e)
    {

        DOTween.To(() => healthBar.fillAmount, x => healthBar.fillAmount = x, e.healthLevelNormalized, 0.5f);
    }
}
