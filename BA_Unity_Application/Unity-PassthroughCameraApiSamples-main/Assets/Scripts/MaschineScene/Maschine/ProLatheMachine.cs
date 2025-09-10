using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProLatheMachine : MonoBehaviour
{

    public enum LatheMachineStatus { Running, Idle }
    public LatheMachineStatus latheMachineStatus = LatheMachineStatus.Idle;

    public Animator latheMachineAnimator;
    public bool hasWheelOnIt = false;

    public BrokenPartFixableByLatheSpindle latheSpindleBrokenPart;
    public BrokenPartFixableByToolTurret toolTurretBrokenPart;
    public GameObject latheSpindleBrokenMarker;
    public GameObject toolTurretBrokenMarker;


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
    public void RemoveWheelOnIt()
    {
        hasWheelOnIt = false;
    }

    public bool HasWheelOnIt()
    {
        return hasWheelOnIt;
    }

    // This is for LLM
    public Dictionary<string, object> ScanLatheMachineStatus()
    {

        string brokenParts = "";

        if (latheSpindleBrokenPart.brokenStatus())
        {
            latheSpindleBrokenMarker.SetActive(true);
            brokenParts = " lathe Spindle ";

            return new Dictionary<string, object> {
                { "status", "success" },
                { "message", "There is an error in lathe machine at part {brokenParts}. The user should grab new lathe spindle and replace it with error part." }
            };
        }

        if (toolTurretBrokenPart.brokenStatus())
        {
            toolTurretBrokenMarker.SetActive(true);
            brokenParts = " tool turret ";
            return new Dictionary<string, object> {
                { "status", "success" },
                { "message", "There is an error in lathe machine at part {brokenParts}. The user should grab new tool turret and replace it with error part." }
            };
        }


        if (brokenParts == "")
        {
            return new Dictionary<string, object> {
                { "status", "success" },
                { "message", "No errors are found by scanning" }
            };
        }

        return new Dictionary<string, object> {
                { "status", "success" },
                { "message", "There is an error in lathe machine at part {brokenParts}." }
        };


    }
}
