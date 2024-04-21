using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;
using UnityEngine.Assertions;

public class MinePlacementManager : MonoBehaviour
{
    [SerializeField] private FindSpawnPositions posSpawner;
    [SerializeField] private RoomManager roomManager;

    void Awake()
    {
        Assert.IsNotNull(posSpawner);
        Assert.IsNotNull(roomManager);

        roomManager.onMRUKSceneLoaded += OnMRUKSceneLoaded;
    }
    
    private void OnDestroy()
    {
        roomManager.onMRUKSceneLoaded -= OnMRUKSceneLoaded;
    }
    
    private void OnMRUKSceneLoaded(MRUKRoom room, float roomsize, float availablespacesize)
    {
        posSpawner.StartSpawn(room);
    }
    
}
