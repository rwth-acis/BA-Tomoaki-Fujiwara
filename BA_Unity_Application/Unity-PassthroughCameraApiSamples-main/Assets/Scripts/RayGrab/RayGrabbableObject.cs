    using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RayGrabbableObject : MonoBehaviour {
    private Rigidbody rb;
    private Transform originalParent;
    private bool isGrabbed = false;

    public Transform CurrentGrabber { get; private set; }

    // ====== Placement Settings ======
    [Header("Placement Settings")]
    [Tooltip("Layer mask to check for collisions when placing the object")]
    public LayerMask placementCollisionLayerMask;

    [Tooltip("Offset applied when placing the object to avoid clipping (default: 1cm)")]
    public float placementOffset = 0.01f; // Default: 1cm offset

    // Materials for placement feedback (can be used to show if placement is valid or invalid)
    //[Tooltip("Material to show when placement is valid (null uses default material)")]
    public Material canPlaceMaterial;
    //[Tooltip("Material to show when placement is invalid")]
    public Material cannotPlaceMaterial;
    private Renderer[] objectRenderers; // Renderers for this object and its children

    // Show placement guide in real-time while grabbing
    //[Tooltip("Show placement guide in real-time while grabbing")]
    public bool showPlacementGuide = true;
    // =============================

    // ====== Scaling Settings ======
    [Header("Scaling Settings")]
    [Tooltip("Initial scale of the object")]
    private Vector3 currentScale;

    private Vector3 initialScale;

    [Tooltip("Speed at which the object scales (e.g., 0.1 means 10% per input)")]
    public float scaleSpeed = 0.01f; // Default: 1% per input

    private float rotationZ; 

    [Tooltip("Minimum scale multiplier (relative to initial scale)")]
    public float minScaleMultiplier = 0.5f; // Default: half the initial size

    [Tooltip("Maximum scale multiplier (relative to initial scale)")]
    public float maxScaleMultiplier = 2.0f; // Default: up to twice the initial size
    // =============================

    void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        // Cache all Renderer components in this object and its children
        objectRenderers = GetComponentsInChildren<Renderer>();

        currentScale = transform.localScale;
        initialScale = currentScale;


        rotationZ = transform.rotation.eulerAngles.z;

    }

    /// <summary>
    /// Called when the object is grabbed
    /// </summary>
    /// <param name="grabber">Transform of the controller grabbing this object</param>
    public void Grab(Transform grabber) {
        if (isGrabbed) return;

        isGrabbed = true;
        CurrentGrabber = grabber;

        originalParent = transform.parent;
        transform.SetParent(grabber, true); // Parent to the controller
        rb.isKinematic = true;

        // Optionally reset placement feedback material
        //SetPlacementMaterial(true);

        Debug.Log($"{gameObject.name} was grabbed by {grabber.name}!");
    }

    /// <summary>
    /// Called when the object is released
    /// </summary>
    public void Release() {
        if (!isGrabbed) return;

        // If placement is valid, release the object
        //if (CanPlaceObject()) {
        if (true) {
            isGrabbed = false;
            CurrentGrabber = null;

            //transform.SetParent(originalParent, true); // Restore original parent
            transform.SetParent(null);
            rb.isKinematic = true; // Keep kinematic after release

            Debug.Log($"{gameObject.name} was released.");
            //SetPlacementMaterial(true); // Reset to valid placement material

            // Optionally snap rotation after release
            Quaternion targetRotation;
            targetRotation = Quaternion.Euler(-90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z   );
            transform.rotation = targetRotation;

        } else {
            // If placement is invalid, restore to original parent and state
            // This section can be expanded to handle failed placement logic
            Debug.LogWarning($"{gameObject.name} cannot be placed at the current location!");
            // SetPlacementMaterial(false); // Show invalid placement material
            isGrabbed = false;
            transform.SetParent(originalParent, true);
        }
    }

    public void RotateObjectRight()
    {
        // Scale down by a fixed amount
        rotationZ += 6f; // Rotate right by 15 degrees

        if (rotationZ >= 360f) {
            rotationZ -= 360f; // Wrap around to keep within 0-360 degrees
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rotationZ);

    }

    public void RotateObjectLeft()
    {
        // Scale down by a fixed amount
        rotationZ -= 6f; // Rotate right by 15 degrees

        if (rotationZ <= 0f)
        {
            rotationZ += 360f; // Wrap around to keep within 0-360 degrees
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rotationZ);

    }

    public void ScaleObjectSmaller() {
        // Scale down by a fixed amount
        currentScale = currentScale - (initialScale * scaleSpeed);
        if (currentScale.x < initialScale.x * minScaleMultiplier) {
            currentScale = new Vector3(initialScale.x * minScaleMultiplier, initialScale.x * minScaleMultiplier, initialScale.x * minScaleMultiplier); // Prevent scaling below 10% of original size
        }

        transform.localScale = currentScale;

        Debug.Log($"Scaled down to: {currentScale.x}"); // Debug log for scaling
    }

    public void ScaleObjectLarger()
    {
        // Scale up by a fixed amount
        currentScale = currentScale + (initialScale * scaleSpeed);
        if (currentScale.x > initialScale.x* maxScaleMultiplier)
        {
            currentScale = new Vector3(initialScale.x * maxScaleMultiplier, initialScale.x * maxScaleMultiplier, initialScale.x * maxScaleMultiplier); // Prevent scaling below 10% of original size
        }

        transform.localScale = currentScale;

        Debug.Log($"Scaled up to: {currentScale.x}"); // Debug log for scaling
    }

    public void ScaleObject(float scaleChangeAmount) {
        // Calculate current scale multiplier
        float currentScaleMultiplier = transform.localScale.x / currentScale.x;

        // Calculate new scale multiplier and clamp between min/max
        float newScaleMultiplier = currentScaleMultiplier + scaleChangeAmount;
        newScaleMultiplier = Mathf.Clamp(newScaleMultiplier, minScaleMultiplier, maxScaleMultiplier);

        // Apply new scale
        transform.localScale = currentScale * newScaleMultiplier;
    }

    // ====== Placement Guide Logic ======
    /*
    void LateUpdate() {
        // While grabbing, show placement guide in real-time
        if (isGrabbed && showPlacementGuide) {
            SetPlacementMaterial(CanPlaceObject());
        }
    }
    */
    /// <summary>
    /// Checks if the object can be placed at its current position and size
    /// </summary>
    /// <returns>True if placement is valid, false otherwise</returns>
    private bool CanPlaceObject() {
        // Get the object's collider
        Collider ownCollider = GetComponent<Collider>();
        if (ownCollider == null) {
            Debug.LogWarning("GrabbableObject has no collider. Placement check skipped.", this);
            return true; // If no collider, allow placement (error handling)
        }

        // Get the bounds of the collider
        Bounds bounds = ownCollider.bounds;

        // Use Physics.OverlapBox to check for collisions with the specified layer mask
        ownCollider.enabled = false; // Temporarily disable own collider for accurate check

        Vector3 checkPosition = transform.position;
        // Optionally adjust check position with offset
        // checkPosition.y += placementOffset; 

        Collider[] hitColliders = Physics.OverlapBox(bounds.center, bounds.extents - Vector3.one * placementOffset, transform.rotation, placementCollisionLayerMask);

        ownCollider.enabled = true; // Re-enable collider

        // Check for collisions with other objects
        foreach (Collider hitCollider in hitColliders) {
            if (hitCollider != ownCollider) // Ignore self
            {
                // Optionally filter by tag (e.g., allow walls/floors)
                // if (hitCollider.CompareTag("Wall") || hitCollider.CompareTag("Floor")) continue;

                Debug.Log($"Placement collision: {gameObject.name} collides with {hitCollider.name}.", hitCollider.gameObject);
                return false; // Invalid placement
            }
        }
        return true; // Valid placement
    }

    /// <summary>
    /// Sets the object's material to indicate placement validity
    /// </summary>
    /// <param name="canPlace">True if placement is valid, false otherwise</param>
    private void SetPlacementMaterial(bool canPlace) {
        if (canPlaceMaterial == null || cannotPlaceMaterial == null) return;

        Material targetMaterial = canPlace ? canPlaceMaterial : cannotPlaceMaterial;
        foreach (Renderer rend in objectRenderers) {
            // Only update if the material is different
            if (rend.sharedMaterial != targetMaterial) {
                rend.sharedMaterial = targetMaterial; // Use sharedMaterial for asset-wide changes
                // rend.material = targetMaterial; // Use material for instance changes
                // Optionally, change color instead of material
                // rend.material.color = canPlace ? Color.green : Color.red; 
            }
        }
    }

    public bool IsGrabbed()
    {
        return isGrabbed;
    }

    // =============================
}