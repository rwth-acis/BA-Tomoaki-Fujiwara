using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.Grab;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction;



public class CutboardCollider : MonoBehaviour
{
    private string currentOnBoard="";

    public Transform PotatoCutBoardCoordinate;
    public Transform EggplantCutBoardCoordinate;

    void OnTriggerEnter(Collider other)
    {

        if (currentOnBoard == "")
        {



            if (other.gameObject.name == "PotatoUncut")
            {   
                //currentOnBoard = "Potato";
                currentOnBoard = other.gameObject.transform.parent.name;

                // Unable the collider of the potato
                // Unable the Grabbable component of the potato
                other.gameObject.GetComponent<Collider>().enabled = false;
                other.gameObject.GetComponent<HandGrabInteractable>().enabled = false;
                other.gameObject.GetComponent<GrabInteractable>().enabled = false;


                // Set the parent of the potato to the PotatoCutBoardCoordinate
                Transform parentTransform = other.gameObject.transform.parent;
                // Enable to cut the potato
                parentTransform.GetComponent<Collider>().enabled = true;

                if (parentTransform != null)
                {
  
                    parentTransform.SetParent(PotatoCutBoardCoordinate);
                    parentTransform.localPosition = Vector3.zero;
                    parentTransform.localRotation = Quaternion.Euler(0, 90, 0);

                    Collider parentCollider = parentTransform.GetComponent<Collider>();
                    if (parentCollider != null)
                    {
                        parentCollider.enabled = true;
                    }
                    else
                    {
                        Debug.LogWarning("Parent has no collider");
                    }
                }
            }


            if (other.gameObject.name == "EggplantUncut")
            {
                //currentOnBoard = "Eggplant";
                currentOnBoard = other.gameObject.transform.parent.name;

                // Unable the collider of the potato
                // Unable the Grabbable component of the potato
                other.gameObject.GetComponent<Collider>().enabled = false;
                other.gameObject.GetComponent<HandGrabInteractable>().enabled = false;
                other.gameObject.GetComponent<GrabInteractable>().enabled = false;


                // Set the parent of the eggplant to the PotatoCutBoardCoordinate
                Transform parentTransform = other.gameObject.transform.parent;
                // Enable to cut the eggplant
                parentTransform.GetComponent<Collider>().enabled = true;

                if (parentTransform != null)
                {

                    parentTransform.SetParent(EggplantCutBoardCoordinate);
                    parentTransform.localPosition = Vector3.zero;
                    parentTransform.localRotation = Quaternion.Euler(0, 0, 0);

                    Collider parentCollider = parentTransform.GetComponent<Collider>();
                    if (parentCollider != null)
                    {
                        parentCollider.enabled = true;
                    }
                    else
                    {
                        Debug.LogWarning("Parent has no collider");
                    }
                }
            }

        }




    }


    void OnTriggerExit(Collider other)
    {

        if (currentOnBoard != "")
        {



            if ((other.gameObject.name == "PotatoCut03") && (currentOnBoard == other.gameObject.transform.parent.name))
            {
                currentOnBoard = "";

                // Set the parent of the potato to free
                Transform parentTransform = other.gameObject.transform.parent;


                if (parentTransform != null)
                {

                    parentTransform.SetParent(null);

                    // Unable the collider of the potato cut
                    parentTransform.GetComponent<Collider>().enabled = false;


                }
            }

            if ((other.gameObject.name == "EggplantCut06") && (currentOnBoard == other.gameObject.transform.parent.name))
            {
                currentOnBoard = "";

                // Set the parent of the potato to free
                Transform parentTransform = other.gameObject.transform.parent;
                if (parentTransform != null)
                {

                    parentTransform.SetParent(null);


                }
            }

        }




    }
    


}
