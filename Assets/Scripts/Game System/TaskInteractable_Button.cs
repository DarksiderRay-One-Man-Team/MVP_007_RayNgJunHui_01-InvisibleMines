using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Assertions;

public class TaskInteractable_Button : TaskInteractable
{
    [Header("Button Config")]
    [SerializeField] private GameObject buttonShadow;
    [SerializeField] private InteractableUnityEventWrapper interactableUnityEventWrapper;

    protected override void Awake()
    {
        base.Awake();

        Assert.IsNotNull(interactableUnityEventWrapper);
        interactableUnityEventWrapper.WhenSelect.AddListener(CompleteTask);
    }

    protected override void Spawn()
    {
        base.Spawn();
        buttonShadow.SetActive(true);
    }

    public override void Despawn()
    {
        base.Despawn();
        buttonShadow.SetActive(false);
    }
}
