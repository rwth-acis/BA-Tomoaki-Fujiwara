using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProMill : MonoBehaviour
{
    public enum ProMillStatus { Running, Idle }
    public ProMillStatus proMillStatus = ProMillStatus.Idle;
    public Animator proMillAnimator;
    public bool hasWheelOnIt = false;

    public BrokenPartFixableByMillingCutter millingCutterBrokenPart;
    public GameObject millingCutterBrokenMarker;

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

    // This is for LLM
    public Dictionary<string, object> ScanMillStatus()
    {

        string brokenParts = "";

        if (millingCutterBrokenPart.brokenStatus())
        {
            millingCutterBrokenMarker.SetActive(true);
            brokenParts =" milling cutter ";
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
                { "message", "There is an error in milling machine at part {brokenParts}. The user should grab the other milling cutter and replace it with error part." }
        };


    }

    public void RemoveWheelOnIt()
    {
        hasWheelOnIt = false;
    }

    public void PartsBreakeMillingCutter()
    {
        //boxColliderHandGrabber.enabled = false;
        millingCutterBrokenPart.BrokeTheJoint();
    }

}
