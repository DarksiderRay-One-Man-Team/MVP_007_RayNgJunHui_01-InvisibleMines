using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial Data - ", menuName = "ScriptableObjects/Tutorial Data", order = 1)]
public class TutorialData : ScriptableObject
{
    public GameObject sceneObject;
    public string text;
}
