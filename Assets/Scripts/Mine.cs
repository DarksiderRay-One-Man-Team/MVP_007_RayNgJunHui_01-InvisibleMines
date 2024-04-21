
using System.Collections;
using System.Collections.Generic;
using CartoonFX;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private CFXR_Effect explodeFx;

    public void ToggleMeshRenderer(bool value)
    {
        meshRenderer.enabled = value;
    }

    public void Explode()
    {
        sfxAudioSource.Play();
        Instantiate(explodeFx, transform.position, transform.rotation);
    }
}
