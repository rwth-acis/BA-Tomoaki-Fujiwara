using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; 
using Newtonsoft.Json;
public class RenderTextureToBase64Converter : MonoBehaviour {
    public RenderTexture targetRenderTexture;

    public enum ImageFormat { PNG, JPG }
    public ImageFormat outputFormat = ImageFormat.PNG;

    public string userPrompt = "What do you see from this image";

    [Range(0, 100)]
    public int jpgQuality = 75;


    public string apiKey = "AIzaSyBX_xWhb5__6kQAAZENsigtFaup9LKoOlE";
    public string apiEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent"; 

    public IEnumerator SendRenderTextureToGemini() {
        string base64Image = ConvertRenderTextureToBase64();
        if (string.IsNullOrEmpty(base64Image)) {
            Debug.LogError("Error by base 64 convert");
            yield break;
        }


        GeminiImageRequest requestData = new GeminiImageRequest {
            contents = new GeminiContent[]
            {
                new GeminiContent
                {
                    role = "user",
                    parts = new GeminiPart[]
                    {
                        // Text
                        new GeminiPart { text = userPrompt, inlineData=null }, 
                        // Image
                        new GeminiPart
                        {
                            text = null,
                            inlineData = new GeminiInlineData
                            {
                                mimeType = outputFormat == ImageFormat.PNG ? "image/png" : "image/jpeg",
                                data = base64Image
                            }
                        }
                    }
                }
            }
        };


        //string jsonPayload = JsonUtility.ToJson(requestData);
        string jsonPayload = JsonConvert.SerializeObject(requestData);
        Debug.Log("Send: " + jsonPayload); 

        using (UnityWebRequest www = new UnityWebRequest(apiEndpoint + "?key=" + apiKey, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");


            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Gemini API Request Error: " + www.error);
                Debug.LogError("Response Text: " + www.downloadHandler.text); 
            } else {
                Debug.Log("Gemini API Response: " + www.downloadHandler.text);

            }
        }
    }

    public string ConvertRenderTextureToBase64() {
        if (targetRenderTexture == null) {
            Debug.LogError("No Target Render Texture");
            return null;
        }

  
        RenderTexture.active = targetRenderTexture;

        // Create a new Texture2D with the same dimensions as the Render Texture
        Texture2D texture2D = new Texture2D(targetRenderTexture.width, targetRenderTexture.height, TextureFormat.RGB24, false);

        // Render Texture to Texture2D Pixel
        texture2D.ReadPixels(new Rect(0, 0, targetRenderTexture.width, targetRenderTexture.height), 0, 0);
        texture2D.Apply(); 


        RenderTexture.active = null;

        byte[] bytes;

        // Convert Texture2D to byte array based on the selected format
        if (outputFormat == ImageFormat.PNG) {
            bytes = texture2D.EncodeToPNG();
        } else // JPG
          {
            bytes = texture2D.EncodeToJPG(jpgQuality);
        }

        // Texture2D destroy
        Destroy(texture2D);

        if (bytes != null && bytes.Length > 0) {
            // Convert byte array to Base64 string
            string base64String = Convert.ToBase64String(bytes);
            Debug.Log("Encoded. Size: " + base64String.Length + "Letter");
            return base64String;
        } else {
            Debug.LogError("Encode error");
            return null;
        }
    }

    [ContextMenu("Test Send To Gemini")]
    public void TestSendToGemini() {
        StartCoroutine(SendRenderTextureToGemini());
    }

    [Serializable]
    public class GeminiImageRequest {
        public GeminiContent[] contents;
    }

    [Serializable]
    public class GeminiContent {
        public string role; 
        public GeminiPart[] parts;
    }

    [Serializable]
    public class GeminiPart {
        public string text;
        public GeminiInlineData inlineData;
    }

    [Serializable]
    public class GeminiInlineData {
        public string mimeType; 
        public string data;    
    }
}