using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
public class OvenBakeCollider : MonoBehaviour
{

    public bool isBakePlateInOven = false;
    // Reference to the bake plate object
    public Transform bakePlateObject;
    public Grabbable bakePlateGrabbable;

    public Transform bakePlatePlaceCoordinate;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Oven Bake Collider Trigger Entered with: " + other.gameObject.name);
        if (other.gameObject.name == "BakePlate")
        {
            isBakePlateInOven = true;
            bakePlateGrabbable.enabled = false;
            bakePlateObject.position = bakePlatePlaceCoordinate.position;
            bakePlateObject.rotation = bakePlatePlaceCoordinate.rotation;
            bakePlateGrabbable.enabled = true; // Disable grabbable to prevent interaction while in oven
        }
    }

    public bool IsBakePlateInOven()
    {
        return isBakePlateInOven;
    }
}
