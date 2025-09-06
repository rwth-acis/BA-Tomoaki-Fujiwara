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
                    cupDocumentation.description = "This is the Mixer Cup, used to prepare cake batter on a mixer stand. " +
                    "To start, the user must add three ingredients—flour, egg, and butter—in a specific order:" +
                    "Order 1: Flour -> Egg -> Butter. Order 2: Flour -> Butter -> Egg. " +
                    "Adding ingredients works as follows: Flour: Tilt the flour bag directly over the cup. " +
                    "Egg & Butter: These ingredients cannot be added directly to the cup. " +
                    "The user must first place them into a bowl, then pour the contents of the bowl into the cup. " +
                    "Note that only one ingredient can be in the bowl at a time. " +
                    "If an ingredient is already in the cup, attempting to add it again will have no effect. " +
                    "Once all ingredients have been added, the user can place the cup on the mixer stand to begin mixing the batter. " +
                    "After the batter is created, the user has two options to empty the cup: " +
                    "Tilt the cup over the baking plate to pour the batter. " +
                    "Grab the cup and hit it against the semi-transparent hitbox above the trash can to reset its status. " +
                    "If the user needs a visual guide, use the playVideoClip function." +
                    "Currently the mixercup contains mixed cake batter. The user can pour this in bakeplate";



                }
                /*
                else {
                    kitchenMixerStandDocumentation.description = "This is the Mixer Cup, used to prepare cake batter on a mixer stand. " +
                    "To start, the user must add three ingredients—flour, egg, and butter—in a specific order:" +
                    "Order 1: Flour -> Egg -> Butter. Order 2: Flour -> Butter -> Egg. " +
                    "Adding ingredients works as follows: Flour: Tilt the flour bag directly over the cup. " +
                    "Egg & Butter: These ingredients cannot be added directly to the cup. " +
                    "The user must first place them into a bowl, then pour the contents of the bowl into the cup. " +
                    "Note that only one ingredient can be in the bowl at a time. " +
                    "If an ingredient is already in the cup, attempting to add it again will have no effect. " +
                    "Once all ingredients have been added, the user can place the cup on the mixer stand to begin mixing the batter. " +
                    "After the batter is created, the user has two options to empty the cup: " +
                    "Tilt the cup over the baking plate to pour the batter. " +
                    "Grab the cup and hit it against the semi-transparent hitbox above the trash can to reset its status. " +
                    "If the user needs a visual guide, use the playVideoClip function." +
                    "Currently the mixercup is placed in mixerstand, however not all material are added in cup.";
                }*/

            }
            /*
            else {
                kitchenMixerStandDocumentation.description = "A MixerCup is in the holder, but the mixer head is up. To start mixing, the user must push the mixer head down.";
            }
            */

        } 
        /*
        
        else {
            kitchenMixerStandDocumentation.description = "The MixerStand is currently empty. To operate the mixer, a MixerCup containing the unmixed cake batter must be placed in the holder.";
        }
        */
        //AddBeatedEggLiquid(); // Call this method to check for egg pouring
        //AddMeltedButterLiquid(); // Call this method to check for butter pouring
    }

    public void AddFlour(){
        if (flourAmountInCup != 1.0f)
        {
            flourAmountInCup = flourAmountInCup + 0.01f;
            cupAnimator.Play("pourFlourIn", 0, flourAmountInCup);


            cupDocumentation.title = "Pouring Flour in Mixercup";
            cupDocumentation.description = "This is the Mixer Cup, used to prepare cake batter on a mixer stand. " +
            "To start, the user must add three ingredients—flour, egg, and butter—in a specific order:" +
            "Order 1: Flour -> Egg -> Butter. Order 2: Flour -> Butter -> Egg. " +
            "Adding ingredients works as follows: Flour: Tilt the flour bag directly over the cup. " +
            "Egg & Butter: These ingredients cannot be added directly to the cup. " +
            "The user must first place them into a bowl, then pour the contents of the bowl into the cup. " +
            "Note that only one ingredient can be in the bowl at a time. " +
            "If an ingredient is already in the cup, attempting to add it again will have no effect. " +
            "Once all ingredients have been added, the user can place the cup on the mixer stand to begin mixing the batter. " +
            "After the batter is created, the user has two options to empty the cup: " +
            "Tilt the cup over the baking plate to pour the batter. " +
            "Grab the cup and hit it against the semi-transparent hitbox above the trash can to reset its status. " +
            "If the user needs a visual guide, use the playVideoClip function." +
            "Currently the user is pouring flour in mixercup.";



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
        cupDocumentation.description = "This is the Mixer Cup, used to prepare cake batter on a mixer stand. " +
            "To start, the user must add three ingredients—flour, egg, and butter—in a specific order:" +
            "Order 1: Flour -> Egg -> Butter. Order 2: Flour -> Butter -> Egg. " +
            "Adding ingredients works as follows: Flour: Tilt the flour bag directly over the cup. " +
            "Egg & Butter: These ingredients cannot be added directly to the cup. " +
            "The user must first place them into a bowl, then pour the contents of the bowl into the cup. " +
            "Note that only one ingredient can be in the bowl at a time. " +
            "If an ingredient is already in the cup, attempting to add it again will have no effect. " +
            "Once all ingredients have been added, the user can place the cup on the mixer stand to begin mixing the batter. " +
            "After the batter is created, the user has two options to empty the cup: " +
            "Tilt the cup over the baking plate to pour the batter. " +
            "Grab the cup and hit it against the semi-transparent hitbox above the trash can to reset its status. " +
            "If the user needs a visual guide, use the playVideoClip function." +
            "Currently the mixercup contains Flour in cup. ";



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
            cupDocumentation.description = "This is the Mixer Cup, used to prepare cake batter on a mixer stand. " +
            "To start, the user must add three ingredients—flour, egg, and butter—in a specific order:" +
            "Order 1: Flour -> Egg -> Butter. Order 2: Flour -> Butter -> Egg. " +
            "Adding ingredients works as follows: Flour: Tilt the flour bag directly over the cup. " +
            "Egg & Butter: These ingredients cannot be added directly to the cup. " +
            "The user must first place them into a bowl, then pour the contents of the bowl into the cup. " +
            "Note that only one ingredient can be in the bowl at a time. " +
            "If an ingredient is already in the cup, attempting to add it again will have no effect. " +
            "Once all ingredients have been added, the user can place the cup on the mixer stand to begin mixing the batter. " +
            "After the batter is created, the user has two options to empty the cup: " +
            "Tilt the cup over the baking plate to pour the batter. " +
            "Grab the cup and hit it against the semi-transparent hitbox above the trash can to reset its status. " +
            "If the user needs a visual guide, use the playVideoClip function." +
            "Currently the mixercup contains Flour, Butter and Egg in cup. Its ready to use mixerstand to create cake batter.";


        }
        else {
            interactableObjectLabel.text = "Flour, Egg in cup";

            cupDocumentation.title = "Flour, Egg in Mixercup";
            cupDocumentation.description = "This is the Mixer Cup, used to prepare cake batter on a mixer stand. " +
            "To start, the user must add three ingredients—flour, egg, and butter—in a specific order:" +
            "Order 1: Flour -> Egg -> Butter. Order 2: Flour -> Butter -> Egg. " +
            "Adding ingredients works as follows: Flour: Tilt the flour bag directly over the cup. " +
            "Egg & Butter: These ingredients cannot be added directly to the cup. " +
            "The user must first place them into a bowl, then pour the contents of the bowl into the cup. " +
            "Note that only one ingredient can be in the bowl at a time. " +
            "If an ingredient is already in the cup, attempting to add it again will have no effect. " +
            "Once all ingredients have been added, the user can place the cup on the mixer stand to begin mixing the batter. " +
            "After the batter is created, the user has two options to empty the cup: " +
            "Tilt the cup over the baking plate to pour the batter. " +
            "Grab the cup and hit it against the semi-transparent hitbox above the trash can to reset its status. " +
            "If the user needs a visual guide, use the playVideoClip function." +
            "Currently the mixercup contains Flour, Egg in cup. Still need butter as material.";
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
            cupDocumentation.description = "This is the Mixer Cup, used to prepare cake batter on a mixer stand. " +
            "To start, the user must add three ingredients—flour, egg, and butter—in a specific order:" +
            "Order 1: Flour -> Egg -> Butter. Order 2: Flour -> Butter -> Egg. " +
            "Adding ingredients works as follows: Flour: Tilt the flour bag directly over the cup. " +
            "Egg & Butter: These ingredients cannot be added directly to the cup. " +
            "The user must first place them into a bowl, then pour the contents of the bowl into the cup. " +
            "Note that only one ingredient can be in the bowl at a time. " +
            "If an ingredient is already in the cup, attempting to add it again will have no effect. " +
            "Once all ingredients have been added, the user can place the cup on the mixer stand to begin mixing the batter. " +
            "After the batter is created, the user has two options to empty the cup: " +
            "Tilt the cup over the baking plate to pour the batter. " +
            "Grab the cup and hit it against the semi-transparent hitbox above the trash can to reset its status. " +
            "If the user needs a visual guide, use the playVideoClip function." +
            "Currently the mixercup contains Flour, Butter and Egg in cup. Its ready to use mixerstand to create cake batter.";

        }
        else
        {
            interactableObjectLabel.text = "Flour, Butter in cup";
            cupDocumentation.title = "Flour, Butter in Mixercup";
            cupDocumentation.description = "This is the Mixer Cup, used to prepare cake batter on a mixer stand. " +
            "To start, the user must add three ingredients—flour, egg, and butter—in a specific order:" +
            "Order 1: Flour -> Egg -> Butter. Order 2: Flour -> Butter -> Egg. " +
            "Adding ingredients works as follows: Flour: Tilt the flour bag directly over the cup. " +
            "Egg & Butter: These ingredients cannot be added directly to the cup. " +
            "The user must first place them into a bowl, then pour the contents of the bowl into the cup. " +
            "Note that only one ingredient can be in the bowl at a time. " +
            "If an ingredient is already in the cup, attempting to add it again will have no effect. " +
            "Once all ingredients have been added, the user can place the cup on the mixer stand to begin mixing the batter. " +
            "After the batter is created, the user has two options to empty the cup: " +
            "Tilt the cup over the baking plate to pour the batter. " +
            "Grab the cup and hit it against the semi-transparent hitbox above the trash can to reset its status. " +
            "If the user needs a visual guide, use the playVideoClip function." +
            "Currently the mixercup contains Flour, Butter in cup. Still need butter as material.";

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
            cupDocumentation.description = "This is the Mixer Cup, used to prepare cake batter on a mixer stand. " +
            "To start, the user must add three ingredients—flour, egg, and butter—in a specific order:" +
            "Order 1: Flour -> Egg -> Butter. Order 2: Flour -> Butter -> Egg. " +
            "Adding ingredients works as follows: Flour: Tilt the flour bag directly over the cup. " +
            "Egg & Butter: These ingredients cannot be added directly to the cup. " +
            "The user must first place them into a bowl, then pour the contents of the bowl into the cup. " +
            "Note that only one ingredient can be in the bowl at a time. " +
            "If an ingredient is already in the cup, attempting to add it again will have no effect. " +
            "Once all ingredients have been added, the user can place the cup on the mixer stand to begin mixing the batter. " +
            "After the batter is created, the user has two options to empty the cup: " +
            "Tilt the cup over the baking plate to pour the batter. " +
            "Grab the cup and hit it against the semi-transparent hitbox above the trash can to reset its status. " +
            "If the user needs a visual guide, use the playVideoClip function." +
            "Currently the mixercup contains Flour and part of egg.";

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
            cupDocumentation.description = "This is the Mixer Cup, used to prepare cake batter on a mixer stand. " +
            "To start, the user must add three ingredients—flour, egg, and butter—in a specific order:" +
            "Order 1: Flour -> Egg -> Butter. Order 2: Flour -> Butter -> Egg. " +
            "Adding ingredients works as follows: Flour: Tilt the flour bag directly over the cup. " +
            "Egg & Butter: These ingredients cannot be added directly to the cup. " +
            "The user must first place them into a bowl, then pour the contents of the bowl into the cup. " +
            "Note that only one ingredient can be in the bowl at a time. " +
            "If an ingredient is already in the cup, attempting to add it again will have no effect. " +
            "Once all ingredients have been added, the user can place the cup on the mixer stand to begin mixing the batter. " +
            "After the batter is created, the user has two options to empty the cup: " +
            "Tilt the cup over the baking plate to pour the batter. " +
            "Grab the cup and hit it against the semi-transparent hitbox above the trash can to reset its status. " +
            "If the user needs a visual guide, use the playVideoClip function." +
            "Currently the mixercup contains Flour, butter and part of egg.";

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
            cupDocumentation.description = "This is the Mixer Cup, used to prepare cake batter on a mixer stand. " +
            "To start, the user must add three ingredients—flour, egg, and butter—in a specific order:" +
            "Order 1: Flour -> Egg -> Butter. Order 2: Flour -> Butter -> Egg. " +
            "Adding ingredients works as follows: Flour: Tilt the flour bag directly over the cup. " +
            "Egg & Butter: These ingredients cannot be added directly to the cup. " +
            "The user must first place them into a bowl, then pour the contents of the bowl into the cup. " +
            "Note that only one ingredient can be in the bowl at a time. " +
            "If an ingredient is already in the cup, attempting to add it again will have no effect. " +
            "Once all ingredients have been added, the user can place the cup on the mixer stand to begin mixing the batter. " +
            "After the batter is created, the user has two options to empty the cup: " +
            "Tilt the cup over the baking plate to pour the batter. " +
            "Grab the cup and hit it against the semi-transparent hitbox above the trash can to reset its status. " +
            "If the user needs a visual guide, use the playVideoClip function." +
            "Currently the mixercup contains Flour and part of butter.";

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
            cupDocumentation.description = "This is the Mixer Cup, used to prepare cake batter on a mixer stand. " +
            "To start, the user must add three ingredients—flour, egg, and butter—in a specific order:" +
            "Order 1: Flour -> Egg -> Butter. Order 2: Flour -> Butter -> Egg. " +
            "Adding ingredients works as follows: Flour: Tilt the flour bag directly over the cup. " +
            "Egg & Butter: These ingredients cannot be added directly to the cup. " +
            "The user must first place them into a bowl, then pour the contents of the bowl into the cup. " +
            "Note that only one ingredient can be in the bowl at a time. " +
            "If an ingredient is already in the cup, attempting to add it again will have no effect. " +
            "Once all ingredients have been added, the user can place the cup on the mixer stand to begin mixing the batter. " +
            "After the batter is created, the user has two options to empty the cup: " +
            "Tilt the cup over the baking plate to pour the batter. " +
            "Grab the cup and hit it against the semi-transparent hitbox above the trash can to reset its status. " +
            "If the user needs a visual guide, use the playVideoClip function." +
            "Currently the mixercup contains Flour, egg and part of butter.";

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
        cupDocumentation.description = "This is the Mixer Cup, used to prepare cake batter on a mixer stand. " +
            "To start, the user must add three ingredients—flour, egg, and butter—in a specific order:" +
            "Order 1: Flour -> Egg -> Butter. Order 2: Flour -> Butter -> Egg. " +
            "Adding ingredients works as follows: Flour: Tilt the flour bag directly over the cup. " +
            "Egg & Butter: These ingredients cannot be added directly to the cup. " +
            "The user must first place them into a bowl, then pour the contents of the bowl into the cup. " +
            "Note that only one ingredient can be in the bowl at a time. " +
            "If an ingredient is already in the cup, attempting to add it again will have no effect. " +
            "Once all ingredients have been added, the user can place the cup on the mixer stand to begin mixing the batter. " +
            "After the batter is created, the user has two options to empty the cup: " +
            "Tilt the cup over the baking plate to pour the batter. " +
            "Grab the cup and hit it against the semi-transparent hitbox above the trash can to reset its status. " +
            "If the user needs a visual guide, use the playVideoClip function." +
            "Currently the mixercup is empty.";


        mixedFlourParticle.SetActive(false);


    }





}
