using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

public class InvisibleMinesGameManager : MonoBehaviour
{
    [Header("Manager Components")]
    [SerializeField] private RoomManager roomManager;
    [SerializeField] private MinePlacementManager minePlacementManager;
    [SerializeField] private TaskPlacementManager taskPlacementManager;
    
    private void Awake()
    {
        Assert.IsNotNull(roomManager);
        Assert.IsNotNull(minePlacementManager);
        Assert.IsNotNull(taskPlacementManager);

        roomManager.onMRUKSceneLoaded += minePlacementManager.OnMRUKSceneLoaded;
    }

    private void OnDestroy()
    {
        roomManager.onMRUKSceneLoaded -= minePlacementManager.OnMRUKSceneLoaded;
    }

    [Button]
    private void StartGame()
    {
        minePlacementManager.PlaceInitialMines(roomManager.Room,
                                                roomManager.RoomSize, 
                                                roomManager.AvailableSpaceSize);
        taskPlacementManager.PlaceInitialTasks();
    }
}
