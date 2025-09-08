using UnityEngine;

public class RayGrabbableObject : MonoBehaviour
{
    // Flag to check if the object is currently grabbed
    private bool isGrabbed = false;

    // The transform of the controller that is grabbing this object
    private Transform grabberTransform;

    // Original parent of the object, to restore it upon release
    private Transform originalParent;

    [Header("Scaling Settings")]
    [Tooltip("How fast the object scales up or down")]
    public float scaleSpeed = 0.01f;

    [Tooltip("Minimum allowed scale for the object")]
    public float minScale = 0.1f;

    [Tooltip("Maximum allowed scale for the object")]
    public float maxScale = 2.0f;

    [Header("Rotation Settings")]
    [Tooltip("How fast the object rotates")]
    public float rotationSpeed = 1.0f;

    void Awake()
    {
        // Store the original parent to restore it later
        originalParent = transform.parent;
    }

    // Called by the grabbing controller to grab this object
    public void Grab(Transform grabber)
    {
        isGrabbed = true;
        grabberTransform = grabber;
        transform.SetParent(grabberTransform); // Make this object a child of the controller
    }

    // Called by the grabbing controller to release this object
    public void Release()
    {
        isGrabbed = false;
        grabberTransform = null;
        transform.SetParent(originalParent); // Restore the original parent
    }

    // Returns true if the object is currently grabbed
    public bool IsGrabbed()
    {
        return isGrabbed;
    }

    // --- Scaling Methods ---

    // Scales the object larger based on the joystick input
    public void ScaleObjectLarger(float joystickInput)
    {
        float scaleAmount = Mathf.Abs(joystickInput) * scaleSpeed;
        float newScale = transform.localScale.x + scaleAmount;
        newScale = Mathf.Clamp(newScale, minScale, maxScale);
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    // Scales the object smaller based on the joystick input
    public void ScaleObjectSmaller(float joystickInput)
    {
        // Use the absolute value of the joystick input to determine the scaling amount
        float scaleAmount = Mathf.Abs(joystickInput) * scaleSpeed;
        float newScale = transform.localScale.x - scaleAmount;
        newScale = Mathf.Clamp(newScale, minScale, maxScale);
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    // --- Rotation Methods ---

    // Rotates the object to the left based on the joystick input
    public void RotateObjectLeft(float joystickInput)
    {
        float rotationAmount = Mathf.Abs(joystickInput) * rotationSpeed;
        transform.Rotate(0, -rotationAmount, 0, Space.World);
    }

    // Rotates the object to the right based on the joystick input
    public void RotateObjectRight(float joystickInput)
    {
        float rotationAmount = Mathf.Abs(joystickInput) * rotationSpeed;
        transform.Rotate(0, rotationAmount, 0, Space.World);
    }
}