using System.Collections;
using UnityEngine;
using i5.VirtualAgents; // Required to access the Item class
using Oculus.Interaction;
using Oculus.Interaction.Grab;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.Input;
using Oculus.Interaction.HandGrab;

// Ensures this script is attached to a GameObject that has an Item component
[RequireComponent(typeof(Item))]
public class ItemDropHandler : MonoBehaviour
{
    [Tooltip("The target height (world Y-coordinate) for the item to rise to when dropped.")]
    public float targetHeight = 1.8f;

    [Tooltip("The duration of the height adjustment animation in seconds.")]
    public float adjustDuration = 0.5f;

    public Vector3 droppedRotation = new Vector3(0, 0, 0);


    private Coroutine _adjustCoroutine;

    private Grabbable grabbable;
    private GrabInteractable grabInteractable;
    private HandGrabInteractable handGrabInteractable;

    public void Awake()
    {
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
        grabbable.enabled = true;
        grabInteractable.enabled = true;
        handGrabInteractable.enabled = true;
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
    /// A coroutine that smoothly moves the item to the specified height.
    /// </summary>
    private IEnumerator SmoothMoveCoroutine()
    {
        // Wait for one frame to ensure the position after dropping is finalized
        yield return null;

        Vector3 startPosition = transform.position;
        // The target position maintains the current X and Z coordinates, only changing the Y coordinate
        Vector3 targetPosition = new Vector3(startPosition.x, targetHeight, startPosition.z);

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
