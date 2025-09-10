using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BrokenPartFixableByRobotArmHandGrabber : MonoBehaviour
{

    //public GameObject visualCollider;
    //public GameObject canvas;
    public GameObject brokeParticle;

    public Image fixingBarImage;

    public bool isBroken = false;
    public string partName = "Unnamed Part";

    private float currentFixedAmount = 0.0f;

    public bool isRobotArmHandGrabberInPlace = false;

    public BoxCollider boxCollider;


    void Update() {
        
        if (isRobotArmHandGrabberInPlace) {
            FixingJoint();
        
        }
        
    }

    public void RobotArmHandGrabberInPlace() { 
        isRobotArmHandGrabberInPlace=true;
    }

    public void RobotArmHandGrabberOutOfPlace() { 
        isRobotArmHandGrabberInPlace=false;
    }


    public void BrokeTheJoint()
    {
        //gameObject.SetActive(true);
        //visualCollider.SetActive(true);
        //canvas.SetActive(true);
        brokeParticle.SetActive(true);
        isBroken = true;
        currentFixedAmount = 0.0f;

        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }

        // Notify the LLM that the part has been fixed.
        if (GeminiAPI.unityAndGeminiInstance != null)
        {
            string notificationMessage = $"System Notification: The part '{partName}' has been successfully repaired.  The robot arm have no problem now.";
            GeminiAPI.unityAndGeminiInstance.SendChatRequest(notificationMessage);
            Debug.Log($"Sent notification to LLM: {notificationMessage}");
        }
        else
        {
            Debug.LogWarning("GeminiAPI instance not found. Could not send repair notification to LLM.");
        }

    }

    public void VisibleTheCollider()
    {
        gameObject.SetActive(true);
    }

    public void FixedTheJoint()
    {
        //visualCollider.SetActive(false);
        //canvas.SetActive(false);
        brokeParticle.SetActive(false);
        gameObject.SetActive(false);
        isBroken = false;

        if (boxCollider != null) { 
            boxCollider.enabled = true;
        }

    }

    public void FixingJoint()
    {
        currentFixedAmount = currentFixedAmount + 0.01f;
        if (currentFixedAmount >= 1.0f)
        {
            currentFixedAmount = 1.0f;
            fixingBarImage.fillAmount = currentFixedAmount;

            FixedTheJoint();
        }
        fixingBarImage.fillAmount = currentFixedAmount;
    }

    public bool brokenStatus() { return isBroken; }

}
