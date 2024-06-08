using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
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

    [Header("Game Status UI")]
    [SerializeField] private GameObject gameStatusUI;
    [SerializeField] private GameObject gameStatusUI_HUD;
    
    [Header("Lethal Checks")]
    [SerializeField] private GUI_LivesRemaining onScreenGUI_LivesRemaining;
    [SerializeField] private List<LethalCheck> lethalChecks = new();
    [SerializeField] private int noOfLivesGiven = 3;
    [SerializeField, ReadOnly] private int noOfLivesRemaining;
    public static bool IsAlive = true;
    
    [Header("Passthrough Layers")]
    [SerializeField] private PassthroughLayerController passthroughLayerController;
    [SerializeField] private OVRPassthroughLayer passthroughLayer_Normal;
    [SerializeField] private OVRPassthroughLayer passthroughLayer_Hurt;

    [Header("DEBUG")]
    //[SerializeField] private TextMeshPro debugText;

    [SerializeField] private MRUKAnchor largestSurface; 

    public delegate void OnGameStatusChange();
    public OnGameStatusChange onGameStart, onGameOver;
    public delegate void OnRemainingLivesUpdated(int noOfLivesRemaining);
    public OnRemainingLivesUpdated onLivesInitialised, onRemainingLivesUpdated;
    
    private void Awake()
    {
        Assert.IsNotNull(roomManager);
        Assert.IsNotNull(minePlacementManager);
        Assert.IsNotNull(taskPlacementManager);
        //roomManager.onMRUKSceneLoaded += minePlacementManager.OnMRUKSceneLoaded;
        roomManager.onMRUKSceneLoaded += (room, _, _) =>
        {
            largestSurface = room.FindLargestSurface("WALL_FACE");
            gameStatusUI.transform.position = largestSurface.transform.position;
            gameStatusUI.transform.rotation = Quaternion.LookRotation(largestSurface.transform.forward, Vector3.up);
            gameStatusUI.SetActive(false);
            //StartGame();
        };
        
        Assert.IsNotNull(onScreenGUI_LivesRemaining);
        Assert.IsTrue(noOfLivesGiven > 0);
        foreach (var lethalCheck in lethalChecks)
        {
            lethalCheck.onLethalInvoked += LoseLife;
        }
        
        Assert.IsNotNull(passthroughLayerController);
        Assert.IsNotNull(passthroughLayer_Normal);
        Assert.IsNotNull(passthroughLayer_Hurt);

        onRemainingLivesUpdated += noOfLives =>
        {
            if (noOfLives > 0)
                onScreenGUI_LivesRemaining.FadeInTemporarily();
            StartCoroutine(FadePassthroughLayerWhenHurt());
        };
        
        onGameOver += () =>
        {
            taskPlacementManager.DestroyAllTasks();
            minePlacementManager.DisableAllMines();
            minePlacementManager.ToggleSpawnTimer(false);
            minePlacementManager.ToggleAllMineVisibilities(true);

            gameStatusUI_HUD.SetActive(false);

            ToggleLethalChecks(false);
        };
        
        ToggleLethalChecks(false);
    }

    // void Update()
    // {
    //     debugText.text = $"Lives Left: {noOfLivesRemaining}\nTasks Done: {taskPlacementManager.NoOfCompletedTasks}";
    // }

    // private void OnDestroy()
    // {
    //     roomManager.onMRUKSceneLoaded -= minePlacementManager.OnMRUKSceneLoaded;
    // }

    [Button]
    public void StartGame()
    {
        roomManager.FadeShaderColorInGame();
        
        noOfLivesRemaining = noOfLivesGiven;
        //onScreenGUI_LivesRemaining.InitializeLives(noOfLivesRemaining);
        onLivesInitialised?.Invoke(noOfLivesRemaining);
        onScreenGUI_LivesRemaining.FadeInTemporarily();

        minePlacementManager.DestroyAllMines();
        minePlacementManager.PlaceInitialMines(roomManager.Room,
                                                roomManager.RoomSize, 
                                                roomManager.AvailableSpaceSize);
        minePlacementManager.ToggleSpawnTimer(true);

        taskPlacementManager.DestroyAllTasks();
        taskPlacementManager.ResetNoOfCompletedTasks();
        taskPlacementManager.PlaceInitialTasks();

        IsAlive = true;
        ToggleLethalChecks(true);
        
        gameStatusUI.SetActive(true);
        gameStatusUI_HUD.SetActive(true);

        onGameStart?.Invoke();
    }

    [Button]
    private void LoseLife()
    {
        noOfLivesRemaining--;
        onRemainingLivesUpdated?.Invoke(noOfLivesRemaining);
        
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

    private void ToggleLethalChecks(bool value)
    {
        foreach (var lethalCheck in lethalChecks)
        {
            lethalCheck.enabled = value;
        }
    }
}
