using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class GUI_TasksCompleted : MonoBehaviour
{
    [SerializeField] private TaskPlacementManager taskPlacementManager;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private TextMeshPro text;

    void Awake()
    {
        Assert.IsNotNull(taskPlacementManager);
        taskPlacementManager.onNumberOfCompletedTasksUpdated += UpdateText;
    }

    private void UpdateText(int noOfCompletedTasks)
    {
        if (textUI)
            textUI.text = noOfCompletedTasks.ToString();
        
        if (text)
            text.text = noOfCompletedTasks.ToString();
    }
}
