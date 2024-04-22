using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class TaskInteractable : MonoBehaviour
{
    [Header("Spawn/Despawn Config")]
    [SerializeField] private bool scaleTransitionOnStart = true;
    [SerializeField] private float scaleTransitionDuration = 0.25f;

    [Header("Task Status")]
    [SerializeField, ReadOnly] private bool isCompleted = false;

    public delegate void OnTaskComplete();
    public OnTaskComplete onTaskComplete;

    protected virtual void Awake()
    {
        if (scaleTransitionOnStart)
            Assert.IsTrue(scaleTransitionDuration > 0, "Scale transition duration must be greater than 0");
    }
    
    protected virtual void Start()
    {
        if (scaleTransitionOnStart)
        {
            var originalScale = transform.localScale;
            transform.localScale = Vector3.zero;
            transform.DOScale(originalScale, scaleTransitionDuration);
        }
    }

    [Button]
    protected void CompleteTask()
    {
        if (isCompleted) return;
        
        onTaskComplete?.Invoke();
        isCompleted = true;
        transform.DOScale(Vector3.zero, scaleTransitionDuration).onComplete += () =>
        {
            Destroy(gameObject);
        };
    }
}
