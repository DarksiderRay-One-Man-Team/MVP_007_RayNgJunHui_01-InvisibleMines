using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class TaskInteractable : MonoBehaviour
{
    [Header("Spawn/Despawn Config")]
    [SerializeField] private bool scaleTransitionOnStart = true;
    [SerializeField] private float scaleTransitionDuration = 0.25f;
    [SerializeField] private List<ButtonShadow> buttonShadows;
    [SerializeField] private List<MeshRenderer> visualMeshRenderers = new();
    [SerializeField] private Vector2 dissolveRange = new(0, 1);
    [SerializeField, Range(0, 1)] private float dissolveLerp = 1;
    [SerializeField, ReadOnly] private float dissolveValue;

    [Header("Task Status")]
    [SerializeField, ReadOnly] private bool isCompleted = false;

    public delegate void OnTaskComplete();
    public OnTaskComplete onTaskComplete;
    
    private Vector3 originalScale;

    private bool isDespawning = false;

    protected virtual void Awake()
    {
        if (scaleTransitionOnStart)
            Assert.IsTrue(scaleTransitionDuration > 0, "Scale transition duration must be greater than 0");
    }
    
    protected virtual void Start()
    {
        if (scaleTransitionOnStart)
        {
            originalScale = transform.localScale;
            Spawn();
        }
    }

    // void Update()
    // {
    //     dissolveValue = 1 - Mathf.Lerp(dissolveRange.x, dissolveRange.y, dissolveLerp);
    //     
    //     foreach (var renderer in visualMeshRenderers)
    //     {
    //         renderer.material.SetFloat("_ShaderDissolve", dissolveValue);
    //     }
    // }

    [Button]
    protected void CompleteTask()
    {
        if (isCompleted) return;
        
        onTaskComplete?.Invoke();
        isCompleted = true;
        Despawn();
    }

    protected virtual void Spawn()
    {
        // transform.localScale = Vector3.zero;
        // transform.DOScale(originalScale, scaleTransitionDuration);

        foreach (var shadow in buttonShadows)
        {
            shadow.gameObject.SetActive(true);
        }
        
        dissolveLerp = 1;
        DOTween.To(() => dissolveLerp, d => dissolveLerp = d, 0, scaleTransitionDuration).
            OnUpdate(() =>
            {
                dissolveValue = 1 - Mathf.Lerp(dissolveRange.x, dissolveRange.y, dissolveLerp);
            foreach (var renderer in visualMeshRenderers)
            {
                renderer.material.SetFloat("_ShaderDissolve", dissolveValue);
            }
        });
    }
    
    public virtual void Despawn()
    {
        // transform.DOScale(Vector3.zero, scaleTransitionDuration).onComplete += () =>
        // {
        //     Destroy(gameObject);
        // };

        // attempt to prevent visual bug when despawning
        if (isDespawning)
            return;

        isDespawning = true;
        
        foreach (var shadow in buttonShadows)
        {
            shadow.gameObject.SetActive(false);
        }
        
        DOTween.To(() => dissolveLerp, d => dissolveLerp = d, 1, scaleTransitionDuration).
            OnUpdate(() =>
            {
                dissolveValue = 1 - Mathf.Lerp(dissolveRange.x, dissolveRange.y, dissolveLerp);
                foreach (var renderer in visualMeshRenderers)
                {
                    renderer.material.SetFloat("_ShaderDissolve", dissolveValue);
                }
            }).OnComplete(() => Destroy(gameObject));
        
    }

    [Button]
    private void GetAllShadows()
    {
        buttonShadows = GetComponentsInChildren<ButtonShadow>(true).ToList();
    }

    [Button]
    private void GetAllRenderers()
    {
        visualMeshRenderers = GetComponentsInChildren<MeshRenderer>(true).ToList();
    }
}
