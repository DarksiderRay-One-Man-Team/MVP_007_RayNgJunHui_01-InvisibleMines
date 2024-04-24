using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Assertions;

public class GUI_IntroScreen : MonoBehaviour
{
    [SerializeField] private InvisibleMinesGameManager mainGameManager;
    [SerializeField] private InteractableUnityEventWrapper startGameButtonEventWrapper;

    [Header("Transition Config")]
    [SerializeField] private float scaleDuration = 0.25f;
    
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
    }
    
    [Button]
    private void ClickButton()
    {
        startGameButtonEventWrapper.WhenSelect.Invoke();
    }
}
