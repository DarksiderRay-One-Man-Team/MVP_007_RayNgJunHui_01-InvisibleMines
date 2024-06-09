using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

public class TaskPlacementManager : MonoBehaviour
{
    [Header("Task Spawn Config")]
    //[SerializeField] private FindSpawnPositions_Custom taskButtonSpawner;
    [SerializeField] private List<FindSpawnPositions_Custom> taskButtonSpawners = new();
    [SerializeField] private int taskInteractableSpawnCount = 12;

    [Header("Task Completion Status")]
    [SerializeField, ReadOnly] private List<TaskInteractable> tasksRemaining = new();
    [SerializeField, ReadOnly] private int noOfCompletedTasks = 0;
    public int NoOfCompletedTasks => noOfCompletedTasks;
    
    public delegate void OnNumberOfCompletedTasksUpdated(int noOfCompletedTasks);
    public OnNumberOfCompletedTasksUpdated onNumberOfCompletedTasksUpdated;
    
    private void Awake()
    {
        //Assert.IsNotNull(taskButtonSpawner);
        Assert.IsTrue(taskButtonSpawners.Count > 0);
    }

    public void PlaceInitialTasks()
    {
        AddNewTasks(taskInteractableSpawnCount);
    }

    private void AddNewTasks(int spawnCount)
    {
        int[] incrementCounts = new int[taskButtonSpawners.Count];
        
        for (int i = 0; i < spawnCount; i++)
        {
            var index = Random.Range(0, taskButtonSpawners.Count);
            incrementCounts[index]++;
        }

        for (int i = 0; i < taskButtonSpawners.Count; i++)
        {
            var taskButtonSpawner = taskButtonSpawners[i];
            taskButtonSpawner.StartSpawn(incrementCounts[i], out var taskButtons);
            foreach (var button in taskButtons)
            {
                if (button.TryGetComponent(out TaskInteractable taskInteractable))
                {
                    tasksRemaining.Add(taskInteractable);
                    taskInteractable.onTaskComplete += () =>
                    {
                        noOfCompletedTasks++;
                        tasksRemaining.Remove(taskInteractable);
                        AddNewTasks(1);
                    };
                }
            }
        }
        
        // taskButtonSpawner.StartSpawn(spawnCount, out var taskButtons);
        // foreach (var button in taskButtons)
        // {
        //     if (button.TryGetComponent(out TaskInteractable taskInteractable))
        //     {
        //         tasksRemaining.Add(taskInteractable);
        //         taskInteractable.onTaskComplete += () =>
        //         {
        //             noOfCompletedTasks++;
        //             tasksRemaining.Remove(taskInteractable);
        //             AddNewTasks(1);
        //         };
        //     }
        // }
        
        onNumberOfCompletedTasksUpdated?.Invoke(noOfCompletedTasks);
    }

    public void ResetNoOfCompletedTasks()
    {
        noOfCompletedTasks = 0;
        onNumberOfCompletedTasksUpdated?.Invoke(noOfCompletedTasks);
    }

    public void DestroyAllTasks()
    {
        foreach (var button in tasksRemaining)
        {
            button.Despawn();
        }
        tasksRemaining.Clear();
    }
}
