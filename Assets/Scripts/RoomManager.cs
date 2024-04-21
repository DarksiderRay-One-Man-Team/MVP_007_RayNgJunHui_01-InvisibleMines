using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class RoomManager : MonoBehaviour
{
    [Header("Dependencies")] [SerializeField]
    private OVRSceneManager sceneManager;

    private OVRSceneRoom room;
    private BoxCollider roomBounds;
    private TransformFollower follower;

    void Awake()
    {
        Assert.IsNotNull(sceneManager);
        roomBounds = GetComponent<BoxCollider>();
        Assert.IsNotNull(roomBounds);
        roomBounds.isTrigger = true;

        follower = GetComponent<TransformFollower>();
        Assert.IsNotNull(follower);

        sceneManager.SceneModelLoadedSuccessfully += OnSceneLoaded;
    }

    private void OnSceneLoaded()
    {
        room = FindAnyObjectByType<OVRSceneRoom>();
        // TODO handle scenario of no room is found;
        room.gameObject.SetLayerRecursive("Room");

        //Setup collider for room to fit the bounds of the space (using collider as a Bounds type doesn't work with rotation)
        float height = room.Walls[0].Height;
        float width = room.Floor.Width;
        float depth = room.Floor.Height;
        roomBounds.size = new Vector3(width, depth, height);
        roomBounds.center = new Vector3(0, 0, height / 2f);
        follower.target = room.Floor.transform;
    }
    
}
