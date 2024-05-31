using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class PassthroughLayerController : MonoBehaviour
{
    [Header("Layer Transition Config")]
    public float defaultFadeDuration = 1f;
    [SerializeField, ReadOnly] private OVRPassthroughLayer activeLayer;
    [SerializeField] private OVRPassthroughLayer[] passthroughLayers;

    [Header("DEBUG")]
    [SerializeField] private OVRPassthroughLayer debugLayerToTransition;
    

    public void SetActiveLayer(OVRPassthroughLayer newActiveLayer, float fadeDuration = -1)
    {
        activeLayer = newActiveLayer;
        if (fadeDuration < 0)
            fadeDuration = defaultFadeDuration;
        
        foreach (var layer in passthroughLayers)
        {
            if (layer == activeLayer && layer.textureOpacity < 1f)
            {
                DOTween.To(() => layer.textureOpacity, x => layer.textureOpacity = x,
                    1f, fadeDuration * (1 - layer.textureOpacity));
            }
            else if (layer != activeLayer && layer.textureOpacity > 0f)
            {
                DOTween.To(() => layer.textureOpacity, x => layer.textureOpacity = x,
                    0f, fadeDuration * layer.textureOpacity);
            }
        }
    }

    [Button]
    private void TestTransitionLayer()
    {
        SetActiveLayer(debugLayerToTransition);
    }
}
