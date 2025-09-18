using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeminiAPIError : MonoBehaviour
{
    public GameObject errorMessage;
    public TMP_Text errorName;
    public TMP_Text errorDescription;

    public void ShowError(string name, string description)
    {
        errorMessage.SetActive(true);
        errorName.text = name;
        errorDescription.text = description;
    }
}
