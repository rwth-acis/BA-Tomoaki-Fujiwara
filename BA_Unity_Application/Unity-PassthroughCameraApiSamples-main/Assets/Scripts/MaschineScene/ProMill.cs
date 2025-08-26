using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProMill : MonoBehaviour
{
    public enum ProMillStatus { Running, Idle }
    public ProMillStatus proMillStatus = ProMillStatus.Idle;
    public Animator proMillAnimator;
    public bool hasWheelOnIt = false;

    public void StartProMill()
    {
        proMillAnimator.SetBool("Running", true);
        proMillStatus = ProMillStatus.Running;
    }

    public void StopProMill()
    {
        proMillAnimator.SetBool("Running", false);
        proMillStatus = ProMillStatus.Idle;
    }

    public ProMillStatus ReturnStatus()
    {
        return proMillStatus;
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
