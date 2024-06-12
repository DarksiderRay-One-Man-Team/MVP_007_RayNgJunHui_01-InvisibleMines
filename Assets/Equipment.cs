using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [Header("Ammo Count")]
    [SerializeField] private int currentAmmoCount;
    [SerializeField] private int maxAmmoCount = 5;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private string ammoTextFormat = "Ammo: {0}/{1}";


    public void ResetAmmo()
    {
        currentAmmoCount = maxAmmoCount;
        UpdateAmmoDisplay();
    }

    public void DecrementAmmo()
    {
        currentAmmoCount--; 
        UpdateAmmoDisplay();
    }

    public bool CheckAmmoCount()
    {
        return currentAmmoCount > 0;
    }
    
    private void UpdateAmmoDisplay()
    {
        if (ammoText != null)
        {
            ammoText.text = string.Format(ammoTextFormat, currentAmmoCount, maxAmmoCount);
        }
    }
}
