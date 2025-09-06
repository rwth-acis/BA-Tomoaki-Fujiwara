using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Interaction;
using i5.Toolkit.Core.SceneDocumentation;
using TMPro;
public class OvenParentBehaviour : MonoBehaviour
{

    public CustomOneGrabRotateTransformer tempHandle;
    public CustomOneGrabRotateTransformer timeHandle;

    // Get Rerefence to the documentation object
    public DocumentationObject ovenDocumentation;

    private int inputTemp = 0; // Temperature in degrees Celsius
    private int inputTime = 0; // Time in minutes

    // These are for oven Door;
    public CustomOneGrabRotateTransformer ovenDoorTransformer;
    public Transform ovenDoorTransform;
    public Grabbable ovenGrabbable;


    public TextMeshProUGUI uiTempText;
    public TextMeshProUGUI uiTimeText;

    public bool isOvenOn = false; // Flag to check if the oven is on
    public bool isOvenDoorOpen = false; // Flag to check if the oven door is open

    public GameObject ovenLight; // Reference to the oven light GameObject


    public OvenBakeCollider ovenBakeCollider; // Reference to the oven bake collider

    public BakePlate bakePlate; // Reference to the bake plate object


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateOvenHandles();
        //BakeOven();
    }

    private void UpdateOvenHandles()
    {
        // Update the UI text with the current constrained angle of the temperature and time handle
        // Time
        // min: -100 degrees (5min), max: 100 degrees (60min)
        // 0.9min per degree
        // Neutral degree is 0, which means 32.5 min
        inputTime = (int)(32.5 + (timeHandle.CurrentConstrainedAngle * -0.275));

        // Temperature
        // min: -100 degrees (80min), max: 100 degrees (250min)
        // 0.85min per degree
        // Neutral degree is 0, which means 80+85=165 min
        inputTemp = 165 + (int)(tempHandle.CurrentConstrainedAngle * -0.85f);

        // You can add temperature calculation here. For example:
        // inputTemp = 150 + (int)(tempHandle.CurrentConstrainedAngle * 2.0f);

        // Update the UI text fields
        if (uiTimeText != null)
        {
            uiTimeText.text = inputTime.ToString();
        }
        if (uiTempText != null)
        {
            // Example of updating temperature text
            uiTempText.text = inputTemp.ToString();
        }

        ovenDocumentation.description = "This is an oven for baking cakes. " +
            "To use it, the user needs to set the temperature and time, place the baking plate inside, and start the oven." +
            "Here are the steps to bake a cake: Set the temperature and time correctly: " +
            "The temperature should be between 170-180 degrees and the timer should be set for 30-35 minutes." +
            "Place the baking plate (with the cake batter) inside the oven. " +
            "Close the oven door and press the red start button. " +
            "Once the door is closed and the red button is pressed, the oven will begin to work. " +
            "The internal light will turn on, providing a visual indicator that it's operating." +
            "The outcome of the baking process depends on the settings: " +
            "If both the temperature and time are set correctly, the cake will be baked successfully. " +
            "If either setting is too low, nothing will happen. " +
            "If both the temperature and time are set too high, the cake will burn, and the baking plate will need to be reset." +
            "To check the status of the baking plate, refer to its documentation component." +
            $"current temperature is at: {uiTempText.text} Degree." + $"current time is at:{uiTimeText.text} min";
    }

    public void BakeOven()
    {
        if ((!isOvenOn) && (!isOvenDoorOpen)) { 
            // Start baking process
            ovenLight.SetActive(true); // Turn on the oven light

            if (ovenBakeCollider.IsBakePlateInOven()) {

                // Cake is burned
                if ((180 < inputTemp) && (35 < inputTime))
                {
                    bakePlate.BakeCake(); 
                }

                // Cake is baked successfully
                else if (((inputTemp<=180) && (170 <= inputTemp)) && ((inputTime <= 35) && (30 <= inputTime)))
                {
                    bakePlate.BakeCake(); // Call the bake method on the bake plate
                }
                

            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ovenDoorCloseCollider")
        {
            //Close the oven door
            isOvenDoorOpen = false;
            ovenGrabbable.enabled = false; // Disable grabbable to prevent interaction while closing
            ovenDoorTransformer.SetRelativeAngle(0); // Reset the oven door angle to closed position
            ovenDoorTransform.localRotation = Quaternion.Euler(0, 0, 0); // Reset the rotation to closed position
            ovenGrabbable.enabled = true; // Re-enable grabbable after closing
            // Debug.Log("Oven door opened");
        } 

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "ovenDoorCloseCollider")
        {
            isOvenDoorOpen = true;
            ovenLight.SetActive(false); // Turn off the oven light
            // Debug.Log("Oven door closed");
        }
    }


}
