using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;

public class MRUKScenePermissionCheck : MonoBehaviour
{
    [SerializeField] private bool askPermissionOnStartup;
    [SerializeField] private UnityEvent onScenePermissionGranted;
    [SerializeField] private UnityEvent onScenePermissionDenied;

    private PermissionCallbacks callbacks;
    
    void Awake()
    {
        #if !UNITY_EDITOR && UNITY_ANDROID
        if (Permission.HasUserAuthorizedPermission(OVRPermissionsRequester.ScenePermission))
        {
            onScenePermissionGranted?.Invoke();
            return;
        }
        
        callbacks = new PermissionCallbacks();
        callbacks.PermissionDenied += permissionId =>
        {
            onScenePermissionDenied?.Invoke();
        };
        callbacks.PermissionGranted += permissionID =>
        {
            onScenePermissionGranted?.Invoke();
        };

        if (askPermissionOnStartup)
        {
            RequestScenePermission();
        }
#else
        onScenePermissionGranted?.Invoke();
#endif
    }
    
    public void RequestScenePermission()
    {
        Permission.RequestUserPermission(OVRPermissionsRequester.ScenePermission, callbacks);
    }
}
