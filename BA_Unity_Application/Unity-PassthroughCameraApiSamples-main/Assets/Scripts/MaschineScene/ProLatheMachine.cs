using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProLatheMachine : MonoBehaviour
{

    public enum LatheMachineStatus { Running, Idle }
    public LatheMachineStatus latheMachineStatus = LatheMachineStatus.Idle;

    public Animator latheMachineAnimator;
    public bool hasWheelOnIt = false;

    public void StartLatheMachine()
    {
        latheMachineAnimator.SetBool("Running", true);
        latheMachineStatus = LatheMachineStatus.Running;
    }

    public void StopLatheMachine()
    {
        latheMachineAnimator.SetBool("Running", false);
        latheMachineStatus = LatheMachineStatus.Idle;
    }


    public LatheMachineStatus ReturnStatus()
    {
        return latheMachineStatus;
    }

    public void PutWheelOnIt()
    {
        hasWheelOnIt = true;
    }

    public bool HasWheelOnIt()
    {
        return hasWheelOnIt;
    }

}
