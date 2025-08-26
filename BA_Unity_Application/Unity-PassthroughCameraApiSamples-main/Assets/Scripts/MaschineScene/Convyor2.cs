using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Convyor2 : MonoBehaviour
{

    public enum ConvyerStatus { Running, Idle }
    public ConvyerStatus convyorStatus = ConvyerStatus.Idle;

    public Animator convyorAnimator;
    public bool hasWheelOnIt = false;

    public Transform wheel;
    public Transform convyor1WheelSpawn;

    public void StartConvyor()
    {
        hasWheelOnIt = true;
        convyorAnimator.SetBool("IsWheelPlaced", true);
        convyorStatus = ConvyerStatus.Running;
    }

    public void StopConvyor()
    {
        hasWheelOnIt = false;
        convyorAnimator.SetBool("IsWheelPlaced", false);
        convyorStatus = ConvyerStatus.Idle;

        // Rolle of wheel is finish. Now reset it from all
        wheel.SetParent(convyor1WheelSpawn);
        wheel.localPosition = Vector3.zero;

    }

    public ConvyerStatus ReturnStatus()
    {
        return convyorStatus;
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
