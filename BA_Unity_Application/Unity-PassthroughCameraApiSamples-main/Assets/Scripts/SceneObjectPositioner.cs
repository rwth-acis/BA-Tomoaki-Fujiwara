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
    /// A method that positions objects as children of the current room's floor anchor.
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

        // Loop through all the objects in the list.
        foreach (GameObject obj in objectsToPosition)
        {
            if (obj != null)
            {
                // Make the object a child of the floor anchor.
                // This moves the object to a position relative to the floor anchor.
                obj.transform.SetParent(floorAnchor.transform, true);
                Debug.Log($"Set '{obj.name}' as a child of floor anchor '{floorAnchor.name}'.");
            }
        }
    }
}