using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class GUI_IntroSubScreen_Tutorial : GUI_IntroSubScreen
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshPro tutorialText;
    [SerializeField] private PokeInteractable previousButtonInteractableView;
    [SerializeField] private InteractableUnityEventWrapper previousButtonUnityEventWrapper;
    [SerializeField] private PokeInteractable nextButtonInteractableView;
    [SerializeField] private InteractableUnityEventWrapper nextButtonUnityEventWrapper;

    [Header("Tutorial Data")]
    [SerializeField] private List<TutorialData> tutorialObjects;
    [SerializeField, ReadOnly] private int currentIndex;

    protected override void Awake()
    {
        base.Awake();
        
        Assert.IsNotNull(tutorialText);
        Assert.IsNotNull(previousButtonUnityEventWrapper);
        Assert.IsNotNull(nextButtonUnityEventWrapper);

        Assert.IsTrue(tutorialObjects.Count > 0);
    }

    void Start()
    {
        onToggledOn += () => GoToIndex(0);
        
        previousButtonUnityEventWrapper.WhenSelect.AddListener(GoToPreviousIndex);
        nextButtonUnityEventWrapper.WhenSelect.AddListener(GoToNextIndex);
    }

    // void OnEnable()
    // {
    //     GoToIndex(0);
    // }

    [Button]
    void GoToPreviousIndex()
    {
        if (currentIndex <= 0)
            return;
        GoToIndex(currentIndex - 1);
    }

    [Button]
    void GoToNextIndex()
    {
        if (currentIndex >= tutorialObjects.Count - 1)
            return;
        GoToIndex(currentIndex + 1);
    }
    
    void GoToIndex(int newIndex)
    {
        currentIndex = newIndex;

        if (currentIndex <= 0)
            previousButtonInteractableView.Disable();
        else
            previousButtonInteractableView.Enable();
        
        if (currentIndex >= tutorialObjects.Count - 1)
            nextButtonInteractableView.Disable();
        else
            nextButtonInteractableView.Enable();

        tutorialText.text = tutorialObjects[currentIndex].text;
    }
}
