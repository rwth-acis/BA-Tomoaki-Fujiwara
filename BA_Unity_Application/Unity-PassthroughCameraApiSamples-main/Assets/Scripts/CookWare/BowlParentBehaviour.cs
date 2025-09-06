using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using i5.Toolkit.Core.SceneDocumentation;


public class BowlParentBehaviour : MonoBehaviour
{

    public bool isEggInBowl = false;
    public bool isButterInBowl = false;
    public bool isEggBeated = false;

     

    public GameObject meltedButterLiquidObject;


    public GameObject eggWhite;
    public GameObject eggYellow;
    public GameObject beatedEggLiquidObject;

    public GameObject beatedEggParticle;
    public GameObject meltedButterParticle;

    public BeatedEggLiquid beatedEggLiquid;
    public MeltedButterLiquid meltedButterLiquid;


    //Reference to Label
    public TextMeshProUGUI interactableObjectLabel;

    // Get Rerefence to the documentation object
    public DocumentationObject documentation;

    void Start()
    {
        // Initialize the bowl state
        ResetAll();
        interactableObjectLabel.text = "Empty bowl";
    }

    void OnTriggerEnter(Collider other)
    {

        Debug.Log("Bowl Parent Trigger Entered with: " + other.gameObject.name);


        if (other.gameObject.name == "ResetCollider") {
            ResetAll();
        }


        if ((!isEggInBowl)&&(!isButterInBowl))
        {
            if (other.gameObject.name == "butter"){
                //currentInBowl = "Butter";

                isButterInBowl = true;

                meltedButterLiquidObject.SetActive(true);
                meltedButterParticle.SetActive(true);

                meltedButterLiquid.SetFillingRate(1.0f);

                interactableObjectLabel.text = "Melted Butter in Bowl";

                documentation.title = "Melted Butter in Bowl";
                documentation.description = 
                    "This is a bowl used to indirectly add eggs and butter to the mixer cup for making cake batter." +
                    "The user must add flour to the mixer cup first. After that, they can use this bowl to add either an egg or butter. " +
                    "The bowl can only hold one ingredient at a time, so the user must use it at least twice." +
                    "Adding Ingredients To add butter: Grab the butter and bring it into contact with the bowl." +
                    "Liquid butter will be added to the bowl." +
                    "Tilt the bowl over the mixer cup to pour the butter in. The bowl will then become empty." +
                    "To add an egg: Grab the egg and bring it into contact with the bowl." +
                    "The egg will be added to the bowl." +
                    "The user must then use a fork to beat the egg before pouring it into the mixer cup." +
                    "Tilt the bowl over the mixer cup to pour the beaten egg in. The bowl will then become empty." +
                    "Resetting the Bowl To empty the bowl and reset its status, " +
                    "the user can grab it and hit it against the semi-transparent hitbox above the trash can." +
                    "If mixer cup does not has flour inside, nothing will happen."+
                    "Currently the bowl has melted butter inside. It can be poured into mixer cup if the mixer cup has flour and the butter is not added yet.";



            }
            else if (other.gameObject.name == "egg")
            {
                //currentInBowl = "Egg";
                isEggInBowl = true;
                eggWhite.SetActive(true);
                eggYellow.SetActive(true);
                //beatedEggLiquidObject.SetActive(true);
                //beatedEggParticle.SetActive(true);
                //beatedEggLiquid.SetFillingRate(1.0f);

                interactableObjectLabel.text = "Egg in Bowl";


                documentation.title = "Egg in Bowl";
                documentation.description = 
                    "This is a bowl used to indirectly add eggs and butter to the mixer cup for making cake batter." +
                    "The user must add flour to the mixer cup first. After that, they can use this bowl to add either an egg or butter. " +
                    "The bowl can only hold one ingredient at a time, so the user must use it at least twice." +
                    "Adding Ingredients To add butter: Grab the butter and bring it into contact with the bowl." +
                    "Liquid butter will be added to the bowl." +
                    "Tilt the bowl over the mixer cup to pour the butter in. The bowl will then become empty." +
                    "To add an egg: Grab the egg and bring it into contact with the bowl." +
                    "The egg will be added to the bowl." +
                    "The user must then use a fork to beat the egg before pouring it into the mixer cup." +
                    "Tilt the bowl over the mixer cup to pour the beaten egg in. The bowl will then become empty." +
                    "Resetting the Bowl To empty the bowl and reset its status, " +
                    "the user can grab it and hit it against the semi-transparent hitbox above the trash can." +
                    "If mixer cup does not has flour inside, nothing will happen." +
                    "Currently the bowl contains egg. But this egg need to be beated by using fork. So the user should grab a fork and hit against this bowl."+
                    "After that the egg is beated and be able to poured.";


            }
        }

        if (isEggInBowl) { 
            if (other.gameObject.name == "fork")
            {
                if (!isEggBeated)
                {
                    isEggBeated = true;
                    Debug.Log("Fork is in the bowl, beating the egg");
                    BeatTheEgg();
                }
            }

        }


    }


