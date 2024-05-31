using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupPulse : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Vector2 alphaRange = new Vector2(0f, 1f);
    
    private void Awake()
    {
        Assert.IsNotNull(canvasGroup);
        Assert.IsTrue(fadeDuration > 0f);
        Assert.IsTrue(alphaRange.y >= alphaRange.x);

        canvasGroup.alpha = alphaRange.x;
        FadeIn();
    }

    void FadeIn()
    {
        canvasGroup.DOFade(alphaRange.y, fadeDuration).OnComplete(FadeOut);
    }

    void FadeOut()
    {
        canvasGroup.DOFade(alphaRange.x, fadeDuration).OnComplete(FadeIn);
    }
}
