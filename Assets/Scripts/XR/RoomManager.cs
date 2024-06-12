using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Meta.XR.MRUtilityKit;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    [Header("Dependencies")]
    //[SerializeField] private OVRSceneManager sceneManager;
    [SerializeField] private MRUK mruk;
    
    private MRUKRoom room;
    public MRUKRoom Room => room;
    private BoxCollider roomBounds;
    private EffectMesh effectMesh;
    
    public delegate void OnMRUKSceneLoaded(MRUKRoom room, float roomSize, float availableSpaceSize);
    public OnMRUKSceneLoaded onMRUKSceneLoaded;
    public UnityEvent OnMRUKSceneLoadFailed;

    // [Header("Material Config")]
    // [SerializeField] private Material roomMeshMat;
    // [SerializeField] private string globalShaderColorProperty;
    // [SerializeField] private Color globalColorBeforeGame;
    // [SerializeField] private Color globalColorDuringGame;
    
    [Header("DEBUG")]
    [SerializeField] private TextMeshPro debugText;
    [SerializeField, ReadOnly] private Vector3 roomDimension;
    [SerializeField, ReadOnly] private float roomSize;
    public float RoomSize => roomSize;
    [SerializeField, ReadOnly] private float availableSpaceSize;
    public float AvailableSpaceSize => availableSpaceSize;

    void Awake()
    {
        Assert.IsNotNull(mruk);
        mruk.SceneLoadedEvent.AddListener(OnSceneLoaded);
        
        roomBounds = GetComponent<BoxCollider>();
        Assert.IsNotNull(roomBounds);
        roomBounds.isTrigger = true;
        
        //roomMeshMat.SetColor(globalShaderColorProperty, globalColorBeforeGame);
    }

    private void OnSceneLoaded()
    {
        room = mruk.GetCurrentRoom();

        if (room == null)
        {
            OnMRUKSceneLoadFailed?.Invoke();
            return;
        }
        
        room.gameObject.SetLayerRecursive("Room");

        //Setup collider for room to fit the bounds of the space (using collider as a Bounds type doesn't work with rotation)
        var bounds = room.GetRoomBounds();
        var center = bounds.center;
        roomBounds.size = bounds.size;
        roomBounds.center = center;
        roomDimension = bounds.size;
        roomSize = bounds.size.x * bounds.size.y * bounds.size.z;

        var roomObjectColliders = room.GetComponentsInChildren<BoxCollider>();
        availableSpaceSize = roomSize;
        foreach (var collider in roomObjectColliders)
        {
            var colliderSize = collider.bounds.size;
            availableSpaceSize -= colliderSize.x * colliderSize.y * colliderSize.z;
        }

        Debug.Log("Room Manager invoked!");
        onMRUKSceneLoaded?.Invoke(room, roomSize, availableSpaceSize);

        if (debugText)
        {
            debugText.text = $"Room Size: {Mathf.Round(roomSize)}\nAvailable Space: {Mathf.Round(availableSpaceSize)}";
        }
    }

    public void FadeShaderColorInGame()
    {
        // DOTween.To(() => currentGlobalShaderColor, c => currentGlobalShaderColor = c, globalColorDuringGame, 0.25f)
        //     .OnUpdate(() => Shader.SetGlobalColor(globalShaderColorProperty, currentGlobalShaderColor));
        //     //.OnUpdate(() => Shader.SetGlobalColor(globalShaderColorProperty, currentGlobalShaderColor));
            
        //roomMeshMat.DOColor(globalColorDuringGame,globalShaderColorProperty, 0.25f);
    }


    private void OnDrawGizmos()
    {
        if (room == null)
            return;

        var outlines = room.GetRoomOutline();

        if (outlines.Count <= 1) return;

        for (int i = 1; i < outlines.Count; i++)
        {
            Gizmos.DrawLine(outlines[i - 1], outlines[i]);
        }
        
        Gizmos.DrawLine(outlines[^1], outlines[0]);
    }
}
