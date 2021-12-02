using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeInThenOut : MonoBehaviour
{
    [SerializeField] float fadeInTime, fadeOutTime, pauseInMiddle;
    CanvasGroup canvasGroup;
    RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(FadeInAndOut());
        rectTransform = GetComponent<RectTransform>();
        Destroy(this.gameObject, fadeInTime + fadeOutTime + pauseInMiddle);
    }

   
    IEnumerator FadeInAndOut()
    {
        this.transform.DOScale(1f, fadeInTime);
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1, fadeInTime).SetUpdate(true);
        yield return new WaitForSeconds(pauseInMiddle);
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0, fadeOutTime).SetUpdate(true);
    }
}
