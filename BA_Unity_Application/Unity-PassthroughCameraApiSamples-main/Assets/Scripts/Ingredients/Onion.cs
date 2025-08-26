using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using i5.Toolkit.Core.SceneDocumentation;

using Oculus.Interaction.Grab;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction;

public class Onion : Ingredient
{
    public int cutMax = 3;
    public int currentCuts = 0;
    public GameObject[] cuttedOnion;

    public enum IngredientState { raw, cutting, cutted, cooking, cooked, boiling, boiled, seasoned, served }

    public IngredientState state = IngredientState.raw;

    public GameObject LabelObject;
    public TextMeshProUGUI interactableObjectLabel;

    public DocumentationObject documentation;

    public override void cut()
    {
        Debug.Log("Cutting onion");
        if (currentCuts < cutMax - 1)
        {
            cuttedOnion[currentCuts].SetActive(false);
            currentCuts++;
            cuttedOnion[currentCuts].SetActive(true);

            state = IngredientState.cutting;

            documentation.title = "Onion Cutting";
            documentation.description = "Cutting the Onion into smaller pieces. The Onion need to be cut few times more.";

            interactableObjectLabel.text = "Not fully cutted Onion";

        }
        else if (currentCuts < cutMax)
        {
            // Last cut, no more cuts allowed
            cuttedOnion[currentCuts].SetActive(false);
            currentCuts++;
            cuttedOnion[currentCuts].SetActive(true);

            state = IngredientState.cutted;

            // enable the collider of the Onion
            // enable the Grabbable component of the Onion
            cuttedOnion[currentCuts].GetComponent<Collider>().enabled = true;
            cuttedOnion[currentCuts].GetComponent<HandGrabInteractable>().enabled = true;
            cuttedOnion[currentCuts].GetComponent<GrabInteractable>().enabled = true;

            documentation.title = "Onion Cutted";
            documentation.description = "Fully cutted Onion. It is ready to be cooked or boiled.";

            interactableObjectLabel.text = "Fully cutted Onion";

        }


    }
    public override void cooking()
    {
        state = IngredientState.cooking;

        Animator animator = cuttedOnion[currentCuts].GetComponent<Animator>();
        animator.SetBool("saute", true);

        documentation.title = "Onion Cooking";
        documentation.description = "Onion is cooking in the frying pan. Need few more second to finish the cooking.";


        interactableObjectLabel.text = "Onion Cooking";
        LabelObject.SetActive(false);

    }

    public override void cooked()
    {
        state = IngredientState.cooked;

        documentation.title = "Onion Cooked";
        documentation.description = "Fully cooked Onion. Need to seasoning by salt or pepper.";
    }

    public override void seasoned()
    {
        state = IngredientState.seasoned;

        documentation.title = "Onion Cooked and seasoned";
        documentation.description = "Fully cooked Onion and seasoned. Now ready to serve on a dish.";
    }

    public override void served()
    {
        state = IngredientState.served;

        documentation.title = "served Onion";
        documentation.description = "The cooed Onion is served on dish.";
    }

    public override void reset()
    {
        state = IngredientState.raw;

        Animator animator = cuttedOnion[currentCuts].GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetBool("saute", false);
            animator.SetBool("boil", false);
        }





        cuttedOnion[currentCuts].SetActive(false);
        currentCuts = 0;
        cuttedOnion[currentCuts].SetActive(true);

        // enable the collider of the Onion
        // enable the Grabbable component of the Onion
        cuttedOnion[currentCuts].GetComponent<Collider>().enabled = true;
        cuttedOnion[currentCuts].GetComponent<HandGrabInteractable>().enabled = true;
        cuttedOnion[currentCuts].GetComponent<GrabInteractable>().enabled = true;





        // Reset the documentation

        documentation.title = "Raw Onion";
        documentation.description = "This is a raw Onion. This can be placed on the cutboard to be cut. ";

        interactableObjectLabel.text = "Onion";
        LabelObject.SetActive(true);


    }


}
