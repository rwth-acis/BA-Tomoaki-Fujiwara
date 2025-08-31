using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmManager : MonoBehaviour
{
    public Animator robotArmAnimator;
    public Wheel holdWheel;

    public enum RobotArmStatus { Idle, Running }
    public RobotArmStatus robotArmStatus = RobotArmStatus.Idle;


    public ProLatheMachine proLatheMachine;
    public FlipTable flipTableMachine;
    public ProMill proMill;
    public Convyor2 convyor2;


    public BrokenPart foundationBrokenPart;
    public BrokenPart lowerArmBrokenPart;
    public BrokenPart lowerUpperArmBrokenPart;
    public BrokenPart handGrabberBrokenPart;



    public GameObject foundationBrokenMarker;
    public GameObject lowerArmBrokenMarker;
    public GameObject lowerUpperArmBrokenMarker;
    public GameObject handGrabberBrokenMarker;


    //public BoxCollider boxColliderHandGrabber;



    public Dictionary<string, object> PutWheelInLatheMachine()
    {
        Debug.Log("ProLathe Button");
        if (robotArmStatus == RobotArmStatus.Running) {
            // RobotArm is doing other action

            Debug.Log("Robot Arm is busy now");

            return new Dictionary<string, object> {
                { "status", "error" },
                { "message", "The robot arm is carrying wheel, please wait until the current process finish." }
            };
        }

        ProLatheMachine.LatheMachineStatus currentStatus = proLatheMachine.ReturnStatus();

        if (currentStatus == ProLatheMachine.LatheMachineStatus.Running) {
            // ProLathe is running, cannot put wheel on it

            Debug.Log("ProLathe is busy now");

            return new Dictionary<string, object> {
                { "status", "error" },
                { "message", "The lather machine is running, please wait until the current process finish." }
            };
        }

        robotArmStatus = RobotArmStatus.Running;

        if (foundationBrokenPart.brokenStatus())
        {
            robotArmAnimator.Play("RobotArm_FoundationBroke_Lathe");
            Debug.Log("Robot Arm Foundation broke");

        }
        else if (lowerArmBrokenPart.brokenStatus())
        {
            robotArmAnimator.Play("RobotArm_LowerArmBroke_Lathe");
            Debug.Log("Robot Arm LowerArm broke");
        }

        else if (lowerUpperArmBrokenPart.brokenStatus())
        {
            robotArmAnimator.Play("RobotArm_LowerUpperArmBroke_Lathe");
            Debug.Log("Robot Arm lower upper arm broke");
        }

        else if (handGrabberBrokenPart.brokenStatus())
        {
            robotArmAnimator.Play("RobotArm_Hand_GrabberBroke_Lathe");
            Debug.Log("Robot Arm Hand Grabber broke");
        }

        else
        {
            robotArmAnimator.Play("RobotArm_PlaceWheelInLether");
            Debug.Log("Robot Arm moved");
        }

        return new Dictionary<string, object> {
                { "status", "success" },
                { "message", "The robot arm has carryed wheel to lather machine." }
        };

    }

    public void StartProLathe() {
        proLatheMachine.StartLatheMachine();
    }

    public Dictionary<string, object> PutWheelInFlipTable()
    {
        Debug.Log("FlipTable Button");
        if (robotArmStatus == RobotArmStatus.Running)
        {
            Debug.Log("Robot Arm is busy now");
            // RobotArm is doing other action
            return new Dictionary<string, object> {
                { "status", "error" },
                { "message", "The robot arm is carrying wheel, please wait until the current process finish." }
            };
        }

        FlipTable.FlipTableStatus currentStatus = flipTableMachine.ReturnStatus();

        if (currentStatus == FlipTable.FlipTableStatus.Running)
        {
            // ProLathe is running, cannot put wheel on it

            Debug.Log("FlipTable is busy now");

            return new Dictionary<string, object> {
                { "status", "error" },
                { "message", "The flip table is running, please wait until the current process finish." }
            };
        }

        robotArmStatus = RobotArmStatus.Running;
        robotArmAnimator.Play("RobotArm_PlaceWheelInTable");

        return new Dictionary<string, object> {
                { "status", "success" },
                { "message", "The robot arm has carryed wheel to flip table machine." }
        };

    }

    public void StartFlipTable()
    {
        flipTableMachine.StartFlipTable();
    }

    public Dictionary<string, object> PutWheelInMillingMachine()
    {
        Debug.Log("Mill Button");
        if (robotArmStatus == RobotArmStatus.Running)
        {
            Debug.Log("Robot Arm is busy now");
            return new Dictionary<string, object> {
                { "status", "error" },
                { "message", "The robot arm is carrying wheel, please wait until the current process finish." }
            };
        }

        ProMill.ProMillStatus currentStatus = proMill.ReturnStatus();

        if (currentStatus == ProMill.ProMillStatus.Running)
        {

            Debug.Log("ProMil is busy now");
            // ProLathe is running, cannot put wheel on it
            return new Dictionary<string, object> {
                { "status", "error" },
                { "message", "The Milling Machine is running, please wait until the current process finish." }
            };
        }

        robotArmStatus = RobotArmStatus.Running;
        robotArmAnimator.Play("RobotArm_PlaceWheelInMill");

        return new Dictionary<string, object> {
                { "status", "success" },
                { "message", "The robot arm has carryed wheel to milling machine." }
        };

    }

    public void StartProMill()
    {
        proMill.StartProMill();
    }

    public Dictionary<string, object> PutWheelConvyor2()
    {
        Debug.Log("OnConvyor");
        if (robotArmStatus == RobotArmStatus.Running)
        {
            Debug.Log("Robot Arm is busy now");
            return new Dictionary<string, object> {
                { "status", "error" },
                { "message", "The robot arm is carrying wheel, please wait until the current process finish." }
            };
        }

        Convyor2.ConvyerStatus currentStatus = convyor2.ReturnStatus();

        if (currentStatus == Convyor2.ConvyerStatus.Running)
        {
            Debug.Log("Conyor is busy now");
            // ProLathe is running, cannot put wheel on it
            return new Dictionary<string, object> {
                { "status", "error" },
                { "message", "The convyor2 is running, please wait until the current process finish." }
            };
        }

        robotArmStatus = RobotArmStatus.Running;
        robotArmAnimator.Play("RobotArm_PlaceWheelOnBand");

        return new Dictionary<string, object> {
                { "status", "success" },
                { "message", "The robot arm has carryed wheel to convyor." }
        };


    }

    public void StartConvyor2()
    {
        convyor2.StartConvyor();
    }


    public void FinishAction()
    {
        robotArmStatus = RobotArmStatus.Idle;
    }

    public void SetHoldWheel(Wheel target)
    {
        holdWheel = target;
    }

    public void ReleaseWheel()
    {
        if (holdWheel != null)
        {
            holdWheel.Release();
        }
    }

    public void ResetWheelInformation()
    {
        ReleaseWheel();
        holdWheel = null;
    }

    public void ScanButton()
    {
        // Scan for broken parts
        ScanRobotArmStatus();
    }

    // This is for LLM
    public Dictionary<string, object> ScanRobotArmStatus()
    {

        string brokenParts = "";

        if (foundationBrokenPart.brokenStatus())
        {
            foundationBrokenMarker.SetActive(true);
            brokenParts = brokenParts + " Foundation ";
        }

        if (lowerArmBrokenPart.brokenStatus())
        {
            lowerArmBrokenMarker.SetActive(true);
            brokenParts = brokenParts + " Lower Arm ";
        }

        if (lowerUpperArmBrokenPart.brokenStatus())
        {
            lowerUpperArmBrokenMarker.SetActive(true);
            brokenParts = brokenParts + " Lower Upper Arm ";
        }

        if (handGrabberBrokenPart.brokenStatus())
        {
            handGrabberBrokenMarker.SetActive(true);
            brokenParts = brokenParts + " Hand Grabber ";
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
                { "message", "There is an error in robot arm at part {brokenParts}." }
        };


    }


    public void PartsBreakeFoundation() { 
        foundationBrokenPart.BrokeTheJoint();
    }

    public void PartsBreakeLowerArm() {
        lowerArmBrokenPart.BrokeTheJoint();
    }

    public void PartsBreakeLowerUpperArm() {
        lowerUpperArmBrokenPart.BrokeTheJoint();
    }

    public void PartsBreakeHandGrabber(){
        //boxColliderHandGrabber.enabled = false;
        handGrabberBrokenPart.BrokeTheJoint();
    }

}
