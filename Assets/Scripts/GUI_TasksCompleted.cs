using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class GUI_TasksCompleted : MonoBehaviour
{
    [SerializeField] private TaskPlacementManager taskPlacementManager;
    [SerializeField] private TextMeshProUGUI text;

    void Awake()
    {
        Assert.IsNotNull(taskPlacementManager);
        taskPlacementManager.onNumberOfCompletedTasksUpdated += UpdateText;
    }

    private void UpdateText(int noOfCompletedTasks)
    {
        text.text = noOfCompletedTasks.ToString();
    }
}
