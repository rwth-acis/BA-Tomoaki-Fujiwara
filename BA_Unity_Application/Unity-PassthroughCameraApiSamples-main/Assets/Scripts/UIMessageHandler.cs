using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static RenderTextureToBase64Converter;

public class UIMessageHandler : MonoBehaviour {
    [SerializeField]
    private TMP_InputField userInput;



    [SerializeField]
    private GameObject dialogBox;

    [SerializeField]
    private GameObject parent;

    private string messageToSend;


    public RawImage rawImage; //UI component to display the RenderTexture
    public RenderTexture targetRenderTexture;
    public GameObject imagePreviewer;


    // // List to store RenderTextures
    private List<RenderTexture> renderTextureList = new List<RenderTexture>();

    [Range(0, 100)]
    public int jpgQuality = 75;

    public GameObject messageUserInstance;

    public static UIMessageHandler instance;

    public void SaveRenderTextureCopy() {
        if (rawImage.texture == null) {
            Debug.Log("No picture is taken1");
            return;
        }

        if (targetRenderTexture == null) {
            Debug.Log("No Target Render Texture set");
            return;
        }

        RenderTexture copy = new RenderTexture(targetRenderTexture.width, targetRenderTexture.height, targetRenderTexture.depth, targetRenderTexture.format);
        Graphics.Blit(targetRenderTexture, copy);
        renderTextureList.Add(copy);
        Debug.Log("Saved RenderTexture copy. Current count: " + renderTextureList.Count);
    }

    public void SetLatestRenderTextureToRawImage(RawImage imageComponent, GameObject rawImageObject) {

        if (rawImage.texture == null) {
            Debug.Log("No picture is taken2");
            return;
        }

        if (renderTextureList.Count == 0) {
            Debug.Log("No RenderTexture in list");
            return;
        }
        imageComponent.texture = renderTextureList[renderTextureList.Count - 1];
        imagePreviewer.SetActive(false);
        Debug.Log("RenderTexture set to RawImage");


        rawImageObject.SetActive(true); // Activate the GameObject containing the RawImage  

        unsetPictureTakenFlag();

    }

    public void unsetPictureTakenFlag() {
        rawImage.texture = null;
    }


    public bool isPictureTaken() {
        if (messageUserInstance == null) {
            Debug.LogWarning("messageUserInstance is null");
            return false;
        }
        RawImage rawImageComponent = messageUserInstance.GetComponentInChildren<RawImage>();
        if (rawImageComponent == null) {
            Debug.LogWarning("RawImage not found in messageUserInstance");
            return false;
        }
        return rawImageComponent.texture != null;
    }

    public RenderTexture GetRenderTextureFromUserMessage() {

        RawImage rawImageComponent = messageUserInstance.GetComponentInChildren<RawImage>();

        if (rawImageComponent != null) {

            return rawImageComponent.texture as RenderTexture;
        }
        return null;
    }


    public void ClearRenderTextureList() {
        foreach (var rt in renderTextureList) {
            if (rt != null) rt.Release();
        }
        renderTextureList.Clear();
        Debug.Log("Cleared RenderTexture List");
    }

    /*
    public void ConvertRenderTextureToJPG() {
        if (rawImage.texture == null) {
            Debug.Log("No RawImage set");
            return;
        }

        RenderTexture.active = targetRenderTexture;
        Texture2D texture2D = new Texture2D(targetRenderTexture.width, targetRenderTexture.height, TextureFormat.RGB24, false);
        texture2D.ReadPixels(new Rect(0, 0, targetRenderTexture.width, targetRenderTexture.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = null;

        byte[] bytes = texture2D.EncodeToJPG(jpgQuality);
        Destroy(texture2D);

        jpgBytesList.Add(bytes);
        Debug.Log("Saved Image in Array. Current Image number: " + jpgBytesList.Count);
    }

    public byte[] GetNewestJPG() {
        if (jpgBytesList.Count == 0) return null;
        return jpgBytesList[jpgBytesList.Count - 1];
    }


    public void ClearJPGList() {
        jpgBytesList.Clear();
        Debug.Log("Cleared JPG List");
    }

    public void SetLatestJPGToImageGUI(Image imageComponent) {
        if (rawImage.texture == null) {
            Debug.Log("No RawImage set");
            return;
        }

        byte[] jpgBytes = GetNewestJPG();
        if (jpgBytes == null) {
            Debug.Log("No JPG image available");
            return;
        }

        Texture2D tex = new Texture2D(2, 2);
        if (!tex.LoadImage(jpgBytes)) {
            Debug.LogError("Failed to load image from bytes");
            return;
        }

        Sprite sprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f)
        );
        imageComponent.sprite = sprite;


        // Clear the texture 
        rawImage.texture = null;

        imagePreviewer.SetActive(false);

        Debug.Log("Image Setted");

    }
    */

    private void Awake() {
        instance = this;
    }




    // Update is called once per frame
    public void Send() {
        messageToSend = userInput.text;
        userInput.text = "";

        if (messageToSend == "") {
            return;

        }

        GenerateUserTextBubble(messageToSend);
        GeminiAPI.unityAndGeminiInstance.SendChatRequest(messageToSend);
        //DialogBox.GetComponent<DialogBox>().AddMessage(messageToSend);


    }

    public void GenerateLLMTextBubble(string message) {


        GameObject messageLLMInstance = Instantiate(dialogBox, parent.transform);

        // Set Text in the prefab



        TMP_Text textComponent = messageLLMInstance.GetComponentInChildren<TMP_Text>();





        if (textComponent != null) {
            // Set Text in prefab and instantiate it
            textComponent.text = message;



            //instance.transform.SetParent(parent, false);

            Debug.Log("Send Message");
        } else {
            Debug.LogError("Text component not found in the parent object's children.");
        }
    }

    public void GenerateUserTextBubble(string message) {


        messageUserInstance = Instantiate(dialogBox, parent.transform);

        // Set Text in the prefab



        TMP_Text textComponent = messageUserInstance.GetComponentInChildren<TMP_Text>();





        if (textComponent != null) {
            // Set Text in prefab and instantiate it
            textComponent.text = message;


            SaveRenderTextureCopy();
            RawImage imageComponent = null;
            GameObject rawImageObject;
            Transform[] children = messageUserInstance.GetComponentsInChildren<Transform>(true);
            foreach (var child in children) {
                if (child.name == "ScreenShotImage") {
                    imageComponent = child.GetComponent<RawImage>();
                    rawImageObject = child.gameObject;

                    SetLatestRenderTextureToRawImage(imageComponent, rawImageObject);

                    break;
                }
            }

            //instance.transform.SetParent(parent, false);

            Debug.Log("Send Message");
        } else {
            Debug.LogError("Text component not found in the parent object's children.");
        }
    }

    public void SendTextFromBubbleToInput(string message) {
        userInput.text = message;
    }

}
