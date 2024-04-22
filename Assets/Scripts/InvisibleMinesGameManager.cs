using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class InvisibleMinesGameManager : MonoBehaviour
{
    [Header("Manager Components")]
    [SerializeField] private RoomManager roomManager;
    [SerializeField] private MinePlacementManager minePlacementManager;
    [SerializeField] private TaskPlacementManager taskPlacementManager;

    [Header("Lethal Checks")]
    [SerializeField] private List<LethalCheck> lethalChecks = new();
    [SerializeField] private int noOfLivesGiven = 3;
    [SerializeField, ReadOnly] private int noOfLivesRemaining;
    public static bool IsAlive = true;

    [Header("DEBUG")]
    [SerializeField] private TextMeshPro debugText;

    private delegate void OnGameOver();
    private OnGameOver onGameOver;
    
    private void Awake()
    {
        Assert.IsNotNull(roomManager);
        Assert.IsNotNull(minePlacementManager);
        Assert.IsNotNull(taskPlacementManager);
        //roomManager.onMRUKSceneLoaded += minePlacementManager.OnMRUKSceneLoaded;
        roomManager.onMRUKSceneLoaded += (_, _, _) => StartGame();
        
        Assert.IsTrue(noOfLivesGiven > 0);
        foreach (var lethalCheck in lethalChecks)
        {
            lethalCheck.onLethalInvoked += LoseLife;
        }

        onGameOver += () =>
        {
            taskPlacementManager.DestroyAllTasks();
            minePlacementManager.DisableAllMines();
        };
    }

    void Update()
    {
        debugText.text = $"Lives Left: {noOfLivesRemaining}\nTasks Done: {taskPlacementManager.NoOfCompletedTasks}";
    }

    // private void OnDestroy()
    // {
    //     roomManager.onMRUKSceneLoaded -= minePlacementManager.OnMRUKSceneLoaded;
    // }

    [Button]
    private void StartGame()
    {
        noOfLivesRemaining = noOfLivesGiven;

        minePlacementManager.DestroyAllMines();
        minePlacementManager.PlaceInitialMines(roomManager.Room,
                                                roomManager.RoomSize, 
                                                roomManager.AvailableSpaceSize);

        taskPlacementManager.DestroyAllTasks();
        taskPlacementManager.PlaceInitialTasks();
    }

    [Button]
    private void LoseLife()
    {
        noOfLivesRemaining--;
        if (noOfLivesRemaining <= 0)
        {
            IsAlive = false;
            onGameOver?.Invoke();
            Debug.Log("Game Over! You ran out of lives");
        }
    }
}