    public void ResetAll()
    {
        Debug.Log("Resetting Bowl Parent Behaviour");
        interactableObjectLabel.text = "Empty bowl";
        isEggInBowl = false;
        isButterInBowl = false;
        isEggBeated = false;
        eggWhite.SetActive(false);
        eggYellow.SetActive(false);
        beatedEggLiquidObject.SetActive(false);
        beatedEggParticle.SetActive(false);
        beatedEggLiquid.SetFillingRate(0.0f);
        meltedButterLiquidObject.SetActive(false);
        meltedButterParticle.SetActive(false);
        meltedButterLiquid.SetFillingRate(0.0f);


        documentation.title = "Empty Bowl";
        documentation.description =
               "This is a bowl used to indirectly add eggs and butter to the mixer cup for making cake batter." +
                    "The user must add flour to the mixer cup first. After that, they can use this bowl to add either an egg or butter. " +
                    "The bowl can only hold one ingredient at a time, so the user must use it at least twice." +
                    "Adding Ingredients To add butter: Grab the butter and bring it into contact with the bowl." +
                    "Liquid butter will be added to the bowl." +
                    "Tilt the bowl over the mixer cup to pour the butter in. The bowl will then become empty." +
                    "To add an egg: Grab the egg and bring it into contact with the bowl." +
                    "The egg will be added to the bowl." +
                    "The user must then use a fork to beat the egg before pouring it into the mixer cup." +
                    "Tilt the bowl over the mixer cup to pour the beaten egg in. The bowl will then become empty." +
                    "Resetting the Bowl To empty the bowl and reset its status, " +
                    "the user can grab it and hit it against the semi-transparent hitbox above the trash can." +
                    "If mixer cup does not has flour inside, nothing will happen." +
                    "Currently the bowl has melted butter inside. It can be poured into mixer cup if the mixer cup has flour and the butter is not added yet.";

    }

    public void BeatTheEgg()
    {
        Debug.Log("Beating the egg");
        interactableObjectLabel.text = "Beated Egg in Bowl";
        eggWhite.SetActive(false);
        eggYellow.SetActive(false);
        beatedEggLiquidObject.SetActive(true);
        beatedEggParticle.SetActive(true);
        beatedEggLiquid.SetFillingRate(1.0f);

        documentation.title = "Beated Egg in Bowl";
        documentation.description =
            "This is a bowl used to indirectly add eggs and butter to the mixer cup for making cake batter." +
                    "The user must add flour to the mixer cup first. After that, they can use this bowl to add either an egg or butter. " +
                    "The bowl can only hold one ingredient at a time, so the user must use it at least twice." +
                    "Adding Ingredients To add butter: Grab the butter and bring it into contact with the bowl." +
                    "Liquid butter will be added to the bowl." +
                    "Tilt the bowl over the mixer cup to pour the butter in. The bowl will then become empty." +
                    "To add an egg: Grab the egg and bring it into contact with the bowl." +
                    "The egg will be added to the bowl." +
                    "The user must then use a fork to beat the egg before pouring it into the mixer cup." +
                    "Tilt the bowl over the mixer cup to pour the beaten egg in. The bowl will then become empty." +
                    "Resetting the Bowl To empty the bowl and reset its status, " +
                    "the user can grab it and hit it against the semi-transparent hitbox above the trash can." +
                    "If mixer cup does not has flour inside, nothing will happen." +
                    "Currently the bowl has beated egg inside. It can be poured into mixer cup if the mixer cup has flour and the egg is not added yet.";


    }

    public void BeatedEggIsAddedCompletely()
    {
        ResetAll();
    }


    public void MeltedButterIsAddedCompletely()
    {
        ResetAll(); ;
    }

}
