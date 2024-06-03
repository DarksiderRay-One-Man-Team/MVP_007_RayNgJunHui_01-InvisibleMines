using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    [SerializeField] private Vector2 rotationSpeedRange_X;
    [SerializeField] private Vector2 rotationSpeedRange_Y;
    [SerializeField] private Vector2 rotationSpeedRange_Z;

    private Vector3 rotationSpeed;

    void Start()
    {
        rotationSpeed = 
            new Vector3(Random.Range(rotationSpeedRange_X.x, rotationSpeedRange_X.y),
            Random.Range(rotationSpeedRange_Y.x, rotationSpeedRange_Y.y),
            Random.Range(rotationSpeedRange_Z.x, rotationSpeedRange_Z.y));
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed.x * Time.deltaTime, 
                    rotationSpeed.y * Time.deltaTime, 
                    rotationSpeed.z * Time.deltaTime);
    }
}
