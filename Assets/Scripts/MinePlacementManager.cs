using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

public class MinePlacementManager : MonoBehaviour
{
    [SerializeField] private FindSpawnPositions_Custom posSpawner;
    [SerializeField] private RoomManager roomManager;

    [Header("Mines")]
    [SerializeField] private int spawnCount = 15;
    [SerializeField] private List<Mine> spawnedMines;

    private bool allMinesVisible = false;

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
    
    private void OnMRUKSceneLoaded(MRUKRoom room, float roomSize, float availableSpaceSize)
    {
        Debug.Log("Mine Placement Manager invoked!");
        spawnCount = availableSpaceSize < 50 ? 10: 16;
        posSpawner.StartSpawn(room, spawnCount, out var spawnedMineObjects);

        

        spawnedMines.Clear();
        foreach (var mineObj in spawnedMineObjects)
        {
            if (mineObj.TryGetComponent(out Mine mine))
                spawnedMines.Add(mine);
        }

        ToggleAllMineVisibilities(false);
    }

    private void ToggleAllMineVisibilities(bool value)
    {
        allMinesVisible = value;
        
        foreach (var mine in spawnedMines)
        {
            mine.ToggleMeshRenderer(value);
        }
    }

    [Button]
    public void AlternateMineVisiblity()
    {
        ToggleAllMineVisibilities(!allMinesVisible);
    }
}
