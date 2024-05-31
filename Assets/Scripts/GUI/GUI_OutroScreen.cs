using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Assertions;

public class GUI_OutroScreen : MonoBehaviour
{
    [SerializeField] private InvisibleMinesGameManager mainGameManager;
    [SerializeField] private InteractableUnityEventWrapper startGameButtonEventWrapper;
    
    [Header("Transition Config")]
    [SerializeField] private float scaleDuration = 0.25f;

    [SerializeField, ReadOnly] private Vector3 originalScale;
    
    private void Awake()
    {
        Assert.IsNotNull(mainGameManager);
        Assert.IsNotNull(startGameButtonEventWrapper);
        
        startGameButtonEventWrapper.WhenUnselect.AddListener(() =>
        {
            transform.DOScale(Vector3.zero, scaleDuration).OnComplete(() =>
            {
                mainGameManager.StartGame();
                gameObject.SetActive(false);
            });
        });
        mainGameManager.onGameOver += () =>
        {
            gameObject.SetActive(true);
            transform.DOScale(originalScale, scaleDuration);
        };
    }

    private void Start()
    {
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    [Button]
    private void ClickButton()
    {
        startGameButtonEventWrapper.WhenUnselect.Invoke();
    }
}
