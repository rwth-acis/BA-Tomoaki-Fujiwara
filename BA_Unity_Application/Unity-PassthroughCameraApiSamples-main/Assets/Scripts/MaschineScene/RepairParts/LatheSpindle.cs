using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatheSpindle : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BrokenPartFixableByLatheSpindle"))
        {
            Debug.Log("LatheSpindle hit brokenPart");
            // Call the method on the BrokenPart script to repair the part
            BrokenPartFixableByLatheSpindle brokenPart = other.GetComponent<BrokenPartFixableByLatheSpindle>();
            if (brokenPart != null)
            {
                brokenPart.LatheSpindleInPlace();
            }
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("BrokenPartFixableByLatheSpindle"))
        {
            Debug.Log("LatheSpindle hit brokenPart");
            // Call the method on the BrokenPart script to stop repairing the part
            BrokenPartFixableByLatheSpindle brokenPart = other.GetComponent<BrokenPartFixableByLatheSpindle>();
            if (brokenPart != null)
            {
                brokenPart.LatheSpindleOutOfPlace();
            }
        }
    }


}
