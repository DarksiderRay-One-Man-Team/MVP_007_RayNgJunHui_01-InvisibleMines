using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class LethalCheck : MonoBehaviour
{
    public delegate void OnLethalInvoked();
    public OnLethalInvoked onLethalInvoked;

    public enum State
    {
        Normal,
        EMP
    }

    private enum Fire
    {
        OffFire,
        OnFire
    }

    private Fire _fire;
    public State currentState;
    public float _EMPTime = 5.0f;
    public TextMeshProUGUI countdownText;


    public int maxAmmo = 3;
    private int currentAmmo;
    public TextMeshProUGUI ammoText;


    public UnityEvent onEMPFired;
    public UnityEvent onEMPCountdownComplete;
    public UnityEvent onOutOfAmmo;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoDisplay();
    }

    void Update()
    {
        Debug.LogWarning(currentState);
        switch (currentState)
        {
            case State.EMP:
                HandleEMPState();
                break;
            case State.Normal:
                HandleNormalState();
                break;
            default:
                break;
        }
    }

    public void SwitchState()
    {
        if (currentState == State.EMP)
        {
            currentState = State.Normal;
        }
        else
        {
            currentState = State.EMP;
        }
    }

    void HandleEMPState()
    {
        if (_fire == Fire.OffFire)
        {
            if (currentAmmo > 0)
            {

                onEMPFired?.Invoke();
                StartCoroutine(_empDelay(_EMPTime));
                currentAmmo--;
                UpdateAmmoDisplay();
            }
            else
            {
                Debug.LogWarning("Out of ammo!");
                onOutOfAmmo?.Invoke();
            }
        }
        else
        {
            Debug.LogWarning("Can't fire now");
        }
    }

    void HandleNormalState()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent(out Mine mine) && currentState == State.Normal)
        {
            mine.Explode();
            onLethalInvoked?.Invoke();
        }
    }

    IEnumerator _empDelay(float delayTime)
    {
        float elapsedTime = 0f;
        _fire = Fire.OnFire;

        while (elapsedTime < delayTime)
        {
            float remainingTime = delayTime - elapsedTime;
            countdownText.text = $"Time remaining: {remainingTime:F1} seconds";
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        countdownText.text = "Time remaining: 0.0 seconds";
        currentState = State.Normal;
        _fire = Fire.OffFire;


        onEMPCountdownComplete?.Invoke();
    }




    void UpdateAmmoDisplay()
    {
        if (ammoText != null)
        {
            ammoText.text = $"EMP: {currentAmmo}/{maxAmmo}";
        }
    }
}

