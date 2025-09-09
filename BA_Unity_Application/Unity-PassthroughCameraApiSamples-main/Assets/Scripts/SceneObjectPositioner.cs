using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

/// <summary>
/// Repositions specified objects to the room's reference point (the floor anchor) after the MRUK scene has loaded.
/// </summary>
public class SceneObjectPositioner : MonoBehaviour
{
    [Tooltip("The list of objects to position inside the room after the MRUK room has loaded.")]
    public List<GameObject> objectsToPosition;

    void Start()
    {
        // Ensure an instance of MRUK exists.
        if (MRUK.Instance == null)
        {
            Debug.LogError("MRUK instance not found in the scene. This script must be used with MRUK.", this);
            return;
        }

        // Register a function to the MRUK scene loaded event.
        // This ensures that the processing is executed at the moment the room is ready.
        MRUK.Instance.RegisterSceneLoadedCallback(PositionObjectsInRoom);
    }

    /// <summary>
    /// A method that positions objects on the floor of the current room.
    /// </summary>
    private void PositionObjectsInRoom()
    {
        Debug.Log("MRUK scene loaded. Repositioning objects.");

        // Get the current room.
        MRUKRoom currentRoom = MRUK.Instance.GetCurrentRoom();
        if (currentRoom == null)
        {
            Debug.LogError("Current room could not be found. Cannot position objects.", this);
            return;
        }

        // Get the room's floor anchor.
        MRUKAnchor floorAnchor = currentRoom.GetFloorAnchor();
        if (floorAnchor == null)
        {
            Debug.LogError("Floor anchor could not be found. Cannot position objects.", this);
            return;
        }

        // The floor anchor's position is the center of the floor.
        Vector3 floorPosition = floorAnchor.transform.position;

        // Loop through all the objects in the list.
        foreach (GameObject obj in objectsToPosition)
        {
            if (obj != null)
            {
                // Default target position is the center of the floor.
                Vector3 targetPosition = floorPosition;

                // Try to adjust the height so the object's bottom rests on the floor.
                Collider objCollider = obj.GetComponent<Collider>();
                if (objCollider != null)
                {
                    // Calculate the distance from the object's pivot to its bottom edge.
                    // This assumes the object is initially at or near world origin.
                    float pivotToBottomDistance = obj.transform.position.y - objCollider.bounds.min.y;
                    
                    // Adjust the target Y position by this distance.
                    targetPosition.y += pivotToBottomDistance;
                }
                else
                {
                    Debug.LogWarning($"'{obj.name}' does not have a Collider. Positioning it at the floor's pivot. It might be submerged or floating.", obj);
                }

                // Set the object's position.
                obj.transform.position = targetPosition;

                // Optionally, parent it to the floor for organization.
                // We set the position first, then parent it while maintaining the world position.
                obj.transform.SetParent(floorAnchor.transform, true);

                Debug.Log($"Positioned '{obj.name}' on the floor anchor '{floorAnchor.name}'.");
            }
        }
    }
}