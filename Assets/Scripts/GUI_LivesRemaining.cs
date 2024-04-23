using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GUI_LivesRemaining : MonoBehaviour
{
    

    [Header("Hearts")]
    [SerializeField] private Transform heartToggleParent;
    [SerializeField] private Toggle heartTogglePrefab;
    [SerializeField] private List<Toggle> hearts;
    
    [Header("Canvas Group Properties")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float delayAfterFadeIn = 3f;
    private void Start()
    {
        canvasGroup.alpha = 0;
    }
    
    public void InitializeLives(int liveCount)
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

    private void FadeIn()
    {
        canvasGroup.DOFade(1, fadeDuration);
    }

    private void FadeOut()
    {
        canvasGroup.DOFade(0, fadeDuration);
    }
}
