using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SonicEmitter : MonoBehaviour
{
    public float _EmitterRadius = 10.0f;
    public int ammoCount = 3;
    
    private Vector3 initialScale;
    public Transform _paticles;
    public float _ScaleFactor = 2.0f;
    public float _ScaleDuration = 1.0f;

    void Start()
    {
        initialScale = transform.localScale;
    }

    
    void Update()
    {
        
    }
    public void _SonicEmitter()
    {
       ScaleUp();
        Collider[] Objects = Physics.OverlapSphere(transform.position, _EmitterRadius);

        foreach (Collider Object in Objects)
        {
            Mine mine = Object.GetComponent<Mine>();
            if (mine != null)
            {
                mine.Explode();
                //Debug.LogWarning("Explode");
            }
        }
        
    }
    public void ScaleUp()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleOverTime(initialScale * _ScaleFactor));
        Invoke("ScaleDown", _ScaleDuration);
    }

    
    public void ScaleDown()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleOverTime(initialScale));

    }

    
    private IEnumerator ScaleOverTime(Vector3 targetScale)
    {
        Vector3 startScale = _paticles.localScale;
        float elapsedTime = 0;

        while (elapsedTime < _ScaleDuration)
        {
            _paticles.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / _ScaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _paticles.localScale = targetScale;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _EmitterRadius);
    }
}
