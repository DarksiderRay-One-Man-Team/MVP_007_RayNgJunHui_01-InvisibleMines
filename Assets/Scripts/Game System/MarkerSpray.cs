using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.HandGrab;
using TMPro;
using UnityEngine;

public class MarkerSpray : MonoBehaviour, IHandGrabUseDelegate
{
    [Header("Ammo Count")]
    [SerializeField] private int currentAmmoCount;
    [SerializeField] private int maxAmmoCount = 5;
    [SerializeField] private TextMeshProUGUI ammoText;
    
    [SerializeField] private GameObject _fogPrefab;
    [SerializeField] private float _fogInstantiationDistance = 0.3f;
    [SerializeField] private Transform _triggerTransform;
    [SerializeField] private Transform _particleSprayOffset;
    private float _fireThresold = 0.8f;
    [SerializeField] ParticleSystem _particleSystem;
    private float _durationWhenIncreasing = 1f;
    private float _increaseScaleIn = 1.1f;
    private float _fogInstantiationInterval = 1f;
    private float _triggerStartRotation = -90;
    private float _triggerEndRotation = -65;
    private float _dampedUseStrength = 0;
    [SerializeField] private float _triggerSpeed = 6f;
    private float _lastUseTime;
    private AnimationCurve _strengthCurve = AnimationCurve.EaseInOut(0f,0f,1f,1f);
    private List<GameObject> _listOfInstances = new();
    private Dictionary<Transform, Coroutine> _dictOfActiveCoroutines = new();

    private bool alreadySprayed = false;

    void Start()
    {
        currentAmmoCount = maxAmmoCount;
        UpdateAmmoDisplay();   
    }
    
    public void StartSpraying()
    {
        if (alreadySprayed || currentAmmoCount == 0)
            return;

        alreadySprayed = true;
        currentAmmoCount--;
        UpdateAmmoDisplay();
        _particleSystem.Play();
        //InvokeRepeating(nameof(Spray), 0, _fogInstantiationInterval);
        Spray();
    }
    public void StopSpraying()
    {
        alreadySprayed = false;
        _particleSystem.Stop();
        CancelInvoke();
    }
    public void RemoveFog(){
        StopAllCoroutines();
        _listOfInstances.ForEach((x)=>Destroy(x));        
        _listOfInstances.Clear();
        _dictOfActiveCoroutines.Clear();
    }
    private void Spray(){
        // RaycastHit hit;
        // if (Physics.Raycast(_particleSprayOffset.position, _particleSprayOffset.forward, out hit, _fogInstantiationDistance)){
        //     if (_dictOfActiveCoroutines.ContainsKey(hit.transform)){
        //         StopCoroutine(_dictOfActiveCoroutines[hit.transform]);
        //         _dictOfActiveCoroutines.Remove(hit.transform);
        //     }
        //     var coroutine = StartCoroutine(IncreaseScaleSmoothly(hit.transform));
        //     _dictOfActiveCoroutines.Add(hit.transform, coroutine);
        // }
        // else
        // {
            Vector3 spawnPosition = _particleSprayOffset.position + _particleSprayOffset.forward * _fogInstantiationDistance;
            _listOfInstances.Add(Instantiate(_fogPrefab, spawnPosition, _particleSprayOffset.rotation));
        //}
    }
    private IEnumerator IncreaseScaleSmoothly(Transform targetTransform){
        Vector3 originalScale = targetTransform.localScale;
        Vector3 targetScale = originalScale * _increaseScaleIn;
        float duration = _durationWhenIncreasing;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            targetTransform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetTransform.localScale = targetScale;
        _dictOfActiveCoroutines.Remove(targetTransform);
    }

    public void BeginUse()
    {
        Debug.Log("Begin Use");
        _dampedUseStrength = 0;
        _lastUseTime = Time.realtimeSinceStartup;
    }

    public void EndUse()
    {
        Debug.Log("End Use");
        StopSpraying();
    }

    public float ComputeUseStrength(float strength)
    {
        float delta = Time.realtimeSinceStartup - _lastUseTime;
        _lastUseTime = Time.realtimeSinceStartup;
         if (strength > _dampedUseStrength)
        {
            _dampedUseStrength = Mathf.Lerp(_dampedUseStrength, strength, _triggerSpeed * delta);
        }
        else
        {
            _dampedUseStrength = strength;
        }
        float progress = _strengthCurve.Evaluate(_dampedUseStrength);
        UpdateTriggerProgress(progress);
        return progress;
    }

    private void UpdateTriggerProgress(float progress){
        Vector3 angles = _triggerTransform.localEulerAngles;
        angles.x = Mathf.Lerp(_triggerStartRotation, _triggerEndRotation, progress);
        _triggerTransform.localEulerAngles = angles;

        if (progress >= _fireThresold)
            StartSpraying();

    }
    
    private void UpdateAmmoDisplay()
    {
        if (ammoText != null)
        {
            ammoText.text = $"Sprays: {currentAmmoCount}/{maxAmmoCount}";
        }
    }
}
