using System.Collections;
using UnityEngine;

public class Scale : MonoBehaviour
{
    // Store the initial scale of the game object
    private Vector3 initialScale;
    // Scale factor to which the object will be scaled up
    public float scaleFactor = 2.0f;
    // Time in seconds over which the scaling will happen
    public float scaleDuration = 1.0f;

    void Start()
    {
        // Store the initial scale when the game starts
        initialScale = transform.localScale;
    }

    // Method to start scaling the object up
    public void ScaleUp()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleOverTime(initialScale * scaleFactor));
    }

    // Method to start scaling the object down to its initial size
    public void ScaleDown()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleOverTime(initialScale));
    }

    // Coroutine to scale the object over time
    private IEnumerator ScaleOverTime(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < scaleDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    // Optional: Example triggers to scale up and down using keyboard input
    /*void Update()
    {
        // Press U to scale up
        if (Input.GetKeyDown(KeyCode.U))
        {
            ScaleUp();
        }

        // Press D to scale down
        if (Input.GetKeyDown(KeyCode.D))
        {
            ScaleDown();
        }
    }*/
}
