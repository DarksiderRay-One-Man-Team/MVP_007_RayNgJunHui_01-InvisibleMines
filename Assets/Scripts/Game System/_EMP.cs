using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class _EMP : MonoBehaviour
{
    public LethalCheck TriggerSphereLeft;
    public LethalCheck TriggerSphereRight;
    public void BeginUse()
    {
        emp();

    }

    public void emp()
    {


        TriggerSphereLeft.SwitchState();
        TriggerSphereRight.SwitchState();

    }
}
