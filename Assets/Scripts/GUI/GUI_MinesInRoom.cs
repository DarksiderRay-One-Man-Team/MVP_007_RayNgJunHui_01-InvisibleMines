using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class GUI_MinesInRoom : MonoBehaviour
{
    [SerializeField] private MinePlacementManager minePlacementManager;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private TextMeshPro text;

    void Awake()
    {
        Assert.IsNotNull(minePlacementManager);
        minePlacementManager.onNumberOfMinesUpdated += UpdateText;
    }

    private void UpdateText(int noOfCompletedTasks)
    {
        if (textUI)
            textUI.text = noOfCompletedTasks.ToString();
        
        if (text)
            text.text = noOfCompletedTasks.ToString();
    }
}
