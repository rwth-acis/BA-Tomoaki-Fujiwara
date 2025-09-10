using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spanner : MonoBehaviour
{

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


}
