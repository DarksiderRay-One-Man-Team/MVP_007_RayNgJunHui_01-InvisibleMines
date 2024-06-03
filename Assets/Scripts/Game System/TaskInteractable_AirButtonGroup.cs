using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Assertions;

public class TaskInteractable_AirButtonGroup : TaskInteractable
{
    [Header("Buttons Config")]
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private List<InteractableUnityEventWrapper> buttonInteractableUnityEventWrappers = new();

    protected override void Awake()
    {
        base.Awake();

        Assert.IsNotNull(buttonsParent);
        buttonInteractableUnityEventWrappers = buttonsParent.GetComponentsInChildren<InteractableUnityEventWrapper>().ToList();
        foreach (var buttonInteractableUnityEventWrapper in buttonInteractableUnityEventWrappers)
        {
            buttonInteractableUnityEventWrapper.WhenSelect.AddListener(CompleteTask);
        }
    }
}
