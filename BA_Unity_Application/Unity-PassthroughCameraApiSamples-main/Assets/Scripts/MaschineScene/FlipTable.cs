using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipTable : MonoBehaviour
{

    public enum FlipTableStatus { Running, Idle }
    public FlipTableStatus flipTableStatus = FlipTableStatus.Idle;
    public Animator flipTableAnimator;
    public bool hasWheelOnIt = false;

    public void StartFlipTable()
    {
        flipTableAnimator.SetBool("Running", true);
        flipTableStatus = FlipTableStatus.Running;
    }

    public void StopFlipTable()
    {
        flipTableAnimator.SetBool("Running", false);
        flipTableStatus = FlipTableStatus.Idle;
    }

    public FlipTableStatus ReturnStatus()
    {
        return flipTableStatus;
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
