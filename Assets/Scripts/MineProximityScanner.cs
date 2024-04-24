using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

public class MineProximityScanner : MonoBehaviour
{
    [SerializeField] private InvisibleMinesGameManager mainGameManager;

    private enum MineProximityStatus
    {
        Normal,
        Warning,
        Danger
    }

    [SerializeField, ReadOnly] private MineProximityStatus mineProximityStatus;
    [SerializeField, ReadOnly] private bool isActive = false;
    
    [Header("Visuals")]
    [SerializeField] private MeshRenderer visualIndicator;
    [SerializeField] private Color normalColor = Color.green;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color dangerColor = Color.yellow;
    [SerializeField] private Color deadColor = Color.red;

    [SerializeField] private GameObject visualIndicatorParent;
    [SerializeField] private GameObject visualIndicator_Normal;
    [SerializeField] private GameObject visualIndicator_Warning;
    [SerializeField] private GameObject visualIndicator_Danger;
    
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
        Assert.IsNotNull(mainGameManager);

        mainGameManager.onGameStart += () =>
        {
            ToggleActive(true);
        };
        mainGameManager.onGameOver += () =>
        {
            ToggleActive(false);
        };

        Assert.IsNotNull(visualIndicatorParent);
        Assert.IsNotNull(visualIndicator_Normal);
        Assert.IsNotNull(visualIndicator_Warning);
        Assert.IsNotNull(visualIndicator_Danger);
        
        Assert.IsNotNull(beepingAudioSource);
        
        Assert.IsNotNull(detectOrigin);
        
        Assert.IsNotNull(xrHandControllerStatusLog);
        Assert.IsNotNull(detectOriginParent_Controller);
        Assert.IsNotNull(detectOriginParent_Hand);
        
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

    void Start()
    {
        ToggleActive(false);
    }
    
    void FixedUpdate()
    {
        // if (!InvisibleMinesGameManager.IsAlive)
        // {
        //     if (beepingAudioSource.isPlaying) beepingAudioSource.Stop();
        //     visualIndicator.material.color = deadColor;
        //     return;
        // }

        if (!isActive)
            return;
        
        var mineCollidersInDangerRange = Physics.OverlapSphere(detectOrigin.position, dangerRangeRadius, LayerMask.GetMask("Mine"));
        if (mineCollidersInDangerRange.Length > 0)
        {
            SetProximityStatus(MineProximityStatus.Danger);
            return;
        }
        
        var mineCollidersInWarningRange = Physics.OverlapSphere(detectOrigin.position, warningRangeRadius, LayerMask.GetMask("Mine"));
        if (mineCollidersInWarningRange.Length > 0)
        {
            SetProximityStatus(MineProximityStatus.Warning);
            return;
        }
        
        SetProximityStatus(MineProximityStatus.Normal);
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

    private void ToggleActive(bool value)
    {
        visualIndicatorParent.SetActive(value);
        isActive = value;

        if (isActive) return;
        
        visualIndicator_Normal.SetActive(false);
        visualIndicator_Warning.SetActive(false);
        visualIndicator_Danger.SetActive(false);
            
        if (beepingAudioSource.isPlaying) beepingAudioSource.Stop();
    }

    private void SetProximityStatus(MineProximityStatus newStatus)
    {
        mineProximityStatus = newStatus;
        
        visualIndicator_Normal.SetActive(mineProximityStatus == MineProximityStatus.Normal);
        visualIndicator_Warning.SetActive(mineProximityStatus == MineProximityStatus.Warning);
        visualIndicator_Danger.SetActive(mineProximityStatus == MineProximityStatus.Danger);

        switch (mineProximityStatus)
        {
            case MineProximityStatus.Normal:
                visualIndicator.material.color = normalColor;
                if (beepingAudioSource.isPlaying) beepingAudioSource.Stop();
                break;
            
            case MineProximityStatus.Warning:
                visualIndicator.material.color = warningColor;
                if (!beepingAudioSource.isPlaying) beepingAudioSource.Play();
                beepingAudioSource.pitch = beepPitch_Warning;
                break;
            
            case MineProximityStatus.Danger:
                visualIndicator.material.color = dangerColor;
                if (!beepingAudioSource.isPlaying) beepingAudioSource.Play();
                beepingAudioSource.pitch = beepPitch_Danger;
                break;
        }
    }
}
