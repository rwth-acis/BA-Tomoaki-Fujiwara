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

            documentation.title = "Cutting Raw Potato";
            documentation.description = "This is a partially cut raw potato. It cannot be used to bake a cake. The user cannot grab this potato. The user must continue to use the kitchen knife to hit it multiple times until it is fully cut.\r\n\r\nOnce fully cut, the user can grab it and place it on the frying pan to be sautéed. If it is not fully cut, the user cannot grab the potato from the cutting board, nor can they place other ingredients on the board.\r\n\r\nThe potato cannot be reset to its initial, uncut state while it is on the cutting board and not fully cut. If the potato is on the frying pan, the user can't grab it directly, but can grab the frying pan and hit it against the hitbox to reset the potato.";

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

            documentation.title = "Fully Cut Raw Potato";
            documentation.description = "This is a fully cut raw potato. It cannot be used to bake a cake. The user can now grab it and place it on the frying pan to be sautéed.\r\n\r\nTo place it on the frying pan, the frying pan must first be on the stove. Once the potato is placed on the pan, it will begin to cook. It will not overcook.\r\n\r\nThe potato can be reset to its initial, uncut state by grabbing it and hitting it against the blue hitbox over the trash can. Since the potato is fully cut, the user can grab it from the cutting board. If the potato is on the frying pan, the user can't grab it directly, but can grab the frying pan and hit it against the hitbox to reset the potato.";

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

        documentation.title = "Cooked Potato";
        documentation.description = "This is a cooked potato. It cannot be used to bake a cake. The potato is currently on the frying pan.\r\n\r\nThe potato can be reset to its initial, uncut state by grabbing it and hitting it against the blue hitbox over the trash can. Since the potato is on the frying pan, the user cannot grab the potato directly, but can grab the frying pan and hit it against the hitbox to reset the potato.";
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

        documentation.title = "uncut raw potato";
        documentation.description = "This is an uncut raw potato. It cannot be used to bake a cake. The user can grab it and place it on the cutting board.\r\n\r\nOnce on the cutting board, the user can use the kitchen knife to repeatedly hit the potato until it's fully cut. A potato on the cutting board cannot be grabbed again until it is fully cut. Other ingredients cannot be placed on the cutting board while an uncut potato is on it.\r\n\r\nThe potato can be reset to its initial, uncut state by grabbing it and hitting it against the blue hitbox over the trash can. Since the potato is currently not on the cutting board, it can be reset. Note that if the potato is on the cutting board, it cannot be reset until it is fully cut. If it's on the frying pan, the user can't grab it directly, but can grab the frying pan and hit it against the hitbox to reset the potato.";

        interactableObjectLabel.text = "Potato";
        LabelObject.SetActive(true);


    }


}
