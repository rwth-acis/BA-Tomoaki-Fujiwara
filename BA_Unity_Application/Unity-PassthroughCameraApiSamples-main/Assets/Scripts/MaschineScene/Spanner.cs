using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spanner : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BrokenPart"))
        {
            Debug.Log("Spanner hit brokenPart");
            // Call the method on the BrokenPart script to repair the part
            BrokenPart brokenPart = other.GetComponent<BrokenPart>();
            if (brokenPart != null)
            {
                brokenPart.SpannerInPlace();
            }
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.CompareTag("BrokenPart"))
        {
            Debug.Log("Spanner hit brokenPart");
            // Call the method on the BrokenPart script to stop repairing the part
            BrokenPart brokenPart = other.GetComponent<BrokenPart>();
            if (brokenPart != null)
            {
                brokenPart.SpannerOutOfPlace();
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
