using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class GUI_LivesRemaining : MonoBehaviour
{
    [SerializeField] private bool hideOnStart;
    [SerializeField] private InvisibleMinesGameManager mainGameManager;
    
    [Header("Hearts")]
    [SerializeField] private Transform heartToggleParent;
    [SerializeField] private Toggle heartTogglePrefab;
    [SerializeField] private List<Toggle> hearts;
    
    [Header("Canvas Group Properties")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float delayAfterFadeIn = 3f;

    private void Awake()
    {
        Assert.IsNotNull(mainGameManager);
        
        Assert.IsNotNull(heartToggleParent);
        Assert.IsNotNull(heartTogglePrefab);

        Assert.IsNotNull(canvasGroup);
        
        if (hideOnStart)
            canvasGroup.alpha = 0;
        
        mainGameManager.onLivesInitialised += InitializeLives;

        mainGameManager.onRemainingLivesUpdated += UpdateHearts;
    }
    
    
    private void InitializeLives(int liveCount)
    {
        ClearHearts();
        for (int i = 0; i < liveCount; i++)
        {
            hearts.Add(Instantiate(heartTogglePrefab, heartToggleParent));
        }
        UpdateHearts(liveCount);
    }

    void ClearHearts()
    {
        foreach (var heart in hearts)
        {
            Destroy(heart.gameObject);
        }
        hearts.Clear();
    }
    
    public void ShowLivesRemaining(int noOfLivesRemaining)
    {
        UpdateHearts(noOfLivesRemaining);
        FadeIn();
        Invoke(nameof(FadeOut), delayAfterFadeIn);
    }

    private void UpdateHearts(int noOfLivesRemaining)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].isOn = noOfLivesRemaining > i;
        }
    }

    public void FadeInTemporarily()
    {
        FadeIn();
        Invoke(nameof(FadeOut), delayAfterFadeIn);
    }
    
    private void FadeIn()
    {
        canvasGroup.DOFade(1, fadeDuration);
    }

    private void FadeOut()
    {
        canvasGroup.DOFade(0, fadeDuration);
    }
}
