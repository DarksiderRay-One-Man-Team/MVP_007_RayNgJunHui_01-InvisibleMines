using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerSpray : MonoBehaviour
{
    [SerializeField] private GameObject _sprayPrefab;
    [SerializeField] private float _sprayDistance = 1.5f;

    private List<GameObject> listOfInstances = new();

    public void Spray(){
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _sprayDistance))
            hit.transform.localScale *= 1.1f;
        else
        {
            Vector3 spawnPosition = transform.position + transform.forward * _sprayDistance;
            listOfInstances.Add(Instantiate(_sprayPrefab, spawnPosition, transform.rotation));
        }
    }

    public void RemoveFog(){
        Debug.Log("Deleting Fog");
        for(int i=0;i<listOfInstances.Count;i++)
            Destroy(listOfInstances[i]);
        
        listOfInstances.Clear();
    }
}
