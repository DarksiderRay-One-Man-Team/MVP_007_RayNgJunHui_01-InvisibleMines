using System;
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

    [Header("Sub Screens")]
    [SerializeField] private GUI_IntroSubScreen subScreen_Main;
    [SerializeField] private GUI_IntroSubScreen subScreen_Tutorial;
    [SerializeField] private InteractableUnityEventWrapper tutorialButtonEventWrapper;
    [SerializeField] private GUI_IntroSubScreen subScreen_Equipment;
    [SerializeField] private InteractableUnityEventWrapper equipmentButtonEventWrapper;
    
    [Header("Transition Config")]
    [SerializeField] private float scaleDuration = 0.25f;
    
    private void Awake()
    {
        Assert.IsNotNull(mainGameManager);
        Assert.IsNotNull(startGameButtonEventWrapper);
        
        startGameButtonEventWrapper.WhenSelect.AddListener(() =>
        {
            transform.DOScale(Vector3.zero, scaleDuration).OnComplete(() =>
            {
                mainGameManager.StartGame();
                gameObject.SetActive(false);
            });
        });
        
        Assert.IsNotNull(subScreen_Main);
        Assert.IsNotNull(subScreen_Tutorial);
        Assert.IsNotNull(tutorialButtonEventWrapper);
        Assert.IsNotNull(subScreen_Equipment);
        Assert.IsNotNull(equipmentButtonEventWrapper);

        tutorialButtonEventWrapper.WhenSelect.AddListener(() =>
        {
            StartCoroutine(ToggleOnSubScreen(subScreen_Tutorial, scaleDuration));
        });
        
        equipmentButtonEventWrapper.WhenSelect.AddListener(() =>
        {
            StartCoroutine(ToggleOnSubScreen(subScreen_Equipment, scaleDuration));
        });
    }

    private void OnEnable()
    {
        //ToggleOffAllSubScreens();
        StartCoroutine(ToggleOnSubScreen(subScreen_Main, 0));
    }
    
    [Button]
    private void ClickStartGameButton()
    {
        startGameButtonEventWrapper.WhenSelect?.Invoke();
    }
    
    [Button]
    private void ClickTutorialButton()
    {
        tutorialButtonEventWrapper.WhenSelect?.Invoke();
    }
    
    [Button]
    private void ClickEquipmentButton()
    {
        equipmentButtonEventWrapper.WhenSelect?.Invoke();
    }

    private IEnumerator ToggleOffAllSubScreens(Action onComplete = null)
    {
        subScreen_Main.ToggleSubScreen(false, scaleDuration);
        subScreen_Tutorial.ToggleSubScreen(false, scaleDuration);
        subScreen_Equipment.ToggleSubScreen(false, scaleDuration);

        yield return new WaitForSeconds(scaleDuration);
        
        onComplete?.Invoke();
    }

    private IEnumerator ToggleOnSubScreen(GUI_IntroSubScreen subScreenToToggle, float scaleDuration, Action onComplete = null)
    {
        subScreen_Main.ToggleSubScreen(subScreenToToggle == subScreen_Main, scaleDuration);
        subScreen_Tutorial.ToggleSubScreen(subScreenToToggle == subScreen_Tutorial, scaleDuration);
        subScreen_Equipment.ToggleSubScreen(subScreenToToggle == subScreen_Equipment, scaleDuration);

        yield return new WaitForSeconds(scaleDuration);

        onComplete?.Invoke();
    }
}

[Serializable]
public class GUI_IntroSubScreen
{
    public enum SubScreen
    {
        Title,
        Tutorial,
        Equipment
    }

    public SubScreen type;
    public GameObject subScreenObject;

    public void ToggleSubScreen(bool value, float scaleDuration = 0f)
    {
        subScreenObject.transform.DOScale(value? Vector3.one: Vector3.zero, scaleDuration);
    }
}
