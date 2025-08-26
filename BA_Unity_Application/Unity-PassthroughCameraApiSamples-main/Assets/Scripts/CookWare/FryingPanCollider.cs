using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.Grab;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction;

public class FryingPanCollider : MonoBehaviour
{

    private bool potatoInPan = false;
    private bool eggplantInPan = false;
    private bool onionInPan = false;
    private bool carrotInPan = false;

    public enum FryingPanState { OnTheStove, NotOnTheStove }

    public FryingPanState state = FryingPanState.NotOnTheStove;


    public Transform PotatoFryingPanCoordinate;
    public Transform EggplantFryingPanCoordinate;
    public Transform OnionFryingPanCoordinate;
    public Transform CarrotFryingPanCoordinate;

    public GameObject PotatoParent;
    public GameObject EggplantParent;
    public GameObject OnionParent;
    public GameObject CarrotParent;

    void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.name == "ResetCollider")
        {

            if (PotatoParent != null)
            {
                PotatoParent.transform.SetParent(null);

                Ingredient potatoIngredient = PotatoParent.GetComponent<Ingredient>();
                if (potatoIngredient != null)
                {
                    potatoIngredient.reset();
                }

                PotatoParent = null;
            }

      
            if (EggplantParent != null)
            {

                EggplantParent.transform.SetParent(null);

                Ingredient eggplantIngredient = EggplantParent.GetComponent<Ingredient>();
                if (eggplantIngredient != null)
                {
                    eggplantIngredient.reset();
                }

                EggplantParent = null;
            }

            potatoInPan = false;
            eggplantInPan = false;
        }

        if (state == FryingPanState.NotOnTheStove)
        {
            Debug.Log("Frying pan is not on the stove, cannot place ingredients.");
            return;
        }

        if (other.gameObject.name == "PotatoCut03")
        {
            if (!potatoInPan) {


                Debug.Log("Potato in pan");

                potatoInPan = true;


                // Set PotatoParent to the parent GameObject of PotatoCut03
                PotatoParent = other.gameObject.transform.parent.gameObject;


                // Unable the collider of the potato
                // Unable the Grabbable component of the potato
                other.gameObject.GetComponent<Collider>().enabled = false;
                other.gameObject.GetComponent<HandGrabInteractable>().enabled = false;
                other.gameObject.GetComponent<GrabInteractable>().enabled = false;


                // Set the parent of the potato to the PotatoCoordinate
                Transform parentTransform = other.gameObject.transform.parent;
                // Enable to cut the potato

                if (parentTransform != null){
                    parentTransform.SetParent(PotatoFryingPanCoordinate);
                    parentTransform.localPosition = Vector3.zero;
                    parentTransform.localRotation = Quaternion.Euler(0, 90, 0);
                }


                // Start the cooking process of the potato
                Potato potato = other.gameObject.transform.parent.gameObject.GetComponent<Potato>();
                if (potato != null)
                {
                    potato.cooking();
                    Debug.Log("Potato is cooking in the frying pan.");
                }
                else
                {
                    Debug.LogError("Potato component not found on the object.");
                }

            }
        }

        if (other.gameObject.name == "EggplantCut06")
        {
            if (!eggplantInPan)
            {


                Debug.Log("Potato in pan");

                eggplantInPan = true;
                // Unable the collider of the potato
                // Unable the Grabbable component of the potato
                other.gameObject.GetComponent<Collider>().enabled = false;
                other.gameObject.GetComponent<HandGrabInteractable>().enabled = false;
                other.gameObject.GetComponent<GrabInteractable>().enabled = false;


                // Set the parent of the potato to the PotatoCoordinate
                Transform parentTransform = other.gameObject.transform.parent;

                // Set EggplantParent to the parent GameObject of PotatoCut03
                EggplantParent = other.gameObject.transform.parent.gameObject;

                // Enable to cut the potato

                if (parentTransform != null)
                {
                    parentTransform.SetParent(EggplantFryingPanCoordinate);
                    parentTransform.localPosition = Vector3.zero;
                    parentTransform.localRotation = Quaternion.Euler(0, 0, 0);
                }


                // Start the cooking process of the Eggplant
                Eggplant eggplant = other.gameObject.transform.parent.gameObject.GetComponent<Eggplant>();
                if (eggplant != null)
                {
                    eggplant.cooking();
                    Debug.Log("Potato is cooking in the frying pan.");
                }
                else
                {
                    Debug.LogError("Potato component not found on the object.");
                }

            }
        }

        if (other.gameObject.name == "OnionCut04")
        {
            if (!onionInPan)
            {
                Debug.Log("Onion in pan");

                onionInPan = true;
                // Unable the collider of the potato
                // Unable the Grabbable component of the potato
                other.gameObject.GetComponent<Collider>().enabled = false;
                other.gameObject.GetComponent<HandGrabInteractable>().enabled = false;
                other.gameObject.GetComponent<GrabInteractable>().enabled = false;


                // Set the parent of the potato to the PotatoCoordinate
                Transform parentTransform = other.gameObject.transform.parent;

                // Set EggplantParent to the parent GameObject of PotatoCut03
                OnionParent = other.gameObject.transform.parent.gameObject;

                // Enable to cut the potato

                if (parentTransform != null)
                {
                    parentTransform.SetParent(OnionFryingPanCoordinate);
                    parentTransform.localPosition = Vector3.zero;
                    parentTransform.localRotation = Quaternion.Euler(0, 0, 0);
                }


                // Start the cooking process of the Eggplant
                Onion onion = other.gameObject.transform.parent.gameObject.GetComponent<Onion>();
                if (onion != null)
                {
                    onion.cooking();
                    Debug.Log("Potato is cooking in the frying pan.");
                }
                else
                {
                    Debug.LogError("Potato component not found on the object.");
                }

            }
        }

        if (other.gameObject.name == "CarrotCut05")
        {
            if (!carrotInPan)
            {
                Debug.Log("Carrot in pan");

                carrotInPan = true;
                // Disable the collider and grabbable components of the carrot
                other.gameObject.GetComponent<Collider>().enabled = false;
                other.gameObject.GetComponent<HandGrabInteractable>().enabled = false;
                other.gameObject.GetComponent<GrabInteractable>().enabled = false;

                // Set CarrotParent to the parent GameObject of CarrotCut05
                CarrotParent = other.gameObject.transform.parent.gameObject;

                // Move the carrot to the frying pan coordinate
                Transform parentTransform = other.gameObject.transform.parent;
                if (parentTransform != null)
                {
                    parentTransform.SetParent(CarrotFryingPanCoordinate);
                    parentTransform.localPosition = Vector3.zero;
                    parentTransform.localRotation = Quaternion.Euler(0, 0, 0);
                }

                // Start the cooking process of the Carrot
                Carrot carrot = other.gameObject.transform.parent.gameObject.GetComponent<Carrot>();
                if (carrot != null)
                {
                    carrot.cooking();
                    Debug.Log("Carrot is cooking in the frying pan.");
                }
                else
                {
                    Debug.LogError("Carrot component not found on the object.");
                }
            }
        }

    }

    public void SetTheState(string stateTxt) {

        if (stateTxt == "onTheStove") {
            state = FryingPanState.OnTheStove;

        } else if (stateTxt == "NotOnTheStove") {
            state = FryingPanState.NotOnTheStove;

        } else {
            Debug.LogError("Invalid state for frying pan: " + stateTxt);
            return;
        }

        
        Debug.Log($"Frying pan state set to: {state}");
    }
}
