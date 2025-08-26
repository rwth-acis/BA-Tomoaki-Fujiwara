using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBoundary : MonoBehaviour
{
    public float padding = 0.01f; // Padding from the box wall (considering the ball's radius, etc.)

    [SerializeField] private Vector3 minLocalBounds = new Vector3(-0.3f, -0.3f, 0.0f);
    [SerializeField] private Vector3 maxLocalBounds = new Vector3(0.3f, 0.3f, 0.3f);

    public Vector3 resetPosition;


    void FixedUpdate() // Use FixedUpdate to match the physics update
    {
        // Get the current local position of this object
        Vector3 localPos = transform.localPosition;

        // Check if the object is out of the defined local bounds
        bool outOfBounds =
            localPos.x < minLocalBounds.x || localPos.x > maxLocalBounds.x ||
            localPos.y < minLocalBounds.y || localPos.y > maxLocalBounds.y ||
            localPos.z < minLocalBounds.z || localPos.z > maxLocalBounds.z;

        if (outOfBounds)
        {
            Debug.Log("Object is out of bounds. Resetting position.");
            Debug.Log("Current Local Coordinate: " + localPos);

            // Reset the object's local position to the specified resetPosition.
            transform.localPosition = resetPosition;
            
            // Optionally, reset velocity if a Rigidbody is attached
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
