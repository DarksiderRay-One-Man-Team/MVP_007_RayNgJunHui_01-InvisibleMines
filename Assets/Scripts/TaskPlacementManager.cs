using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

public class TaskPlacementManager : MonoBehaviour
{
    [Header("Task Spawn Config")]
    [SerializeField] private FindSpawnPositions_Custom taskButtonSpawner;
    [SerializeField] private int taskInteractableSpawnCount = 12;

    [Header("Task Completion Status")]
    [SerializeField, ReadOnly] private List<TaskInteractable> tasksRemaining = new();
    [SerializeField, ReadOnly] private int noOfCompletedTasks = 0;
    public int NoOfCompletedTasks => noOfCompletedTasks;
    
    private void Awake()
    {
        Assert.IsNotNull(taskButtonSpawner);
    }

    public void PlaceInitialTasks()
    {
        taskButtonSpawner.StartSpawn(taskInteractableSpawnCount, out var taskButtons);
        foreach (var button in taskButtons)
        {
            if (button.TryGetComponent(out TaskInteractable taskInteractable))
            {
                tasksRemaining.Add(taskInteractable);
                taskInteractable.onTaskComplete += () =>
                {
                    noOfCompletedTasks++;
                    tasksRemaining.Remove(taskInteractable);
                };
            }
        }
    }

    public void DestroyAllTasks()
    {
        foreach (var button in tasksRemaining)
        {
            button.Destroy();
        }
        tasksRemaining.Clear();
    }
}
