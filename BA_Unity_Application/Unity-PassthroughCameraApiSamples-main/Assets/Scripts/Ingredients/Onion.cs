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

            documentation.title = "Cutting Raw Onion";
            documentation.description = "This is a partially cut raw onion. It cannot be used to bake a cake. The user cannot grab this onion. The user must continue to use the kitchen knife to hit it multiple times until it is fully cut.\r\n\r\nOnce fully cut, the user can grab it and place it on the frying pan to be sautéed. If it is not fully cut, the user cannot grab the onion from the cutting board, nor can they place other ingredients on the board.\r\n\r\nThe onion cannot be reset to its initial, uncut state while it is on the cutting board and not fully cut. If the onion is on the frying pan, the user can't grab it directly, but can grab the frying pan and hit it against the hitbox to reset the onion.";

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

            documentation.title = "Fully Cut Raw Onion";
            documentation.description = "This is a fully cut raw onion. It cannot be used to bake a cake. The user can now grab it and place it on the frying pan to be sautéed.\r\n\r\nTo place it on the frying pan, the frying pan must first be on the stove. Once the onion is placed on the pan, it will begin to cook. It will not overcook.\r\n\r\nThe onion can be reset to its initial, uncut state by grabbing it and hitting it against the blue hitbox over the trash can. Since the onion is fully cut, the user can grab it from the cutting board. If the onion is on the frying pan, the user can't grab it directly, but can grab the frying pan and hit it against the hitbox to reset the onion.";

            interactableObjectLabel.text = "Fully cutted Onion";

        }


    }
    public override void cooking()
    {
        state = IngredientState.cooking;

        Animator animator = cuttedOnion[currentCuts].GetComponent<Animator>();
        animator.SetBool("saute", true);

        documentation.title = "Cooking Onion";
        documentation.description = "This is a cooking onion. It cannot be used to bake a cake. The onion is currently on the frying pan.\r\n\r\nThe onion can be reset to its initial, uncut state by grabbing it and hitting it against the blue hitbox over the trash can. Since the onion is on the frying pan, the user cannot grab the onion directly, but can grab the frying pan and hit it against the hitbox to reset the onion.";


        interactableObjectLabel.text = "Onion Cooking";
        LabelObject.SetActive(false);

    }

    public override void cooked()
    {
        state = IngredientState.cooked;

        documentation.title = "Onion Cooked";
        documentation.description = "This is a cooked onion. It cannot be used to bake a cake. The onion is currently on the frying pan.\r\n\r\nThe onion can be reset to its initial, uncut state by grabbing it and hitting it against the blue hitbox over the trash can. Since the onion is on the frying pan, the user cannot grab the onion directly, but can grab the frying pan and hit it against the hitbox to reset the onion.";
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
        documentation.description = "This is an uncut raw onion. It cannot be used to bake a cake. The user can grab it and place it on the cutting board.\r\n\r\nOnce on the cutting board, the user can use the kitchen knife to repeatedly hit the onion until it's fully cut. An onion on the cutting board cannot be grabbed again until it is fully cut. Other ingredients cannot be placed on the cutting board while an uncut onion is on it.\r\n\r\nThe onion can be reset to its initial, uncut state by grabbing it and hitting it against the blue hitbox over the trash can. Since the onion is currently not on the cutting board, it can be reset. Note that if the onion is on the cutting board, it cannot be reset until it is fully cut. If it's on the frying pan, the user can't grab it directly, but can grab the frying pan and hit it against the hitbox to reset the onion.";

        interactableObjectLabel.text = "Onion";
        LabelObject.SetActive(true);


    }


}
