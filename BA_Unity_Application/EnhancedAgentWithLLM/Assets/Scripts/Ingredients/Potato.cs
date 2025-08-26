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

public class Potato : Ingredient
{
    public int cutMax = 3;
    public int currentCuts = 0;
    public GameObject[] cutedPotatos;

    public enum IngredientState { raw, cutting, cutted, cooking, cooked, boiling, boiled, seasoned, served }

    public IngredientState state = IngredientState.raw;

    public GameObject LabelObject;
    public TextMeshProUGUI interactableObjectLabel;

    public DocumentationObject documentation;

    public override void cut() {
        Debug.Log("Cutting potato");
        if (currentCuts < cutMax - 1)
        {
            cutedPotatos[currentCuts].SetActive(false);
            currentCuts++;
            cutedPotatos[currentCuts].SetActive(true);

            state = IngredientState.cutting;

            documentation.title = "Potato Cutting";
            documentation.description = "Cutting the Potato into smaller pieces. The Potato need to be cut few times more.";

            interactableObjectLabel.text = "Not fully cutted Potato";

        }
        else if (currentCuts < cutMax)
        {
            // Last cut, no more cuts allowed
            cutedPotatos[currentCuts].SetActive(false);
            currentCuts++;
            cutedPotatos[currentCuts].SetActive(true);

            state = IngredientState.cutted;

            // enable the collider of the potato
            // enable the Grabbable component of the potato
            cutedPotatos[currentCuts].GetComponent<Collider>().enabled = true;
            cutedPotatos[currentCuts].GetComponent<HandGrabInteractable>().enabled = true;
            cutedPotatos[currentCuts].GetComponent<GrabInteractable>().enabled = true;

            documentation.title = "Potato Cutted";
            documentation.description = "Fully cutted Potato. It is ready to be cooked or boiled.";

            interactableObjectLabel.text = "Fully cutted Potato";

        }


    }
    public override void cooking()
    {
        state = IngredientState.cooking;

        Animator animator = cutedPotatos[currentCuts].GetComponent<Animator>();
        animator.SetBool("saute", true);

        documentation.title = "Potato Cooking";
        documentation.description = "Potato is cooking in the frying pan. Need few more second to finish the cooking.";


        interactableObjectLabel.text = "Potato Cooking";
        LabelObject.SetActive(false);

    }

    public override void cooked()
    {
        state = IngredientState.cooked;

        documentation.title = "Potato Cooked";
        documentation.description = "Fully cooked Potato. Need to seasoning by salt or pepper.";
    }

    public override void seasoned()
    {
        state = IngredientState.seasoned;

        documentation.title = "Potato Cooked and seasoned";
        documentation.description = "Fully cooked Potato and seasoned. Now ready to serve on a dish.";
    }

    public override void served()
    {
        state = IngredientState.served;

        documentation.title = "served Potato";
        documentation.description = "The cooed Potato is served on dish.";
    }

    public override void reset()
    {
        state = IngredientState.raw;

        Animator animator = cutedPotatos[currentCuts].GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetBool("saute", false);
            animator.SetBool("boil", false);
        }

        



        cutedPotatos[currentCuts].SetActive(false);
        currentCuts = 0;
        cutedPotatos[currentCuts].SetActive(true);

        // enable the collider of the potato
        // enable the Grabbable component of the potato
        cutedPotatos[currentCuts].GetComponent<Collider>().enabled = true;
        cutedPotatos[currentCuts].GetComponent<HandGrabInteractable>().enabled = true;
        cutedPotatos[currentCuts].GetComponent<GrabInteractable>().enabled = true;


        


        // Reset the documentation

        documentation.title = "Raw Potato";
        documentation.description = "This is a raw potato. This can be placed on the cutboard to be cut. ";

        interactableObjectLabel.text = "Potato";
        LabelObject.SetActive(true);


    }


}
