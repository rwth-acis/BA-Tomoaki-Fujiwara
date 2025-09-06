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
            documentation.title = "Cutting Raw Eggplant";
            documentation.description = "This is a partially cut raw eggplant. It cannot be used to bake a cake. The user cannot grab this eggplant. The user must continue to use the kitchen knife to hit it multiple times until it is fully cut.\r\n\r\nOnce fully cut, the user can grab it and place it on the frying pan to be sautéed. If it is not fully cut, the user cannot grab the eggplant from the cutting board, nor can they place other ingredients on the board.\r\n\r\nThe eggplant cannot be reset to its initial, uncut state while it is on the cutting board and not fully cut. If the eggplant is on the frying pan, the user can't grab it directly, but can grab the frying pan and hit it against the hitbox to reset the eggplant.";


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
            documentation.title = "Fully Cut Raw Eggplant";
            documentation.description = "This is a fully cut raw eggplant. It cannot be used to bake a cake. The user can now grab it and place it on the frying pan to be sautéed.\r\n\r\nTo place it on the frying pan, the frying pan must first be on the stove. Once the eggplant is placed on the pan, it will begin to cook. It will not overcook.\r\n\r\nThe eggplant can be reset to its initial, uncut state by grabbing it and hitting it against the blue hitbox over the trash can. Since the eggplant is fully cut, the user can grab it from the cutting board. If the eggplant is on the frying pan, the user can't grab it directly, but can grab the frying pan and hit it against the hitbox to reset the eggplant.";

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
        documentation.title = "Cooking Eggplant";
        documentation.description = "This is a cooking eggplant. It cannot be used to bake a cake. The eggplant is currently on the frying pan.\r\n\r\nThe eggplant can be reset to its initial, uncut state by grabbing it and hitting it against the blue hitbox over the trash can. Since the eggplant is on the frying pan, the user cannot grab the eggplant directly, but can grab the frying pan and hit it against the hitbox to reset the eggplant.";


        // Update the interactable object label
        interactableObjectLabel.text = "Eggplant Cooking";
        // Disable the label object, otherwise other labels from other ingredients will overlap
        LabelObject.SetActive(false);
    }

    public override void cooked()
    {
        // Set the state to cooked
        state = IngredientState.cooked;

   
        documentation.title = "Cooked Eggplant";
        documentation.description = "This is a cooked eggplant. It cannot be used to bake a cake. The eggplant is currently on the frying pan.\r\n\r\nThe eggplant can be reset to its initial, uncut state by grabbing it and hitting it against the blue hitbox over the trash can. Since the eggplant is on the frying pan, the user cannot grab the eggplant directly, but can grab the frying pan and hit it against the hitbox to reset the eggplant.";
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

        documentation.title = "Uncut Raw Eggplant ";
        documentation.description = "This is an uncut raw eggplant. It cannot be used to bake a cake. The user can grab it and place it on the cutting board.\r\n\r\nOnce on the cutting board, the user can use the kitchen knife to repeatedly hit the eggplant until it's fully cut. An eggplant on the cutting board cannot be grabbed again until it is fully cut. Other ingredients cannot be placed on the cutting board while an uncut eggplant is on it.\r\n\r\nThe eggplant can be reset to its initial, uncut state by grabbing it and hitting it against the blue hitbox over the trash can. Since the eggplant is currently not on the cutting board, it can be reset. Note that if the eggplant is on the cutting board, it cannot be reset until it is fully cut. If it's on the frying pan, the user can't grab it directly, but can grab the frying pan and hit it against the hitbox to reset the eggplant.";

        interactableObjectLabel.text = "Eggplant";
        LabelObject.SetActive(true);

    }

}
