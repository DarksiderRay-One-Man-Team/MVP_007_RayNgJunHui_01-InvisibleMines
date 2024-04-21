using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class RoomManager : MonoBehaviour
{
    [Header("Dependencies")]
    //[SerializeField] private OVRSceneManager sceneManager;
    [SerializeField] private MRUK mruk;
    
    private MRUKRoom room;
    private BoxCollider roomBounds;
    private EffectMesh effectMesh;
    
    public delegate void OnMRUKSceneLoaded(MRUKRoom room, float roomSize, float availableSpaceSize);
    public OnMRUKSceneLoaded onMRUKSceneLoaded;

    [Header("DEBUG")]
    [SerializeField] private TextMeshPro debugText;
    [SerializeField, ReadOnly] private Vector3 roomDimension;
    [SerializeField, ReadOnly] private float roomSize;
    [SerializeField, ReadOnly] private float availableSpaceSize;

    void Awake()
    {
        Assert.IsNotNull(mruk);
        mruk.SceneLoadedEvent.AddListener(OnSceneLoaded);
        
        roomBounds = GetComponent<BoxCollider>();
        Assert.IsNotNull(roomBounds);
        roomBounds.isTrigger = true;
    }

    private void OnSceneLoaded()
    {
        room = mruk.GetCurrentRoom();
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
