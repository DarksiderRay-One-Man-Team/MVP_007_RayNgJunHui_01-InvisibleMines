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
    [SerializeField] private Transform detectOrigin;
    [SerializeField] private float warningRangeRadius = 1f;
    [SerializeField] private float dangerRangeRadius = 0.25f;

    [Header("Input Config")]
    [SerializeField] private XRHandControllerStatusLog xrHandControllerStatusLog;
    [SerializeField] private Transform detectOriginParent_Controller;
    [SerializeField] private Transform detectOriginParent_Hand;
    

    //private bool isDead = false;

    void Awake()
    {
        xrHandControllerStatusLog.onHandControllerStatusUpdated += status =>
        {
            switch (status)
            {
                case XRHandControllerStatusLog.HandControllerStatus.None:
                    detectOrigin.gameObject.SetActive(false);
                    break;
                
                case XRHandControllerStatusLog.HandControllerStatus.HandOnly:
                    detectOrigin.gameObject.SetActive(true);
                    detectOrigin.parent = detectOriginParent_Hand;
                    detectOrigin.transform.localPosition = Vector3.zero;
                    detectOrigin.transform.localRotation = Quaternion.identity;
                    break;
                
                case XRHandControllerStatusLog.HandControllerStatus.ControllerOnly:
                case XRHandControllerStatusLog.HandControllerStatus.HandAndController:
                    detectOrigin.gameObject.SetActive(true);
                    detectOrigin.parent = detectOriginParent_Controller;
                    detectOrigin.transform.localPosition = Vector3.zero;
                    detectOrigin.transform.localRotation = Quaternion.identity;
                    break;
            }
        };
    }
    
    void FixedUpdate()
    {
        if (!InvisibleMinesGameManager.IsAlive)
        {
            if (beepingAudioSource.isPlaying) beepingAudioSource.Stop();
            visualIndicator.material.color = deadColor;
            return;
        }
        
        var mineCollidersInDangerRange = Physics.OverlapSphere(detectOrigin.position, dangerRangeRadius, LayerMask.GetMask("Mine"));
        if (mineCollidersInDangerRange.Length > 0)
        {
            visualIndicator.material.color = dangerColor;
            if (!beepingAudioSource.isPlaying) beepingAudioSource.Play();
            beepingAudioSource.pitch = beepPitch_Danger;
            return;
        }
        
        var mineCollidersInWarningRange = Physics.OverlapSphere(detectOrigin.position, warningRangeRadius, LayerMask.GetMask("Mine"));
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

    // void OnTriggerEnter(Collider col)
    // {
    //     if (col.TryGetComponent(out Mine mine))
    //     {
    //         beepingAudioSource.Stop();
    //         visualIndicator.material.color = deadColor;
    //         mine.Explode();
    //         isDead = true;
    //     }
    // }
    //
    // void OnTriggerExit(Collider col)
    // {
    //     if (col.TryGetComponent(out Mine mine))
    //     {
    //         isDead = false;
    //     }
    // }
}
