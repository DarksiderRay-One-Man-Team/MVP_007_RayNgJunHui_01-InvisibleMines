using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class GUI_LivesRemainingText : MonoBehaviour
{
    [SerializeField] private InvisibleMinesGameManager mainGameManager;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private TextMeshPro text;

    void Awake()
    {
        Assert.IsNotNull(mainGameManager);
        mainGameManager.onLivesInitialised += UpdateText;
        mainGameManager.onRemainingLivesUpdated += UpdateText;
    }
    
    private void UpdateText(int noOfLives)
    {
        var textString = noOfLives.ToString();
        
        if (textUI)
            textUI.text = textString;
        
        if (text)
            text.text = textString;
    }
}
