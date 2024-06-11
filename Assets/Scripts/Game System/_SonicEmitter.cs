using System.Collections;
using UnityEngine;
using TMPro; 

public class _SonicEmitter : MonoBehaviour
{
    public float _EmitterRadius = 10.0f;
    public int maxAmmoCount = 3;
    private int currentAmmoCount;

    private Vector3 initialScale;
    public Transform _particles;
    public float _ScaleFactor = 2.0f;
    public float _ScaleDuration = 1.0f;
    public float delay = 1.0f;

    public TextMeshProUGUI ammoText;  
    

    void Start()
    {
        initialScale = _particles.localScale;
        currentAmmoCount = maxAmmoCount; 
        UpdateAmmoDisplay();   
    }

    void Update()
    {

    }

    public void SonicEmitter()
    {
        if (currentAmmoCount > 0)
        {
            ScaleUp();
            Collider[] objects = Physics.OverlapSphere(transform.position, _EmitterRadius);

            foreach (Collider obj in objects)
            {
                Mine mine = obj.GetComponent<Mine>();
                if (mine != null)
                {
                    mine.Explode();
                }
            }

            currentAmmoCount--; 
            UpdateAmmoDisplay(); 
        }
        
    }

    public void ScaleUp()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleOverTime(initialScale * _ScaleFactor));
        StartCoroutine(CallFunctionAfterDelay(delay));
    }

    public void ScaleDown()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleOverTime(initialScale));
    }

    private IEnumerator ScaleOverTime(Vector3 targetScale)
    {
        Vector3 startScale = _particles.localScale;
        float elapsedTime = 0;

        while (elapsedTime < _ScaleDuration)
        {
            _particles.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / _ScaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _particles.localScale = targetScale;
    }

    private IEnumerator CallFunctionAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        ScaleDown();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _EmitterRadius);
    }

    
    private void UpdateAmmoDisplay()
    {
        if (ammoText != null)
        {
            ammoText.text = $"Sonic: {currentAmmoCount}/{maxAmmoCount}";
        }
    }

    
}
