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
                documentation.description = "The bowl contains melted butter, an ingredient for the cake batter. The user can grab the bowl to pour the butter into the MixerCup, provided the MixerCup already contains flour. To pour this ingredient, the user need to tilt the bowl. This action will empty the bowl.";



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
                documentation.description = "The bowl contains a raw egg. The user can beat the egg by using a fork on it. Beating the egg will turn it into a liquid, which is an ingredient for the cake batter.";


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
        documentation.description = "This is an empty bowl. The user can place either a raw egg or butter inside. Placing butter will cause it to melt. Placing an egg will allow it to be beaten with a fork.";

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
        documentation.description = "The bowl contains a beaten egg, an ingredient for the cake batter. The user can grab the bowl to pour the beaten egg into the MixerCup, provided the MixerCup already contains flour. To pour this ingredient, the user need to tilt the bowl. This action will empty the bowl.";


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
