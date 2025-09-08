using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayVisualizer : MonoBehaviour {
    // Reference to the Line Renderer component.
    [SerializeField]
    private LineRenderer lineRenderer;

    // Maximum length of the ray.
    [SerializeField]
    private float maxRayLength = 100f;

    // The controller type (e.g., right or left hand) that will emit the ray.
    [SerializeField]
    private OVRInput.Controller controllerType = OVRInput.Controller.RTouch; // Default is the right controller.

    void Awake() {
        // Ensure the Line Renderer component is attached.
        if (lineRenderer == null) {
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null) {
                Debug.LogError("Line Renderer component not found. Please add a Line Renderer to the same GameObject as this script.", this);
                enabled = false; // Disable the script.
                return;
            }
        }

        // Initial setup for the Line Renderer.
        lineRenderer.positionCount = 2; // Two points for start and end.
        lineRenderer.useWorldSpace = true; // Use world coordinates.
    }

    void Update() {
        // This script assumes it is attached to the controller object itself.
        // The ray's origin will be this object's position, and the direction will be its forward vector.
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;

        // Perform the Raycast.
        RaycastHit hit;
        Vector3 rayEndPoint;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxRayLength)) {
            // If the ray hits something, set the end point to the hit point.
            rayEndPoint = hit.point;
        } else {
            // If the ray does not hit anything, set the end point to the maximum length.
            rayEndPoint = rayOrigin + rayDirection * maxRayLength;
        }

        // Update the positions of the Line Renderer.
        lineRenderer.SetPosition(0, rayOrigin);    // Start point.
        lineRenderer.SetPosition(1, rayEndPoint);  // End point.
    }
}