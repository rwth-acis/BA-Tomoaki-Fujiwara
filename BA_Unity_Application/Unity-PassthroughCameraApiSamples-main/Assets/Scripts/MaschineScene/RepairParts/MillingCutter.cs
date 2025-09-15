using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillingCutter : MonoBehaviour
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
        if (other.gameObject.layer == LayerMask.NameToLayer("BrokenPartFixableByMillingCutter"))
        {
            Debug.Log("MillingCutter hit brokenPart");
            // Call the method on the BrokenPart script to repair the part
            BrokenPartFixableByMillingCutter brokenPart = other.GetComponent<BrokenPartFixableByMillingCutter>();
            if (brokenPart != null)
            {
                brokenPart.MillingCutterInPlace();
            }
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("BrokenPartFixableByMillingCutter"))
        {
            Debug.Log("MillingCutter hit brokenPart");
            // Call the method on the BrokenPart script to stop repairing the part
            BrokenPartFixableByMillingCutter brokenPart = other.GetComponent<BrokenPartFixableByMillingCutter>();
            if (brokenPart != null)
            {
                brokenPart.MillingCutterOutOfPlace();
            }
        }
    }

    public void ResetPosition()
    {

        transform.localPosition = initialLocalPosition;
        transform.localRotation = initialLocalRotation;
    }


}
