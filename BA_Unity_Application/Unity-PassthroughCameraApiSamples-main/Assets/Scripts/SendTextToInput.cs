using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SendTextToInput : MonoBehaviour
{

    [SerializeField]
    private TMPro.TextMeshProUGUI userInput;



    public void SendTextToInputField() {
        UIMessageHandler.instance.SendTextFromBubbleToInput(userInput.text);
    }
}
