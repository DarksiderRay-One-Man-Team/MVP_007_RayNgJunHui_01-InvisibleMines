using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class MineScannerShader : MonoBehaviour
{
    [Header("Scan Configuration")]
    [SerializeField] private float scanDuration = 0.5f;
    [SerializeField] private float scanRadius = 0.5f;
    
    [Header("Property Names")]
    [SerializeField] private string propertyName_OriginPos = "_MineScanOrigin";
    [SerializeField] private string propertyName_Alpha = "_MineScanAlpha";
    [SerializeField] private string propertyName_Radius = "_MineScanRadius";

    [Header("DEBUG")] 
    [SerializeField] private Vector3 debugOriginPos;

    void Start()
    {
        Shader.SetGlobalFloat(propertyName_Alpha, 0);
        
    }
    

    [Button]
    private void DebugStartScan()
    {
        StartScan(debugOriginPos);
    }


    private void StartScan(Vector3 originPos)
    {
        Shader.SetGlobalVector(propertyName_OriginPos, originPos);
        
        float alpha = 1;
        Shader.SetGlobalFloat(propertyName_Alpha, alpha);
        DOTween.To(() => alpha, a => alpha = a, 0, scanDuration).
            SetEase(Ease.InQuint).OnUpdate(() =>
        {
            Shader.SetGlobalFloat(propertyName_Alpha, alpha);
        });

        float radius = 0;
        Shader.SetGlobalFloat(propertyName_Radius, radius);
        DOTween.To(() => radius, r => radius = r, scanRadius, scanDuration).OnUpdate(() =>
        {
            Shader.SetGlobalFloat(propertyName_Radius, radius);
        });
    }
}
