using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineProximityScanner : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private MeshRenderer visualIndicator;
    [SerializeField] private Color normalColor = Color.green;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color dangerColor = Color.yellow;
    [SerializeField] private Color deadColor = Color.red;
    
    [Header("Audio")]
    [SerializeField] private AudioSource beepingAudioSource;

    [SerializeField] private float beepPitch_Warning = 1f;
    [SerializeField] private float beepPitch_Danger = 2f;
    
    [Header("Range Properties")]
    [SerializeField] private float warningRangeRadius = 1f;
    [SerializeField] private float dangerRangeRadius = 0.25f;

    private bool isDead = false;
    
    void FixedUpdate()
    {
        if (isDead)
            return;
        
        var mineCollidersInDangerRange = Physics.OverlapSphere(transform.position, dangerRangeRadius, LayerMask.GetMask("Mine"));
        if (mineCollidersInDangerRange.Length > 0)
        {
            visualIndicator.material.color = dangerColor;
            if (!beepingAudioSource.isPlaying) beepingAudioSource.Play();
            beepingAudioSource.pitch = beepPitch_Danger;
            return;
        }
        
        var mineCollidersInWarningRange = Physics.OverlapSphere(transform.position, warningRangeRadius, LayerMask.GetMask("Mine"));
        if (mineCollidersInWarningRange.Length > 0)
        {
            visualIndicator.material.color = warningColor;
            if (!beepingAudioSource.isPlaying) beepingAudioSource.Play();
            beepingAudioSource.pitch = beepPitch_Warning;
            return;
        }
        
        visualIndicator.material.color = normalColor;
        if (beepingAudioSource.isPlaying) beepingAudioSource.Stop();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent(out Mine mine))
        {
            beepingAudioSource.Stop();
            visualIndicator.material.color = deadColor;
            mine.Explode();
            isDead = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.TryGetComponent(out Mine mine))
        {
            isDead = false;
        }
    }
}
