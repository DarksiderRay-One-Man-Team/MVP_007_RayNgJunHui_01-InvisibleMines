using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LethalCheck : MonoBehaviour
{
    public delegate void OnLethalInvoked();
    public OnLethalInvoked onLethalInvoked;

    public enum State
    {
        
        Normal,
        EMP
    }
    public State currentState;
    public float _EMPTime = 5.0f;
    public TextMeshProUGUI countdownText;
    void Update()
    {
        Debug.LogWarning(currentState);
        switch (currentState)
        {
            case State.EMP:
                HandleEMPState();
                break;
            case State.Normal:
                HandleNormalState();
                break;
            default:
                
                break;
        }
    }
    public void SwitchState()
    {
        
        if (currentState == State.EMP)
        {
            currentState = State.Normal;
        }
        else
        {
            currentState = State.EMP;
        }

        
    }
    void HandleEMPState()
    {

        StartCoroutine(_empDelay(_EMPTime));

    }
    void HandleNormalState()
    {
        
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent(out Mine mine)&& currentState== State.Normal)
        {
            mine.Explode();
            onLethalInvoked?.Invoke();
        }
    }
    IEnumerator _empDelay(float delayTime)
    {
        float elapsedTime = 0f;

        
        while (elapsedTime < delayTime)
        {
            float remainingTime = delayTime - elapsedTime;
            countdownText.text = $"Time remaining: {remainingTime:F1} seconds";
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        
        countdownText.text = "Time remaining: 0.0 seconds";
        currentState = State.Normal;



    }


}

