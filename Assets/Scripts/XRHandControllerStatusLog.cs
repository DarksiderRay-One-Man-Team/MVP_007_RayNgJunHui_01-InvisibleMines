using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class XRHandControllerStatusLog : MonoBehaviour
{
    public enum HandControllerStatus
    {
        None,
        HandOnly,
        ControllerOnly,
        HandAndController
    }

    public HandControllerStatus status;

    [Header("Components")]
    [SerializeField] private GameObject controllerObj;
    [SerializeField] private SkinnedMeshRenderer handMeshRenderer;
    
    public delegate void OnHandControllerStatusUpdated(HandControllerStatus status);
    public OnHandControllerStatusUpdated onHandControllerStatusUpdated;

    void Awake()
    {
        Assert.IsNotNull(controllerObj);
        Assert.IsNotNull(handMeshRenderer);
    }
    
    void Update()
    {
        bool controllerActive = controllerObj.activeInHierarchy;
        bool handActive = handMeshRenderer.enabled;

        var lastStatus = status;
        
        if (controllerActive && handActive)
        {
            status = HandControllerStatus.HandAndController;
        }
        else if (controllerActive)
        {
            status = HandControllerStatus.ControllerOnly;
        }
        else if (handActive)
        {
            status = HandControllerStatus.HandOnly;
        }
        else
        {
            status = HandControllerStatus.None;
        }

        if (status != lastStatus)
            onHandControllerStatusUpdated?.Invoke(status);
    }
}
