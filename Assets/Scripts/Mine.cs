
using System.Collections;
using System.Collections.Generic;
using CartoonFX;
using UnityEngine;
using UnityEngine.Assertions;

public class Mine : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Collider collider;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private CFXR_Effect explodeFx;

    public delegate void OnExplode();
    public OnExplode onExplode;

    private bool isActive = true;
    private bool hasExploded = false;

    private void Awake()
    {
        Assert.IsNotNull(meshRenderer);
        Assert.IsNotNull(collider);
        Assert.IsNotNull(sfxAudioSource);
        Assert.IsNotNull(explodeFx);
    }
    
    public void ToggleMeshRenderer(bool value)
    {
        meshRenderer.enabled = value;
    }

    public void SetActive(bool value)
    {
        isActive = value;
    }

    public void Explode()
    {
        if (hasExploded || !isActive) return;
        
        collider.enabled = false;
        sfxAudioSource.Play();
        Instantiate(explodeFx, transform.position, transform.rotation);
        onExplode?.Invoke();

        hasExploded = true;
    }
}
