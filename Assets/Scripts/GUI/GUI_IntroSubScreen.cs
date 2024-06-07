using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class GUI_IntroSubScreen : MonoBehaviour
{
    public GameObject subScreenObject;

    protected delegate void OnToggledEvent();
    protected OnToggledEvent onToggledOn, onToggledOff;

    protected virtual void Awake()
    {
        Assert.IsNotNull(subScreenObject);
    }
    
    public void ToggleSubScreen(bool value, float scaleDuration = 0f)
    {
        subScreenObject.transform.DOScale(value? Vector3.one: Vector3.zero, scaleDuration).OnComplete(() =>
        {
            if (!value)
                onToggledOff?.Invoke();
        });

        if (value)
            onToggledOn?.Invoke();
    }
}
