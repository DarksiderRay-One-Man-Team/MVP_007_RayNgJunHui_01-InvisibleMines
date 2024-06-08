using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _EMP : MonoBehaviour
{
    public LethalCheck TriggerSphereLeft;
    public LethalCheck TriggerSphereRight;

    public void emp()
    {


        TriggerSphereLeft.SwitchState();
        TriggerSphereRight.SwitchState();

    }
}
