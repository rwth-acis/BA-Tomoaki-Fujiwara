using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.Grab;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction;


public class KitchenStoveRight : MonoBehaviour
{


    public string itemNameOnStove = "";

    public Transform itemOnStoveCoordinate;

    void OnTriggerEnter(Collider other) {
        Debug.Log("Something collide with stove");
        if (itemNameOnStove == "")
        {
            Debug.Log("Something collide with stove2");
            // Check if the object is a frying pan or something else that can be placed on the stove
            if (other.gameObject.GetComponent<FryingPan>() != null)
            {
                Debug.Log("Something collide with stove3");

                itemNameOnStove = other.gameObject.name;

                // Set the frying pan or something else to the stove coordinate

                // == Disable the grab component first to fix the place ==
                other.gameObject.GetComponent<Grabbable>().enabled = false;

                Transform otherTransform = other.gameObject.transform;
                otherTransform.SetParent(itemOnStoveCoordinate);
                otherTransform.localPosition = Vector3.zero;
                otherTransform.localRotation = Quaternion.Euler(90, 0, 0);


                // Enable the grab component again 
                other.gameObject.GetComponent<Grabbable>().enabled = true;


                // Set the state of the frying pan or something else to be on the stove
                FryingPanCollider fryingPan = other.gameObject.GetComponent<FryingPanCollider>();
                if (fryingPan != null)
                {
                    fryingPan.state = FryingPanCollider.FryingPanState.OnTheStove;
                    //fryingPan.SetTheState("OnTheStove");
                }

                // Enable the collider of the frying pan or something else, so the user can place ingredients on it

            }
        }

    }


    void OnTriggerExit(Collider other) { 
        if (itemNameOnStove != "")
        {
            if (itemNameOnStove == other.gameObject.name)
            {
                itemNameOnStove = "";

                // Disable the collider of the frying pan or something else, so the user can place ingredients on it


                // Reset the parent of the object to null
                other.gameObject.transform.SetParent(null);

                // Set the state of the frying pan or something else to be not on the stove
                FryingPanCollider fryingPan = other.gameObject.GetComponent<FryingPanCollider>();
                if (fryingPan != null)
                {
                    fryingPan.state = FryingPanCollider.FryingPanState.NotOnTheStove;
                    //fryingPan.SetTheState("NotOnTheStove");
                }

            }
        }

    }

}
