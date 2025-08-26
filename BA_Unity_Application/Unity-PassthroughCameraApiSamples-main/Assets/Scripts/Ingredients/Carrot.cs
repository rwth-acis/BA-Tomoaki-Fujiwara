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

public class Carrot : Ingredient
{
    public int cutMax = 3;
    public int currentCuts = 0;
    public GameObject[] cuttedCarrot;

    public enum IngredientState { raw, cutting, cutted, cooking, cooked, boiling, boiled, seasoned, served }

    public IngredientState state = IngredientState.raw;

    public GameObject LabelObject;
    public TextMeshProUGUI interactableObjectLabel;

    public DocumentationObject documentation;

    public override void cut()
    {
        Debug.Log("Cutting carrot");
        if (currentCuts < cutMax - 1)
        {
            cuttedCarrot[currentCuts].SetActive(false);
            currentCuts++;
            cuttedCarrot[currentCuts].SetActive(true);

            state = IngredientState.cutting;

            documentation.title = "Carrot Cutting";
            documentation.description = "Cutting the carrot into smaller pieces. The carrot needs to be cut a few times more.";

            interactableObjectLabel.text = "Not fully cut carrot";

        }
        else if (currentCuts < cutMax)
        {
            // Last cut, no more cuts allowed
            cuttedCarrot[currentCuts].SetActive(false);
            currentCuts++;
            cuttedCarrot[currentCuts].SetActive(true);

            state = IngredientState.cutted;

            // enable the collider of the carrot
            // enable the Grabbable component of the carrot
            cuttedCarrot[currentCuts].GetComponent<Collider>().enabled = true;
            cuttedCarrot[currentCuts].GetComponent<HandGrabInteractable>().enabled = true;
            cuttedCarrot[currentCuts].GetComponent<GrabInteractable>().enabled = true;

            documentation.title = "Carrot Cutted";
            documentation.description = "Fully cut carrot. It is ready to be cooked or boiled.";

            interactableObjectLabel.text = "Fully cut carrot";

        }
    }

    public override void cooking()
    {
        state = IngredientState.cooking;

        Animator animator = cuttedCarrot[currentCuts].GetComponent<Animator>();
        animator.SetBool("saute", true);

        documentation.title = "Carrot Cooking";
        documentation.description = "Carrot is cooking in the frying pan. Needs a few more seconds to finish cooking.";

        interactableObjectLabel.text = "Carrot Cooking";
        LabelObject.SetActive(false);
    }

    public override void cooked()
    {
        state = IngredientState.cooked;

        documentation.title = "Carrot Cooked";
        documentation.description = "Fully cooked carrot. Needs seasoning by salt or pepper.";
    }

    public override void seasoned()
    {
        state = IngredientState.seasoned;

        documentation.title = "Carrot Cooked and Seasoned";
        documentation.description = "Fully cooked carrot and seasoned. Now ready to serve on a dish.";
    }

    public override void served()
    {
        state = IngredientState.served;

        documentation.title = "Served Carrot";
        documentation.description = "The cooked carrot is served on dish.";
    }

    public override void reset()
    {
        state = IngredientState.raw;

        Animator animator = cuttedCarrot[currentCuts].GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetBool("saute", false);
            animator.SetBool("boil", false);
        }

        cuttedCarrot[currentCuts].SetActive(false);
        currentCuts = 0;
        cuttedCarrot[currentCuts].SetActive(true);

        // enable the collider of the carrot
        // enable the Grabbable component of the carrot
        cuttedCarrot[currentCuts].GetComponent<Collider>().enabled = true;
        cuttedCarrot[currentCuts].GetComponent<HandGrabInteractable>().enabled = true;
        cuttedCarrot[currentCuts].GetComponent<GrabInteractable>().enabled = true;

        // Reset the documentation
        documentation.title = "Raw Carrot";
        documentation.description = "This is a raw carrot. This can be placed on the cutboard to be cut.";

        interactableObjectLabel.text = "Carrot";
        LabelObject.SetActive(true);
    }
}
