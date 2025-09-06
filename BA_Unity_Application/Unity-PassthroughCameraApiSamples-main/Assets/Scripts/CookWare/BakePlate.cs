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
        documentation.description = "This is a baking plate used to bake a cake. " +
            "The user can grab it and place it inside the oven." +
            "To bake a cake, the user must first pour the mixed cake batter onto this plate. " +
            "After that, they can place the plate in the oven and start it." +
            "The outcome depends on the oven settings:" +
            "Successful Baking: " +
            "The cake will be baked if the temperature is set to 170-180 degrees and the timer is set for 30-35 minutes." +
            "No Effect: If either the temperature or time is set too low, nothing will happen." +
            "Burned Cake: " +
            "If both the temperature and time are set too high, the cake will burn, and the baking plate will need to be reset." +
            "To empty the plate and reset its status, the user can grab it and hit it against the semi-transparent hitbox above the trash can." +
            "currently the cake batter is poured in bake plate. The user can place this bake plate and start the oven.";

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
            documentation.description = "This is a baking plate used to bake a cake. " +
            "The user can grab it and place it inside the oven." +
            "To bake a cake, the user must first pour the mixed cake batter onto this plate. " +
            "After that, they can place the plate in the oven and start it." +
            "The outcome depends on the oven settings:" +
            "Successful Baking: " +
            "The cake will be baked if the temperature is set to 170-180 degrees and the timer is set for 30-35 minutes." +
            "No Effect: If either the temperature or time is set too low, nothing will happen." +
            "Burned Cake: " +
            "If both the temperature and time are set too high, the cake will burn, and the baking plate will need to be reset." +
            "To empty the plate and reset its status, the user can grab it and hit it against the semi-transparent hitbox above the trash can." +
            "currently the cake batter is baked successful. The user can empty the bakeplate by trash can to reset the bakeplate.";


        }
        else
        {
            //Debug.LogWarning("Bake plate is not ready to bake.");
        }
    }

    public void BurnedCake()
    {
        // Logic to bake the cake
        if ((isBakePlateReady) && (isFlourInBakePlate))
        {
            Debug.Log("Baking the cake in the bake plate.");
            bakePlateAnimator.Play("BurnedCake", 0);

            interactableObjectLabel.text = "Burned Cake in Plate";

            isBakePlateReady = false; // Cake is baked

            documentation.title = "Burned Sponge Cake in Plate";
            documentation.description = "This is a baking plate used to bake a cake. " +
            "The user can grab it and place it inside the oven." +
            "To bake a cake, the user must first pour the mixed cake batter onto this plate. " +
            "After that, they can place the plate in the oven and start it." +
            "The outcome depends on the oven settings:" +
            "Successful Baking: " +
            "The cake will be baked if the temperature is set to 170-180 degrees and the timer is set for 30-35 minutes." +
            "No Effect: If either the temperature or time is set too low, nothing will happen." +
            "Burned Cake: " +
            "If both the temperature and time are set too high, the cake will burn, and the baking plate will need to be reset." +
            "To empty the plate and reset its status, the user can grab it and hit it against the semi-transparent hitbox above the trash can." +
            "currently the cake in bakeplate is burned. The user need to reset the status of bakeplate by grabbing bake plate and hit against blue semi-transparent hitbox over trash can.";


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
        documentation.description = "This is a baking plate used to bake a cake. " +
            "The user can grab it and place it inside the oven." +
            "To bake a cake, the user must first pour the mixed cake batter onto this plate. " +
            "After that, they can place the plate in the oven and start it." +
            "The outcome depends on the oven settings:" +
            "Successful Baking: " +
            "The cake will be baked if the temperature is set to 170-180 degrees and the timer is set for 30-35 minutes." +
            "No Effect: If either the temperature or time is set too low, nothing will happen." +
            "Burned Cake: " +
            "If both the temperature and time are set too high, the cake will burn, and the baking plate will need to be reset." +
            "To empty the plate and reset its status, the user can grab it and hit it against the semi-transparent hitbox above the trash can." +
            "currently the bakeplate is empty.";


    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bake Plate Trigger Entered with: " + other.gameObject.name);
        if (other.gameObject.name == "ResetCollider") {
            ResetBakePlate();
        }
    }

}