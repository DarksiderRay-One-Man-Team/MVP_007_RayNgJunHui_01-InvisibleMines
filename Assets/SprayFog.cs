using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayFog : MonoBehaviour
{
    private float lifeTime = 7f;

    void Start()
    {
        Invoke(nameof(Destroy), lifeTime);
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent(out Mine mine))
        {
            mine.RevealTemp();
        }
    }

    void Destroy()
    {
        Destroy(this.gameObject);
    }
}
