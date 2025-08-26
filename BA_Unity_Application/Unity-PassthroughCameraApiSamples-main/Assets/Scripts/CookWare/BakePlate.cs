using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using i5.Toolkit.Core.SceneDocumentation;


public class BakePlate : MonoBehaviour
{

    public Animator bakePlateAnimator;
    public bool isFlourInBakePlate = false; // Flag to check if flour is in the bake plate
    public bool isBakePlateReady = false;

    // Reference to Label
    public TextMeshProUGUI interactableObjectLabel;

    // Get Rerefence to the documentation object
    public DocumentationObject documentation;

    public void PourFlour()
    {

        if (isFlourInBakePlate)
        {
            //Debug.LogWarning("Flour is already in the bake plate.");
            return;
        }

        // Logic to pour flour into the bake plate
        Debug.Log("Pouring flour into the bake plate.");

        bakePlateAnimator.Play("PourFlourIn", 0);

        isBakePlateReady = true;

        isFlourInBakePlate = true;

        interactableObjectLabel.text = "Batter in Bake Plate";

        documentation.title = "Batter in Bake Plate";
        documentation.description = "The BakePlate now contains unbaked cake batter and is ready for the oven. The next step is to place this BakePlate inside a preheated oven. For specific baking instructions, consult the documentation for the Oven object.";


    }

    public void BakeCake()
    {
        // Logic to bake the cake
        if ((isBakePlateReady)&&(isFlourInBakePlate))
        {
            Debug.Log("Baking the cake in the bake plate.");
            bakePlateAnimator.Play("bakeCake", 0);

            interactableObjectLabel.text = "Baked Cake in Plate";

            isBakePlateReady = false; // Cake is baked

            documentation.title = "Baked Sponge Cake in Plate";
            documentation.description = "A sponge cake has been successfully baked on the BakePlate. The user can now remove the cake from the plate. The next step is to decorate it with cream and strawberries.";



        }
        else
        {
            //Debug.LogWarning("Bake plate is not ready to bake.");
        }
    }

    public void ResetBakePlate()
    {
        // Logic to reset the bake plate
        Debug.Log("Resetting the bake plate.");
        isBakePlateReady = false;
        isFlourInBakePlate = false;
        bakePlateAnimator.Play("init", 0);

        interactableObjectLabel.text = "Empty BakePlate";

        documentation.title = "Empty BakePlate";
        documentation.description = "This is an empty BakePlate, used for baking a cake. The first step is to fill it with mixed cake batter from the MixerCup. Once filled, it can be placed in the oven for baking. Refer to the Oven's documentation for baking instructions.";


    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bake Plate Trigger Entered with: " + other.gameObject.name);
        if (other.gameObject.name == "ResetCollider") {
            ResetBakePlate();
        }
    }

}