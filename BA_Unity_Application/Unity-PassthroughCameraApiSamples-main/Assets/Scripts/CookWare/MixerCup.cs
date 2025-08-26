using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Oculus.Interaction;
using i5.Toolkit.Core.SceneDocumentation;


public class MixerCup : MonoBehaviour
{

    [SerializeField] private GameObject flourInCup;
    [SerializeField] private GameObject mixerFlourInCup;


    [SerializeField] private Animator cupAnimator;

    public float flourAmountInCup = 0.0f;
    public float eggAmountInCup = 0.0f; // 0 = no egg, 1 = egg added
    public float butterAmountInCup = 0.0f; // 0 = no egg, 1 = egg added

    public int liquidAmount = 0; // 0 = empty, 1 = egg or butter, 2 = both

    public bool isFlourAdded = false;



    public bool isEggPouring = false;
    public bool isEggAdded = false;

    public bool isButterPouring = false;
    public bool isButterAdded = false;


    public BeatedEggLiquid beatedEggLiquid;
    public MeltedButterLiquid meltedButterLiquid;

    public BowlParentBehaviour bowlparentBehaviour;

    // Reference to Label
    public TextMeshProUGUI interactableObjectLabel;


    // 
    public GameObject mixedFlourParticle;

    // Reference to run mixer
    public bool isFlourReadyToMix = false;
    public bool isFlourMixed = false;
    public CustomOneGrabRotateTransformer mixerJointRotation;
    public MixerCupParentBehaviour mixerCupParentBehaviour;

    public Animator mixerStandAnimator;

    // Get Rerefence to the documentation object
    public DocumentationObject cupDocumentation;

    public DocumentationObject kitchenMixerStandDocumentation;

    // Start is called before the first frame update
    void Start()
    {
        interactableObjectLabel.text = "Empty cup";
        mixedFlourParticle.SetActive(false);
    }

    public void Update()
    {

        if (mixerCupParentBehaviour.IsCupInHolder()) {
            if (mixerJointRotation.CurrentConstrainedAngle == 54)
            {
                //Debug.Log("Mixer is ready to mix");

                if ((isFlourReadyToMix) && (!isFlourMixed))
                {
                    isFlourMixed = true;

                    mixerStandAnimator.Play("RotateMixerBlade", 0);

                    cupAnimator.Play("init", 0);

                    flourInCup.SetActive(false);
                    mixerFlourInCup.SetActive(true);
                    // Enable to drop mixedFlour
                    mixedFlourParticle.SetActive(true);

                    interactableObjectLabel.text = "Mixed cake batter";

                    cupDocumentation.title = "Mixed cake batter in Mixercup";
                    cupDocumentation.description = "The MixerCup contains the final mixed cake batter. The next step is to pour the batter onto the BakePlate by grabbing and tilting the MixerCup. For details on baking, refer to the documentation for the BakePlate or Oven objects.";




                }
                else {
                    kitchenMixerStandDocumentation.description = "The MixerCup is in place and the mixer head is down, but the cup does not contain the complete unmixed cake batter (flour, egg, and butter). The mixer will not operate until all ingredients are present in the cup.";
                }

            }
            else {
                kitchenMixerStandDocumentation.description = "A MixerCup is in the holder, but the mixer head is up. To start mixing, the user must push the mixer head down.";
            }


        } else {
            kitchenMixerStandDocumentation.description = "The MixerStand is currently empty. To operate the mixer, a MixerCup containing the unmixed cake batter must be placed in the holder.";
        }
        
        //AddBeatedEggLiquid(); // Call this method to check for egg pouring
        //AddMeltedButterLiquid(); // Call this method to check for butter pouring
    }

    public void AddFlour(){
        if (flourAmountInCup != 1.0f)
        {
            flourAmountInCup = flourAmountInCup + 0.01f;
            cupAnimator.Play("pourFlourIn", 0, flourAmountInCup);


            cupDocumentation.title = "Pouring Flour in Mixercup";
            cupDocumentation.description = "The user is currently pouring flour into the MixerCup. This action must be completed before other ingredients can be added.";



            //mixerFlourInCup.SetActive(false);
            //flourInCup.SetActive(true);
            if (flourAmountInCup >= 1.0f) {
                flourAmountInCup = 1.0f; // Cap the flour amount at 1
                FlourAddedCompletely();
            }
        }
    }

