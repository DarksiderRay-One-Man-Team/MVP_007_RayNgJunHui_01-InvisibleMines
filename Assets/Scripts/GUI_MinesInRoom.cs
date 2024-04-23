using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class GUI_MinesInRoom : MonoBehaviour
{
    [SerializeField] private MinePlacementManager minePlacementManager;
    [SerializeField] private TextMeshProUGUI text;

    void Awake()
    {
        Assert.IsNotNull(minePlacementManager);
        minePlacementManager.onNumberOfMinesUpdated += UpdateText;
    }

    private void UpdateText(int noOfCompletedTasks)
    {
        text.text = noOfCompletedTasks.ToString();
    }
}
