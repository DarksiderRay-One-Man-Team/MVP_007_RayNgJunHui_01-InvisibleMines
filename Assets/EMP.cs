using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : MonoBehaviour
{

    public LethalCheck TriggerSphereLeft;
    public LethalCheck TriggerSphereRight;

    public void _EMP()
    {


        TriggerSphereLeft.SwitchState();
        TriggerSphereRight.SwitchState();
        
    }
}
