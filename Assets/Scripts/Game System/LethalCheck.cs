using System.Collections;
using System.Collections.Generic;
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
        
        Debug.Log("Handling EMP state");
        
    }
    void HandleNormalState()
    {
        void OnTriggerEnter(Collider col)
        {
            if (col.TryGetComponent(out Mine mine))
            {
                mine.Explode();
                onLethalInvoked?.Invoke();
            }
        }
    }

    
}

