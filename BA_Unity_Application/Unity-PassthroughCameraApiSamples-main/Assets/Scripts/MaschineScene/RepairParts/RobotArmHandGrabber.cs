using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmHandGrabber : MonoBehaviour
{

    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    void Start()
    {

        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BrokenPartFixableByRobotArmHandGrabber"))
        {
            Debug.Log("RobotArmHandGrabber hit brokenPart");
            // Call the method on the BrokenPart script to repair the part
            BrokenPartFixableByRobotArmHandGrabber brokenPart = other.GetComponent<BrokenPartFixableByRobotArmHandGrabber>();
            if (brokenPart != null)
            {
                brokenPart.RobotArmHandGrabberInPlace();
            }
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("BrokenPartFixableByRobotArmHandGrabber"))
        {
            Debug.Log("RobotArmHandGrabber hit brokenPart");
            // Call the method on the BrokenPart script to stop repairing the part
            BrokenPartFixableByRobotArmHandGrabber brokenPart = other.GetComponent<BrokenPartFixableByRobotArmHandGrabber>();
            if (brokenPart != null)
            {
                brokenPart.RobotArmHandGrabberOutOfPlace();
            }
        }
    }

    public void ResetPosition()
    {

        transform.localPosition = initialLocalPosition;
        transform.localRotation = initialLocalRotation;
    }

}
