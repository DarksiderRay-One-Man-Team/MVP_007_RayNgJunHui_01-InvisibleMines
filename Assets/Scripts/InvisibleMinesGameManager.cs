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
    [SerializeField] private GUI_LivesRemaining gui_LivesRemaining;
    [SerializeField] private List<LethalCheck> lethalChecks = new();
    [SerializeField] private int noOfLivesGiven = 3;
    [SerializeField, ReadOnly] private int noOfLivesRemaining;
    public static bool IsAlive = true;
    
    [Header("Passthrough Layers")]
    [SerializeField] private PassthroughLayerController passthroughLayerController;
    [SerializeField] private OVRPassthroughLayer passthroughLayer_Normal;
    [SerializeField] private OVRPassthroughLayer passthroughLayer_Hurt;

    [Header("DEBUG")]
    [SerializeField] private TextMeshPro debugText;

    private delegate void OnGameStatusChange();
    private OnGameStatusChange onLoseLife, onGameOver;
    
    private void Awake()
    {
        Assert.IsNotNull(roomManager);
        Assert.IsNotNull(minePlacementManager);
        Assert.IsNotNull(taskPlacementManager);
        //roomManager.onMRUKSceneLoaded += minePlacementManager.OnMRUKSceneLoaded;
        roomManager.onMRUKSceneLoaded += (_, _, _) => StartGame();
        
        Assert.IsNotNull(gui_LivesRemaining);
        Assert.IsTrue(noOfLivesGiven > 0);
        foreach (var lethalCheck in lethalChecks)
        {
            lethalCheck.onLethalInvoked += LoseLife;
        }
        
        Assert.IsNotNull(passthroughLayerController);
        Assert.IsNotNull(passthroughLayer_Normal);
        Assert.IsNotNull(passthroughLayer_Hurt);

        onLoseLife += () =>
        {
            gui_LivesRemaining.ShowLivesRemaining(noOfLivesRemaining);
            StartCoroutine(FadePassthroughLayerWhenHurt());
        };
        
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
        gui_LivesRemaining.InitializeLives(noOfLivesRemaining);
        gui_LivesRemaining.ShowLivesRemaining(noOfLivesRemaining);

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
        onLoseLife?.Invoke();
        
        if (noOfLivesRemaining <= 0)
        {
            IsAlive = false;
            onGameOver?.Invoke();
            Debug.Log("Game Over! You ran out of lives");
        }
    }

    private IEnumerator FadePassthroughLayerWhenHurt()
    {
        passthroughLayerController.SetActiveLayer(passthroughLayer_Hurt, 0.1f);
        yield return new WaitForSecondsRealtime(1.5f);
        passthroughLayerController.SetActiveLayer(passthroughLayer_Normal, 1f);
    }
}
