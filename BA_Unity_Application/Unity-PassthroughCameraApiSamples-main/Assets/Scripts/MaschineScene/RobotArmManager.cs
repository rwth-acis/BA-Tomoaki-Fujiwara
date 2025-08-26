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
    public BrokenPart handGrabberBrokenPart;

    public BoxCollider boxColliderHandGrabber;



    public void PutOnProLathe()
    {
        Debug.Log("ProLathe Button");
        if (robotArmStatus == RobotArmStatus.Running) {
            // RobotArm is doing other action

            Debug.Log("Robot Arm is busy now");

            return;
        }

        ProLatheMachine.LatheMachineStatus currentStatus = proLatheMachine.ReturnStatus();

        if (currentStatus == ProLatheMachine.LatheMachineStatus.Running) {
            // ProLathe is running, cannot put wheel on it

            Debug.Log("ProLathe is busy now");

            return;
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

            
    }

    public void StartProLathe() {
        proLatheMachine.StartLatheMachine();
    }

    public void PutOnFlipTable()
    {
        Debug.Log("FlipTable Button");
        if (robotArmStatus == RobotArmStatus.Running)
        {
            Debug.Log("Robot Arm is busy now");
            // RobotArm is doing other action
            return;
        }

        FlipTable.FlipTableStatus currentStatus = flipTableMachine.ReturnStatus();

        if (currentStatus == FlipTable.FlipTableStatus.Running)
        {
            // ProLathe is running, cannot put wheel on it

            Debug.Log("FlipTable is busy now");

            return;
        }

        robotArmStatus = RobotArmStatus.Running;
        robotArmAnimator.Play("RobotArm_PlaceWheelInTable");
    }

    public void StartFlipTable()
    {
        flipTableMachine.StartFlipTable();
    }

    public void PutOnMilling()
    {
        Debug.Log("Mill Button");
        if (robotArmStatus == RobotArmStatus.Running)
        {
            Debug.Log("Robot Arm is busy now");
            return;
        }

        ProMill.ProMillStatus currentStatus = proMill.ReturnStatus();

        if (currentStatus == ProMill.ProMillStatus.Running)
        {

            Debug.Log("ProMil is busy now");
            // ProLathe is running, cannot put wheel on it
            return;
        }

        robotArmStatus = RobotArmStatus.Running;
        robotArmAnimator.Play("RobotArm_PlaceWheelInMill");

    }

    public void StartProMill()
    {
        proMill.StartProMill();
    }

    public void PutOnConvyor2()
    {
        Debug.Log("OnConvyor");
        if (robotArmStatus == RobotArmStatus.Running)
        {
            Debug.Log("Robot Arm is busy now");
            return;
        }

        Convyor2.ConvyerStatus currentStatus = convyor2.ReturnStatus();

        if (currentStatus == Convyor2.ConvyerStatus.Running)
        {
            Debug.Log("Conyor is busy now");
            // ProLathe is running, cannot put wheel on it
            return;
        }

        robotArmStatus = RobotArmStatus.Running;
        robotArmAnimator.Play("RobotArm_PlaceWheelOnBand");


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


    public void PartsBreakeFoundation() { 
        foundationBrokenPart.BrokeTheJoint();
    }

    public void PartsBreakeLowerArm() {
        lowerArmBrokenPart.BrokeTheJoint();
    }

    public void PartsBreakeHandGrabber(){
        boxColliderHandGrabber.enabled = false;
        handGrabberBrokenPart.BrokeTheJoint();
    }

}
