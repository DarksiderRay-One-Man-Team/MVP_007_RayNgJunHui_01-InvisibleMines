using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentLoadoutSystem : MonoBehaviour
{
    [SerializeField] private List<Equipment> equipments = new();


    public void ResetEquipments()
    {
        foreach (var equipment in equipments)
        {
            equipment.ResetAmmo();
        }
    }
}
