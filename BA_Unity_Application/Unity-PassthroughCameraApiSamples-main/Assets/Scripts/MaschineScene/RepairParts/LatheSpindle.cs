using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatheSpindle : MonoBehaviour
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

    public void ResetPosition()
    {

        transform.localPosition = initialLocalPosition;
        transform.localRotation = initialLocalRotation;
    }

}
