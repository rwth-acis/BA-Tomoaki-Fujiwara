using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using i5.Toolkit.Core.SceneDocumentation;
using TMPro;
using Oculus.Interaction.Grab;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction;

public class Eggplant : Ingredient
{
    // The maximum number of cuts allowed for the eggplant
    public int cutMax = 3;
    public int currentCuts = 0;
    // Array of GameObjects representing the cutted eggplants
    public GameObject[] cutedEggplants;


    // Enum to represent the different states of the ingredient
    public enum IngredientState { raw, cutting, cutted, cooking, cooked, boiling, boiled , seasoned , served}

    // Current state of the ingredient represented by Interactable Object Label from Meta XR SDK
    public GameObject LabelObject;
    public TextMeshProUGUI interactableObjectLabel;


    // Current state of the eggplant
    public IngredientState state = IngredientState.raw;

    // Documentation object to hold the title and description for the eggplant
    public DocumentationObject documentation;


    // Start is called before the first frame update
    public override void cut()
    {
        Debug.Log("Cutting eggplant");
        if (currentCuts < cutMax - 1)
        {
            // Set the last cutted eggplant to inactive and activate the next one
            cutedEggplants[currentCuts].SetActive(false);
            currentCuts++;
            cutedEggplants[currentCuts].SetActive(true);


            // Update the state to cutting
            state = IngredientState.cutting;

            // Update the documentation title and description
            documentation.title = "Eggplant Cutting";
            documentation.description = "Cutting the eggplant into smaller pieces. The eggplant need to be cut few times more.";


            // Update the interactable object label
            interactableObjectLabel.text = "Not fully cutted Eggplant";


        }
        else if (currentCuts < cutMax) {
            // Last cut, no more cuts allowed
            // Set the last cutted eggplant to inactive and activate the next one
            cutedEggplants[currentCuts].SetActive(false);
            currentCuts++;
            cutedEggplants[currentCuts].SetActive(true);

            // Update the state to cutted
            state = IngredientState.cutted;

            // enable the collider of the eggplant
            // enable the Grabbable component of the eggplant
            cutedEggplants[currentCuts].GetComponent<Collider>().enabled = true;
            cutedEggplants[currentCuts].GetComponent<HandGrabInteractable>().enabled = true;
            cutedEggplants[currentCuts].GetComponent<GrabInteractable>().enabled = true;

            // Update the documentation title and description
            documentation.title = "Eggplant Cutted";
            documentation.description = "Fully cutted eggplant. It is ready to be cooked or boiled.";

            // Update the interactable object label
            interactableObjectLabel.text = "Fully cutted Eggplant";

        }
    }

    public override void cooking()
    {
        // Set the state to cooking
        state = IngredientState.cooking;


        // Start the cooking animation for the current cutted eggplant
        Animator animator = cutedEggplants[currentCuts].GetComponent<Animator>();
        animator.SetBool("saute", true);


        // Update the documentation title and description
        documentation.title = "Eggplant Cooking";
        documentation.description = "Eggplant is cooking in the frying pan. Need few more second to finish the cooking.";


        // Update the interactable object label
        interactableObjectLabel.text = "Eggplant Cooking";
        // Disable the label object, otherwise other labels from other ingredients will overlap
        LabelObject.SetActive(false);
    }

    public override void cooked()
    {
        // Set the state to cooked
        state = IngredientState.cooked;

   
        documentation.title = "Eggplant Cooked";
        documentation.description = "Fully cooked eggplant. Need to seasoning by salt or pepper.";
    }

    public override void seasoned()
    {
        // Set the state to seasoned
        state = IngredientState.seasoned;


        documentation.title = "Eggplant Cooked and seasoned";
        documentation.description = "Fully cooked eggplant and seasoned. Now ready to serve on a dish.";
    }

    public override void served()
    {
        state = IngredientState.served;

        documentation.title = "served Eggplant";
        documentation.description = "The cooed eggplant is served on dish.";
    }

    public override void reset() {
        state = IngredientState.raw;

        Animator animator = cutedEggplants[currentCuts].GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetBool("saute", false);
            animator.SetBool("boil", false);
        }

        cutedEggplants[currentCuts].SetActive(false);
        currentCuts = 0;
        cutedEggplants[currentCuts].SetActive(true);

        // enable the collider of the eggplant
        // enable the Grabbable component of the eggplant
        cutedEggplants[currentCuts].GetComponent<Collider>().enabled = true;
        cutedEggplants[currentCuts].GetComponent<HandGrabInteractable>().enabled = true;
        cutedEggplants[currentCuts].GetComponent<GrabInteractable>().enabled = true;

        documentation.title = "Raw Eggplant";
        documentation.description = "This is a raw Eggplant. This can be placed on the cutboard to be cut. ";

        interactableObjectLabel.text = "Eggplant";
        LabelObject.SetActive(true);

    }

}
