using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRCenterEyeAnchorFollow : MonoBehaviour
{
    [SerializeField] private Transform centerEyeAnchor;
    [SerializeField] private bool lockYPos = false;

    private Vector3 posVelocity = Vector3.zero;
    private Vector3 rotVelocity = Vector3.zero;
    private float smoothTime = 0.3f;

    private Vector3 initialPos;

    void Awake()
    {
        initialPos = transform.position;
    }
    
    void Update()
    {
        var targetPosition = centerEyeAnchor.position;
        if (lockYPos) targetPosition.y = initialPos.y;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref posVelocity, smoothTime);

        var targetEulerAngles = centerEyeAnchor.eulerAngles;
        targetEulerAngles.x = 0;
        targetEulerAngles.z = 0;

        var rotOffset = targetEulerAngles.y - transform.eulerAngles.y;
        if (rotOffset > 180) rotOffset -= 360;
        if (rotOffset < -180) rotOffset += 360;
        
        targetEulerAngles.y = transform.eulerAngles.y + rotOffset;
        
        transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, targetEulerAngles, ref rotVelocity, smoothTime);
    }
}
