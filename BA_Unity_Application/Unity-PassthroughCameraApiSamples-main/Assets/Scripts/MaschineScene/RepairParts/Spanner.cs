using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spanner : MonoBehaviour
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
        if (other.gameObject.layer == LayerMask.NameToLayer("BrokenPartFixableBySpanner"))
        {
            Debug.Log("Spanner hit brokenPart");
            // Call the method on the BrokenPart script to repair the part
            BrokenPartFixableBySpanner brokenPart = other.GetComponent<BrokenPartFixableBySpanner>();
            if (brokenPart != null)
            {
                brokenPart.SpannerInPlace();
            }
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("BrokenPartFixableBySpanner"))
        {
            Debug.Log("Spanner hit brokenPart");
            // Call the method on the BrokenPart script to stop repairing the part
            BrokenPartFixableBySpanner brokenPart = other.GetComponent<BrokenPartFixableBySpanner>();
            if (brokenPart != null)
            {
                brokenPart.SpannerOutOfPlace();
            }
        }
    }


    public void ResetPosition()
    {

        transform.localPosition = initialLocalPosition;
        transform.localRotation = initialLocalRotation;
    }

}
