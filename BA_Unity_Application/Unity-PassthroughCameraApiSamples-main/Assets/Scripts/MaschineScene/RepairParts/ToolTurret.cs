using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTurret : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BrokenPartFixableByToolTurret"))
        {
            Debug.Log("ToolTurret hit brokenPart");
            // Call the method on the BrokenPart script to repair the part
            BrokenPartFixableByToolTurret brokenPart = other.GetComponent<BrokenPartFixableByToolTurret>();
            if (brokenPart != null)
            {
                brokenPart.ToolTurretInPlace();
            }
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("BrokenPartFixableByToolTurret"))
        {
            Debug.Log("ToolTurret hit brokenPart");
            // Call the method on the BrokenPart script to stop repairing the part
            BrokenPartFixableByToolTurret brokenPart = other.GetComponent<BrokenPartFixableByToolTurret>();
            if (brokenPart != null)
            {
                brokenPart.ToolTurretOutOfPlace();
            }
        }
    }


}