    public void FlourAddedCompletely(){
        Debug.Log("Flour added completely");
        isFlourAdded = true;
        flourInCup.SetActive(true);
        mixerFlourInCup.SetActive(false);
        cupAnimator.SetBool("isFlourAdded", isFlourAdded);

        interactableObjectLabel.text = "Flour in cup";

        cupDocumentation.title = "Flour in Mixercup";
        cupDocumentation.description = "The MixerCup is now filled with flour. The next step is to add the other ingredients: melted butter and a beaten egg. Once all ingredients are added, the cup can be placed in the MixerStand to be mixed.";



    }

    public void BeatedEggAddedCompletely(){
        Debug.Log("Beated egg added completely");
        isEggAdded = true;
        isEggPouring = false; // Stop pouring animation
        //cupAnimator.SetBool("isEggAdded", isEggAdded);
        //eggAmountInCup = 1.0f; // Set egg amount to 1 when added completely
        liquidAmount++; // Increment liquid amount to indicate egg is added

        bowlparentBehaviour.BeatedEggIsAddedCompletely(); // Notify the bowl parent that egg is added

        if (isButterAdded)
        {
            interactableObjectLabel.text = "Unmixed cake batter";
            isFlourReadyToMix = true;

            cupDocumentation.title = "Unmixed cake batter Mixercup";
            cupDocumentation.description = "The MixerCup contains all the necessary ingredients for the cake batter (flour, beaten egg, and melted butter), but it is not yet mixed. The user should now place the MixerCup into the MixerStand and push the mixer head down to mix the batter.";


        }
        else {
            interactableObjectLabel.text = "Flour, Egg in cup";

            cupDocumentation.title = "Flour, Egg in Mixercup";
            cupDocumentation.description = "The MixerCup contains flour and a beaten egg. It still needs melted butter to complete the unmixed cake batter.";

        }

    }

    public void MeltedButterAddedCompletely()
    {
        Debug.Log("Melted added completely");
        isButterAdded = true;
        isButterPouring = false; // Stop pouring animation
        cupAnimator.SetBool("isButterAdded", isButterAdded);
        //eggAmountInCup = 1.0f; // Set egg amount to 1 when added completely
        liquidAmount++; // Increment liquid amount to indicate egg is added

        bowlparentBehaviour.MeltedButterIsAddedCompletely(); // Notify the bowl parent that butter is added

        if (isEggAdded)
        {
            interactableObjectLabel.text = "Unmixed cake batter";
            isFlourReadyToMix = true;

            cupDocumentation.title = "Unmixed cake batter Mixercup";
            cupDocumentation.description = "The MixerCup contains all the necessary ingredients for the cake batter (flour, beaten egg, and melted butter), but it is not yet mixed. The user should now place the MixerCup into the MixerStand and push the mixer head down to mix the batter.";

        }
        else
        {
            interactableObjectLabel.text = "Flour, Butter in cup";
            cupDocumentation.title = "Flour, Butter in Mixercup";
            cupDocumentation.description = "The MixerCup contains flour and melted butter. It still needs a beaten egg to complete the unmixed cake batter.";

        }

    }

    public void AddBeatedEggLiquid(){
        
        if (!isFlourAdded) { 
            return; // Prevent adding egg if flour is not added
        }

        if (isButterPouring) { 
            return; // Prevent adding egg if butter is being poured
        }

        if (isEggAdded) {
            return; // Prevent adding egg if it has already been added
        }
        
        if (liquidAmount < 1){
            eggAmountInCup = eggAmountInCup + 0.01f;
            cupAnimator.Play("pouringOneTime", 0, eggAmountInCup);

            beatedEggLiquid.SetFillingRate(1 - eggAmountInCup);

            cupDocumentation.title = "Pouring Beaten Egg liquid in MixerCup";
            cupDocumentation.description = "The user is currently pouring a beaten egg into the MixerCup, which already contains flour. This action must be completed. Afterwards, melted butter needs to be added.";

            interactableObjectLabel.text = "Flour and part of egg in cup";

            if (eggAmountInCup >= 1.0f)
            {
                eggAmountInCup = 1.0f; // Cap the flour amount at 1
                BeatedEggAddedCompletely();
            }

            

        }
        else if (liquidAmount < 2){
            eggAmountInCup = eggAmountInCup + 0.01f;
            cupAnimator.Play("pouringTwoTime", 0, eggAmountInCup);

            beatedEggLiquid.SetFillingRate(1 - eggAmountInCup);

            cupDocumentation.title = "Pouring Beaten Egg liquid in MixerCup";
            cupDocumentation.description = "The user is currently pouring a beaten egg into the MixerCup, which already contains flour and butter. Once this action is complete, the cup will contain the unmixed cake batter, ready to be mixed in the MixerStand.";

            interactableObjectLabel.text = "Flour, butter and part of egg in cup";

            if (eggAmountInCup >= 1.0f)
            {
                eggAmountInCup = 1.0f; // Cap the flour amount at 1
                BeatedEggAddedCompletely();
            }
        }
    }

