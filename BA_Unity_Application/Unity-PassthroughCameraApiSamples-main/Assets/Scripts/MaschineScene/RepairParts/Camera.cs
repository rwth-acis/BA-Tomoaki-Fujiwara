using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BrokenPartFixableByCamera"))
        {
            Debug.Log("Camera hit brokenPart");
            // Call the method on the BrokenPart script to repair the part
            BrokenPartFixableByCamera brokenPart = other.GetComponent<BrokenPartFixableByCamera>();
            if (brokenPart != null)
            {
                brokenPart.CameraInPlace();
            }
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.CompareTag("BrokenPartFixableByCamera"))
        {
            Debug.Log("Camera hit brokenPart");
            // Call the method on the BrokenPart script to stop repairing the part
            BrokenPartFixableByCamera brokenPart = other.GetComponent<BrokenPartFixableByCamera>();
            if (brokenPart != null)
            {
                brokenPart.CameraOutOfPlace();
            }
        }
    }


}
