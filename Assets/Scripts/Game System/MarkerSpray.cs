using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerSpray : MonoBehaviour
{
    [SerializeField] private GameObject _fogPrefab;
    [SerializeField] private float _fogInstantiationDistance = 0.3f;
    private float _durationWhenIncreasing = 1f;
    private float _increaseScaleIn = 1.1f;
    private float _fogInstantiationInterval = 1f;
    private List<GameObject> _listOfInstances = new();
    private Dictionary<Transform, Coroutine> _dictOfActiveCoroutines = new();

    public void StartSpraying(){
        InvokeRepeating(nameof(Spray), 0, _fogInstantiationInterval);
    }
    public void StopSpraying(){
        CancelInvoke();
    }
    public void RemoveFog(){
        StopAllCoroutines();
        _listOfInstances.ForEach((x)=>Destroy(x));        
        _listOfInstances.Clear();
        _dictOfActiveCoroutines.Clear();
    }
    private void Spray(){
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _fogInstantiationDistance)){
            if (_dictOfActiveCoroutines.ContainsKey(hit.transform)){
                StopCoroutine(_dictOfActiveCoroutines[hit.transform]);
                _dictOfActiveCoroutines.Remove(hit.transform);
            }
            var coroutine = StartCoroutine(IncreaseScaleSmoothly(hit.transform));
            _dictOfActiveCoroutines.Add(hit.transform, coroutine);
        }
        else
        {
            Vector3 spawnPosition = transform.position + transform.forward * _fogInstantiationDistance;
            _listOfInstances.Add(Instantiate(_fogPrefab, spawnPosition, transform.rotation));
        }
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
}
