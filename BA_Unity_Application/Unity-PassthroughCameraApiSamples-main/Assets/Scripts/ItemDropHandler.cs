using System.Collections;
using UnityEngine;
using i5.VirtualAgents; // Required to access the Item class
using Oculus.Interaction;
using Oculus.Interaction.Grab;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.Input;
using Oculus.Interaction.HandGrab;
using Meta.XR.MRUtilityKit; // Required for using MRUK

// Ensures this script is attached to a GameObject that has an Item component
[RequireComponent(typeof(Item))]
public class ItemDropHandler : MonoBehaviour
{
    [Tooltip("The height above the reference floor for the item to rise to when dropped. In MRUK scenes, this is relative to the scanned floor. In other scenes, it's relative to Y=0.")]
    public float heightAboveFloor = 1.2f;

    [Tooltip("The duration of the height adjustment animation in seconds.")]
    public float adjustDuration = 0.5f;

    private Vector3 droppedRotation;


    private Coroutine _adjustCoroutine;

    private Grabbable grabbable;
    private GrabInteractable grabInteractable;
    private HandGrabInteractable handGrabInteractable;

    public void Awake()
    {
        droppedRotation = transform.eulerAngles;

        // Get the Grabbable component to handle item interactions
        grabbable = GetComponent<Grabbable>();
        grabInteractable = GetComponent<GrabInteractable>();
        handGrabInteractable = GetComponent<HandGrabInteractable>();
    }

    public void ItemDropped()
    {

        transform.rotation = Quaternion.Euler(droppedRotation);

        // Start the height adjustment process when the item is dropped
        SetComponentOn();
        StartHeightAdjustment();
    }

    public void SetComponentOn() {
        if (grabInteractable != null) {
            grabbable.enabled = true;
        }
        if (grabInteractable != null) {
            grabInteractable.enabled = true;
        }
        if (handGrabInteractable != null) {
            handGrabInteractable.enabled = true;
        }
            
        Debug.Log("TurnComponentOn");
    }


    /// <summary>
    /// Starts the item height adjustment process.
    /// </summary>
    public void StartHeightAdjustment()
    {
        // If an adjustment is already in progress, stop it
        if (_adjustCoroutine != null)
        {
            StopCoroutine(_adjustCoroutine);
        }
        // Start a new height adjustment coroutine
        _adjustCoroutine = StartCoroutine(SmoothMoveCoroutine());
    }

    /// <summary>
    /// A coroutine that smoothly moves the item to a calculated height above the floor.
    /// </summary>
    private IEnumerator SmoothMoveCoroutine()
    {
        // Wait for one frame to ensure the position after dropping is finalized
        yield return null;

        Vector3 startPosition = transform.position;

        // --- Dynamic Height Calculation ---
        float referenceFloorY = 0.0f; // Default to world origin floor (Y=0)

        // If MRUK is available, try to get the actual floor height
        if (MRUK.Instance != null)
        {
            MRUKRoom currentRoom = MRUK.Instance.GetCurrentRoom();
            if (currentRoom != null)
            {
                MRUKAnchor floorAnchor = currentRoom.GetFloorAnchor();
                if (floorAnchor != null)
                {
                    referenceFloorY = floorAnchor.transform.position.y;
                }
                else
                {
                    Debug.LogWarning("ItemDropHandler: MRUK Floor Anchor not found. Defaulting to Y=0.", this);
                }
            }
        }

        // Calculate the final target height based on the reference floor and the desired offset.
        float finalTargetHeight = referenceFloorY + heightAboveFloor;
        Vector3 targetPosition = new Vector3(startPosition.x, finalTargetHeight, startPosition.z);

        float elapsedTime = 0f;

        while (elapsedTime < adjustDuration)
        {
            // Calculate the progress of the movement (0 to 1) from the elapsed time
            float t = elapsedTime / adjustDuration;
            // Calculation for a more natural movement (ease-in/out)
            t = t * t * (3f - 2f * t);

            // Use Lerp (linear interpolation) to calculate and apply the position for the current frame
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Finally, set the position exactly to the target position
        transform.position = targetPosition;
        _adjustCoroutine = null;
    }
}
