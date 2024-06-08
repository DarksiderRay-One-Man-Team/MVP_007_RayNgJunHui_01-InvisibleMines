using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class GUI_EquipmentDescription : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshPro titleText;
    [SerializeField] private TextMeshPro descriptionText;

    [Header("Equipment Data")]
    [SerializeField] private EquipmentData equipmentData;

    void Awake()
    {
        Assert.IsNotNull(titleText);
        Assert.IsNotNull(descriptionText);

        if (equipmentData)
            UpdateUI();
    }
    
    void UpdateEquipmentData(EquipmentData equipmentData)
    {
        this.equipmentData = equipmentData;
        UpdateUI();
    }

    void UpdateUI()
    {
        titleText.text = equipmentData.equipmentName;
        descriptionText.text = equipmentData.description;
    }
}
