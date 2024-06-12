
using System.Collections;
using System.Collections.Generic;
using CartoonFX;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

public class Mine : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material mat_Default;
    [SerializeField] private Material mat_Exploded;
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
        Assert.IsNotNull(mat_Default);
        Assert.IsNotNull(mat_Exploded);
        meshRenderer.material = mat_Default;
        Assert.IsNotNull(collider);
        Assert.IsNotNull(sfxAudioSource);
        Assert.IsNotNull(explodeFx);
    }
    
    public void ToggleMeshRenderer(bool value)
    {
        meshRenderer.enabled = value;
    }

    public void RevealTemp()
    {
        //StartCoroutine(StartRevealTemp());
        ToggleMeshRenderer(true);
    }

    IEnumerator StartRevealTemp()
    {
        ToggleMeshRenderer(true);
        yield return new WaitForSeconds(3f);
        ToggleMeshRenderer(false);
    }

    public void SetActive(bool value)
    {
        isActive = value;
        collider.enabled = value;
    }

    [Button]
    public void Explode()
    {
        if (hasExploded || !isActive) return;

        meshRenderer.material = mat_Exploded;
        collider.enabled = false;
        sfxAudioSource.Play();
        Instantiate(explodeFx, transform.position, transform.rotation);
        
        
        onExplode?.Invoke();

        hasExploded = true;
    }
}
