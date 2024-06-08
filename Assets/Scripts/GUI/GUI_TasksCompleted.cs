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

    [Header("Text Format")]
    [SerializeField] private bool followTextFormat;

    [SerializeField] private string textFormat = "D2";

    void Awake()
    {
        Assert.IsNotNull(taskPlacementManager);
        taskPlacementManager.onNumberOfCompletedTasksUpdated += UpdateText;
    }

    private void UpdateText(int noOfCompletedTasks)
    {
        var textString = followTextFormat? noOfCompletedTasks.ToString(textFormat) : noOfCompletedTasks.ToString();
        
        if (textUI)
            textUI.text = textString;
        
        if (text)
            text.text = textString;
    }
}
