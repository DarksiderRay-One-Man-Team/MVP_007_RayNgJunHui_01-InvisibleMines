using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class _EMP : MonoBehaviour,IHandGrabUseDelegate
{
    public LethalCheck TriggerSphereLeft;
    public LethalCheck TriggerSphereRight;
    public void BeginUse()
    {
        emp();

    }
    public void EndUse()
    {

    }
    public float ComputeUseStrength(float strength)
    {

        return 0;
    }
    public void emp()
    {


        TriggerSphereLeft.SwitchState();
        TriggerSphereRight.SwitchState();

    }
}