    public void AddMeltedButterLiquid()
    {

        if (!isFlourAdded)
        {
            return; // Prevent adding egg if flour is not added
        }

        if (isEggPouring)
        {
            return; // Prevent adding egg if butter is being poured
        }

        if (isButterAdded)
        {
            return; // Prevent adding egg if it has already been added
        }

        if (liquidAmount < 1)
        {
            butterAmountInCup = butterAmountInCup + 0.01f;
            cupAnimator.Play("pouringOneTime", 0, butterAmountInCup);

            meltedButterLiquid.SetFillingRate(1 - butterAmountInCup);

            cupDocumentation.title = "Pouring butter liquid in MixerCup";
            cupDocumentation.description = "The user is currently pouring melted butter into the MixerCup, which already contains flour. This action must be completed. Afterwards, a beaten egg needs to be added.";

            interactableObjectLabel.text = "Flour and part of butter in cup";

            if (butterAmountInCup >= 1.0f)
            {
                butterAmountInCup = 1.0f; // Cap the flour amount at 1
                MeltedButterAddedCompletely();
            }


        }
        else if (liquidAmount < 2)
        {
            butterAmountInCup = butterAmountInCup + 0.01f;
            cupAnimator.Play("pouringTwoTime", 0, butterAmountInCup);

            meltedButterLiquid.SetFillingRate(1 - butterAmountInCup);

            cupDocumentation.title = "Pouring butter in MixerCup";
            cupDocumentation.description = "The user is currently pouring melted butter into the MixerCup, which already contains flour and a beaten egg. Once this action is complete, the cup will contain the unmixed cake batter, ready to be mixed in the MixerStand.";

            interactableObjectLabel.text = "Flour, egg and part of butter in cup";

            if (butterAmountInCup >= 1.0f)
            {
                butterAmountInCup = 1.0f; // Cap the flour amount at 1
                MeltedButterAddedCompletely();
            }
        }
    }



    /*
    public void PourLiquid(){
        if (liquidAmount < 2){
            liquidAmount++;
            cupAnimator.SetInteger("pouredTime", liquidAmount);
        }
    }*/

    public void MixContainment() {
        flourInCup.SetActive(false);
        mixerFlourInCup.SetActive(true);
        liquidAmount = 0;
        cupAnimator.SetInteger("pouredTime", liquidAmount);
    }


    public void ResetCupFood(){
        Debug.Log("Resetting cup");
        flourAmountInCup = 0.0f;
        eggAmountInCup = 0.0f;
        butterAmountInCup = 0.0f;
        isFlourAdded = false;
        isEggAdded = false;
        isEggPouring = false;
        isButterAdded = false;
        isButterPouring = false;
        flourInCup.SetActive(false);
        mixerFlourInCup.SetActive(false);
        //cupAnimator.SetBool("isFlourAdded", isFlourAdded);
        //cupAnimator.SetBool("isEggAdded", isEggAdded);
        //cupAnimator.SetBool("isButterAdded", isButterAdded);
        interactableObjectLabel.text = "Empty cup";
        liquidAmount = 0; // Reset liquid amount

        isFlourReadyToMix = false;
        isFlourMixed = false;

        cupAnimator.Play("init", 0, 1);

        cupDocumentation.title = "Empty Mixercup";
        cupDocumentation.description = "This is an empty MixerCup, used for preparing cake batter. The process is: 1. Add flour. 2. Add a beaten egg and melted butter. 3. Place the cup in the MixerStand and operate it to mix the batter. 4. Pour the mixed batter onto the BakePlate.";


        mixedFlourParticle.SetActive(false);


    }





}
