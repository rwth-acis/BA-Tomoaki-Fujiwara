using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillingCutter : MonoBehaviour
{

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


}
