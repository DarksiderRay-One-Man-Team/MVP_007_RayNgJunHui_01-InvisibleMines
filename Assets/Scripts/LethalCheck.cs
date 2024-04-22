using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LethalCheck : MonoBehaviour
{
    public delegate void OnLethalInvoked();
    public OnLethalInvoked onLethalInvoked;
    
    void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent(out Mine mine))
        {
            mine.Explode();
            onLethalInvoked?.Invoke();
        }
    }
}
