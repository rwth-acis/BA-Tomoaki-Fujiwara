using UnityEngine;

public class RayGrabbing : MonoBehaviour {
    // Maximum length of the ray (used for RayVisualizer and object detection)
    [SerializeField]
    private float maxRayLength = 100f;

    // Controller type used to cast the ray (default: right controller)
    [SerializeField]
    private OVRInput.Controller controllerType = OVRInput.Controller.RTouch; // Default is right controller

    // Button used to grab objects (default: right hand trigger)
    [SerializeField]
    private OVRInput.RawButton grabButton = OVRInput.RawButton.RHandTrigger;

    // Currently grabbed object
    private RayGrabbableObject grabbedObject;

    // Reference to the LineRenderer for visualizing the ray
    // Attach the same script to the GameObject as RayVisualizer, and assign the LineRenderer here to visualize the ray's endpoint.
    [SerializeField]
    private LineRenderer rayLineRenderer;

    [Header("Scaling Input")]
    [Tooltip("Joystick axis used to scale the grabbed object (default: right thumbstick)")]
    public OVRInput.RawAxis2D scaleAxis = OVRInput.RawAxis2D.RThumbstick; // Default: right stick

    [Tooltip("Sensitivity for scaling the object with the joystick")]
    public float joystickScaleSensitivity = 0.01f; // Adjust as needed

    void Update() {
        // Get the origin and direction of the ray (same logic as RayVisualizer)
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;

        // Perform raycast
        RaycastHit hit;
        bool hasHit = Physics.Raycast(rayOrigin, rayDirection, out hit, maxRayLength);

        // Visualize the ray using LineRenderer
        if (rayLineRenderer != null) {
            rayLineRenderer.SetPosition(0, rayOrigin);
            rayLineRenderer.SetPosition(1, hasHit ? hit.point : rayOrigin + rayDirection * maxRayLength);
        }

        // --- Grab object ---
        if (OVRInput.GetDown(grabButton, controllerType)) // When grab button is pressed
        {
            if (grabbedObject == null) // If not already grabbing an object
            {
                if (hasHit) // If the ray hit something
                {
                    // Check if the hit object is grabbable
                    RayGrabbableObject hitGrabbable = hit.collider.GetComponent<RayGrabbableObject>();
                    if (hitGrabbable != null && !hitGrabbable.IsGrabbed()) // If it's not already grabbed by another controller
                    {
                        grabbedObject = hitGrabbable;
                        grabbedObject.Grab(this.transform); // Pass this controller's transform to the object
                    }
                }
            }
        }
        // --- Release object ---
        else if (OVRInput.GetUp(grabButton, controllerType)) // When grab button is released
        {
            if (grabbedObject != null) // If currently grabbing an object
            {
                grabbedObject.Release();
                grabbedObject = null; // Reset grabbed object
            }
        }

        // If currently grabbing an object, handle scaling
        if (grabbedObject != null)
        {
            // Get 2D joystick input
            Vector2 joystickInput = OVRInput.Get(scaleAxis, controllerType);

            Debug.Log($"Joystick Input: {joystickInput}");

            // Get horizontal and vertical input
            float horizontalInput = joystickInput.x;
            float verticalInput = joystickInput.y;

            Debug.Log($"Horizontal Input: {horizontalInput}, Vertical Input: {verticalInput}");

            // Only apply scaling if vertical input is dominant
            if (Mathf.Abs(verticalInput) > Mathf.Abs(horizontalInput))
            {

                if (verticalInput >= 0.1f)
                {
                    grabbedObject.ScaleObjectLarger();
                }
                else if (verticalInput <= -0.1f)
                {
                    grabbedObject.ScaleObjectSmaller();

                }
            }
            else if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
            {
                if (horizontalInput >= 0.1f)
                {
                    grabbedObject.RotateObjectRight();
                }
                else if (horizontalInput <= -0.1f)
                {
                    grabbedObject.RotateObjectLeft();

                }
            }

        }

    }
}