using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Assertions;

public class TaskInteractable_Button : TaskInteractable
{
    [Header("Button Config")]
    [SerializeField] private InteractableUnityEventWrapper interactableUnityEventWrapper;

    protected override void Awake()
    {
        base.Awake();

        Assert.IsNotNull(interactableUnityEventWrapper);
        interactableUnityEventWrapper.WhenSelect.AddListener(CompleteTask);
    }
}
