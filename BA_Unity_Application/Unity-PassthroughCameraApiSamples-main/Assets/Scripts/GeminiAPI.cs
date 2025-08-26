using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
//using TMPro.Examples;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using static RenderTextureToBase64Converter;
public class GeminiAPI : MonoBehaviour {
    [SerializeField]
    public TMPro.TextMeshProUGUI uiText;

    [SerializeField]
    public FunctionCaller functionCaller;


    public string apiKey = "AIzaSyBX_xWhb5__6kQAAZENsigtFaup9LKoOlE";
    public string apiEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

    // Define Scene Contexts
    public enum SceneContext { Kitchen, Factory }
    [Tooltip("Select the current scene context to provide the appropriate functions to the LLM.")]
    public SceneContext currentScene = SceneContext.Kitchen;


    public enum ImageFormat { PNG, JPG }
    public ImageFormat outputFormat = ImageFormat.PNG;

    [Range(0, 100)]
    public int jpgQuality = 75;

    public static GeminiAPI unityAndGeminiInstance;

    private List<Content> chatHistory = new List<Content>(); // Stores conversation history

    [SerializeField]
    [TextArea(3, 15)]
    private string initConversationPrompt = "When a user asks about an object and doesn't provide a specific name, first attempt to use getAllObjectsWithDocumentation to identify relevant objects. Then, if more detail is needed, use getObjectInformation with the identified object names." +
        "There is a faucet in this scene. Can you tell me how many potato does it has?";

    // Define the tools (functions) available to the LLM
    private Tool[] infoFunctionCallingLists;

    private void Awake() {
        unityAndGeminiInstance = this;
        InitializeTools(); // Set up your functions here
    }

    private void Start() {
        // Start an initial conversation with the LLM
        //string initConversationPrompt = "When a user asks about an object and doesn't provide a specific name, first attempt to use getAllObjectsWithDocumentation to identify relevant objects. Then, if more detail is needed, use getObjectInformation with the identified object names." +
        //    "There is a faucet in this scene. Can you tell me how many handle does it has?";
        //string initConversationPrompt = "When a user asks about an object and doesn't provide a specific name, first attempt to use getAllObjectsWithDocumentation to identify relevant objects. Then, if more detail is needed, use getObjectInformation with the identified object names." +
        //    "There is a faucet in this scene. Can you tell me the information from faucet_handle_cold and faucet_handle_hot and faucet_handle_normal? Can you tell me what parent object does faucet has? Can you tell me what the water status from faucet is?";
        //string initConversationPrompt = "When a user asks about an object and doesn't provide a specific name, first attempt to use getAllObjectsWithDocumentation to identify relevant objects. Then, if more detail is needed, use getObjectInformation with the identified object names.";
        //"There is a faucet in this scene. Can you tell me the information from faucet_handle_cold and faucet_handle_hot and faucet_handle_normal? Can you tell me what parent object does faucet has? Can you tell me what the water status from faucet is?";
        //string initConversationPrompt = "When a user asks about an object and doesn't provide a specific name, first attempt to use getAllObjectsWithDocumentation to identify relevant objects. Then, if more detail is needed, use getObjectInformation with the identified object names." +
        //    "There is a faucet in this scene. Can you tell me the information from faucet_handle_cold and faucet_handle_hot and faucet_handle_normal? Can you tell me what parent object does faucet has?";

        //string initConversationPrompt = "Hello!";
        //string initConversationPrompt = "Can you tell me the informaton of highlighted objects? What documentation does it has?";

        //string initConversationPrompt = "Make the 3D figure very happy";
        //string initConversationPrompt = "";

        //string initConversationPrompt = "When a user asks about an object and doesn't provide a specific name, first attempt to use getAllObjectsWithDocumentation to identify relevant objects. Then, if more detail is needed, use getObjectInformation with the identified object names.";

        //UIMessageHandler.instance.GenerateUserTextBubble(initConversationPrompt);

        SendChatRequest(initConversationPrompt);
    }

    /// <summary>
    /// Define the available callback functions for the LLM based on the current scene context.
    /// </summary>
    private void InitializeTools()
    {
        var functionDeclarations = new List<FunctionDeclaration>();

        // --- Add functions common to all scenes ---
        functionDeclarations.Add(new FunctionDeclaration
        {
            name = "setObjectColor",
            description = "Sets the color of an object in the 3D scene.",
            parameters = new Parameters
            {
                type = "object",
                properties = new Dictionary<string, Property>
                {
                    { "objectName", new Property { type = "string", description = "The name of the object to change color." } },
                    { "color", new Property { type = "string", description = "The color to set (e.g., 'red', 'blue', 'green')." } }
                },
                required = new string[] { "objectName", "color" }
            }
        });
        functionDeclarations.Add(new FunctionDeclaration
        {
            name = "getObjectInformation",
            description = "Get the object information through the object name. Such as parameter, child objetc etc.",
            parameters = new Parameters
            {
                type = "object",
                properties = new Dictionary<string, Property>
                {
                    { "objectName", new Property { type = "string", description = "The name of the object." } }
                },
                required = new string[] { "objectName" }
            }
        });
        functionDeclarations.Add(new FunctionDeclaration
        {
            name = "getAllObjectsWithDocumentation",
            description = "Retrieves a list of all objects present in the scene, along with their names and any available documentation. This function is useful for discovering objects when their exact names are unknown, or for getting an overview of the scene's contents. You can then use this information to query specific objects with getObjectInformation.",
            parameters = new Parameters { type = "object" }
        });
        functionDeclarations.Add(new FunctionDeclaration
        {
            name = "getSelectedObjectName",
            description = "The user could give a reference word like 'this' or 'that' or 'these' and select the objects. This will get the name list of all objects, that user selected. If the name list is empty, that means no objects are selected.",
            parameters = new Parameters { type = "object" }
        });

        functionDeclarations.Add(new FunctionDeclaration
        {
            name = "pointVirtualAgentArmToTarget",
            description = "Commands the agent to first turn towards a target object and then point its arm at it for a few seconds.",
            parameters = new Parameters
            {
                type = "object",
                properties = new Dictionary<string, Property>
                        {
                            { "objectName", new Property { type = "string", description = "The name of the object for the agent to point at." } }
                        },
                required = new string[] { "objectName" }
            }
        });

        // --- Add scene-specific functions ---
        switch (currentScene)
        {
            case SceneContext.Kitchen:
                functionDeclarations.Add(new FunctionDeclaration
                {
                    name = "get_Faucet_Water_Status",
                    description = "This will get the water status from kitchen faucet. Faucet has cold water handle and hot water handle. The water status is represented by combination of rotations from handles.",
                    parameters = new Parameters { type = "object" }
                });

                functionDeclarations.Add(new FunctionDeclaration
                {
                    name = "carryItemToUser",
                    description = "Commands the agent to pick up a specified item and carry it to the user.",
                    parameters = new Parameters
                    {
                        type = "object",
                        properties = new Dictionary<string, Property>
                        {
                            { "objectName", new Property { type = "string", description = "The name of the item for the agent to pick up and carry." } }
                        },
                        required = new string[] { "objectName" }
                    }
                });
                // Add other kitchen-specific functions here
                break;

            case SceneContext.Factory:
                functionDeclarations.Add(new FunctionDeclaration
                {
                    name = "putWheelInLatheMachine",
                    description = "Commands the robot arm pick up the wheel from belt convyor and carry it to the Lathe machine. If this action is success, the lathe machine will process the wheel.",
                    parameters = new Parameters { type = "object" }
                });

                functionDeclarations.Add(new FunctionDeclaration
                {
                    name = "putWheelInFlipTable",
                    description = "Commands the robot arm pick up the wheel from Lathe machine and carry it to the FlipTable. If this action is success, the FlipTable machine will turn the wheel upside down in the machine.",
                    parameters = new Parameters { type = "object" }
                });

                functionDeclarations.Add(new FunctionDeclaration
                {
                    name = "putWheelInMillingMachine",
                    description = "Commands the robot arm pick up the wheel from FlipTable machine and carry it to the Milling machine. If this action is success, the Milling machine will process the wheel.",
                    parameters = new Parameters { type = "object" }
                });

                functionDeclarations.Add(new FunctionDeclaration
                {
                    name = "putWheelOnConvyorMachine",
                    description = "Commands the robot arm pick up the wheel from Milling machine and carry it to the belt conyor. If this action is success, the convyor will carry the wheel.",
                    parameters = new Parameters { type = "object" }
                });

                functionDeclarations.Add(new FunctionDeclaration
                {
                    name = "getRobotArmStatus",
                    description = "Get the current status of the robot arm and any errors or alerts.",
                    parameters = new Parameters { type = "object" }
                });

                functionDeclarations.Add(new FunctionDeclaration
                {
                    name = "scanMachineError",
                    description = "This will scann all machine and get any errors or alerts from machine.",
                    parameters = new Parameters { type = "object" }
                });

                // Add other factory-specific functions here
                break;
        }

        // Finalize the tool list
        infoFunctionCallingLists = new Tool[]
        {
            new Tool
            {
                functionDeclarations = functionDeclarations.ToArray()
            }
        };
    }

    /// <summary>
    /// Sends a user message to the Gemini LLM.
    /// </summary>
    /// <param name="userInput">The user's message.</param>
    public void SendChatRequest(string userInput) {
        StartCoroutine(SendRequestToGemini(userInput));
    }

    /// <summary>
    /// Coroutine to send a chat request to the Gemini LLM.
    /// </summary>
    private IEnumerator SendRequestToGemini(string userInput) {
        string url = $"{apiEndpoint}?key={apiKey}";


        //Check if user took an image
        if (UIMessageHandler.instance.isPictureTaken()) {
            // Convert the RenderTexture to Base64
            string base64Image = ConvertRenderTextureToBase64();
            if (string.IsNullOrEmpty(base64Image)) {
                Debug.LogError("Error converting RenderTexture to Base64.");
                yield break;
            }
            // Create an inline data part for the image

            Content userContent = new Content {
                role = "user",
                parts = new Part[] {
                new Part {
                    text = userInput,
                    //inlineData = null, 
                    //fileData = null,   
                    //functionCall = null,
                    //functionResponse = null 
                },

                // Image
                new Part{
                    text = null,
                    inlineData = new InlineData
                    {
                        mimeType = outputFormat == ImageFormat.PNG ? "image/png" : "image/jpeg",
                        data = base64Image
                    }
                }

            }
            };


            chatHistory.Add(userContent);

        } else {

            // Add the user's message to chat history
            Content userContent = new Content {
                role = "user",
                parts = new Part[] {
                new Part {
                    text = userInput,
                    //inlineData = null, 
                    //fileData = null,   
                    //functionCall = null,
                    //functionResponse = null 
                }
            }
            };
            // Add the user content to chat history

            chatHistory.Add(userContent);
        }






        // Prepare the request body for the LLM
        ChatRequest chatRequest = new ChatRequest {
            contents = chatHistory.ToArray(), // Contains user, LLM and function responses
            tools = infoFunctionCallingLists // Always send available functions with user messages
        };

        // Convert the chatRequest object to a JSON string
        //string jsonData = JsonUtility.ToJson(chatRequest);  // Need to use other method, because we use Newtonsoft.Json
        string jsonData = JsonConvert.SerializeObject(chatRequest);
        Debug.Log($"Sending to Gemini: {jsonData}");

        // Convert the JSON string to a byte array
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest www = new UnityWebRequest(url, "POST")) {
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.LogError($"Gemini Request Error: {www.error}");
                Debug.LogError($"Gemini Response: {www.downloadHandler.text}");
            } else {
                Debug.Log($"Gemini Response Received: {www.downloadHandler.text}");
                //Response geminiResponse = JsonUtility.FromJson<Response>(www.downloadHandler.text);
                Response geminiResponse = JsonConvert.DeserializeObject<Response>(www.downloadHandler.text);

                if (geminiResponse.candidates != null && geminiResponse.candidates.Length > 0 && geminiResponse.candidates[0].content != null) {
                    Content modelResponseContent = geminiResponse.candidates[0].content;
                    chatHistory.Add(modelResponseContent); // Add model's response to history

                    ProcessModelResponse(modelResponseContent);
                } else {
                    Debug.Log("No valid candidates or content found in Gemini response.");
                }
            }
        }
    }

    /// <summary>
    /// Processes the content received from the LLM.
    /// Responce of LLM be like:
    ///     - No function calling:
    ///     {"role": "model",
    ///     "parts": [
    ///    {"text": "Text Responce."} ]
    ///}
    ///
    ///     -With function calling:
    ///     {"role": "model",
    ///     "parts": [
    ///    {"functionCall": {
    ///        "name": "setObjectColor",  // Name of the function to call
    ///        "args": {
    ///          "objectName": "table",
    ///          "color": "red"}} }      ]}
    /// 
    /// </summary>
    /// 
    /// <param name="content">The content from the LLM.</param>
    private void ProcessModelResponse(Content content) {
        if (content.parts != null) {

            List<FunctionCall> functionCallsToExecute = new List<FunctionCall>();

            foreach (Part part in content.parts) {
                if (!string.IsNullOrEmpty(part.text)) {
                    // If the LLM responds with text, display it to the user
                    Debug.Log($"LLM Text: {part.text}");
                    UIMessageHandler.instance.GenerateLLMTextBubble(part.text);
                } else if (part.functionCall != null) {
                    // If the LLM wants to call a function
                    // Debug.Log($"LLM wants to call function: {part.functionCall.name} with args: {JsonUtility.ToJson(part.functionCall.args)}"); // Need to use other method, because we use Newtonsoft.Json
                    Debug.Log($"LLM wants to call function: {part.functionCall.name} with args: {JsonConvert.SerializeObject(part.functionCall.args)}");
                    functionCallsToExecute.Add(part.functionCall);
                }
            }

            if (functionCallsToExecute.Count > 0) {
                StartCoroutine(ExecuteAndSendAllFunctionResponses(functionCallsToExecute));
            } else {
                Debug.Log("No function calls found in content parts.");
            }

        }
    }

    private IEnumerator ExecuteAndSendAllFunctionResponses(List<FunctionCall> functionCalls) {
        List<FunctionResponse> responsesToGemini = new List<FunctionResponse>();

        foreach (FunctionCall functionCall in functionCalls) {
            Dictionary<string, object> functionResult = new Dictionary<string, object>();

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (var arg in functionCall.args) {
                parameters[arg.Key] = arg.Value;
            }

            functionResult = functionCaller.CallFunction(functionCall.name, parameters);

            responsesToGemini.Add(new FunctionResponse {
                name = functionCall.name,
                response = functionResult
            });

            yield return null;

        }

        yield return SendFunctionResponsesToGemini(responsesToGemini);
    }

    /*
    private IEnumerator ExecuteAndSendAllFunctionResponses(List<FunctionCall> functionCalls) {
        List<FunctionResponse> responsesToGemini = new List<FunctionResponse>();

        foreach (FunctionCall functionCall in functionCalls) {
            Dictionary<string, object> functionResult = new Dictionary<string, object>();

            
            switch (functionCall.name) {
                case "setObjectColor":
                    if (functionCall.args.TryGetValue("objectName", out string colorObjName) && colorObjName is string cObjName &&
                        functionCall.args.TryGetValue("color", out string colorVal) && colorVal is string colorString) {
                        functionCaller.SetObjectColor(cObjName, colorString);
                        //functionResult["status"] = "success"; //
                    } else {
                        functionResult["status"] = "error";
                        functionResult["message"] = "Missing objectName or color argument.";
                    }
                    break;

                case "getObjectInformation":
                    if (functionCall.args.TryGetValue("objectName", out string objName) && objName is string nameString) {
                        functionResult = functionCaller.GetObjectInfoFromScene(nameString);
                    } else {
                        functionResult["status"] = "error";
                        functionResult["message"] = "Missing objectName argument.";
                    }
                    break;

                case "getAllObjectsWithDocumentation":
                    functionResult = functionCaller.GetAllObjectsWithDocumentation();
                    break;

                default:
                    Debug.LogWarning($"Unknown function call: {functionCall.name}");
                    functionResult["status"] = "error";
                    functionResult["message"] = $"Unknown function: {functionCall.name}";
                    break;
            }


            responsesToGemini.Add(new FunctionResponse {
                name = functionCall.name,
                response = functionResult
            });

            yield return null; 
            
        }

        yield return SendFunctionResponsesToGemini(responsesToGemini);
    }

    */

    private IEnumerator SendFunctionResponsesToGemini(List<FunctionResponse> functionResponses) {
        string url = $"{apiEndpoint}?key={apiKey}";


        List<Part> parts = new List<Part>();
        foreach (FunctionResponse response in functionResponses) {
            parts.Add(new Part {
                functionResponse = response,
                text = null,
                inlineData = null,
                fileData = null,
                functionCall = null
            });
        }


        Content functionResponseContent = new Content {
            role = "function",
            parts = parts.ToArray()
        };
        chatHistory.Add(functionResponseContent);


        ChatRequest chatRequest = new ChatRequest {
            contents = chatHistory.ToArray(),
            tools = infoFunctionCallingLists
        };

        string jsonData = JsonConvert.SerializeObject(chatRequest);
        Debug.Log($"Sending Function Responses to Gemini: {jsonData}");

        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest www = new UnityWebRequest(url, "POST")) {
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.LogError($"Function Response Error: {www.error}");
                Debug.LogError($"Function Response Detail: {www.downloadHandler.text}");
            } else {
                Debug.Log($"Function Response Sent. Model's follow-up: {www.downloadHandler.text}");
                Response geminiResponse = JsonConvert.DeserializeObject<Response>(www.downloadHandler.text);

                if (geminiResponse.candidates != null && geminiResponse.candidates.Length > 0 && geminiResponse.candidates[0].content != null) {
                    Content modelFollowUpContent = geminiResponse.candidates[0].content;
                    chatHistory.Add(modelFollowUpContent);// Add model's follow-up to history

                    ProcessModelResponse(modelFollowUpContent); // Process the model's follow-up (text or another function call)
                } else {
                    Debug.Log("No valid candidates or content in model's follow-up response.");
                }
            }
        }
    }



    public void PNGToBase64Converter() {
        if (!UIMessageHandler.instance.isPictureTaken()) {
            return;
        }



    }

    public string ConvertRenderTextureToBase64() {


        RenderTexture renderImage = UIMessageHandler.instance.GetRenderTextureFromUserMessage();

        // Create a new Texture2D with the same dimensions as the Render Texture
        Texture2D texture2D = new Texture2D(renderImage.width, renderImage.height, TextureFormat.RGB24, false);

        // Render Texture to Texture2D Pixel
        texture2D.ReadPixels(new Rect(0, 0, renderImage.width, renderImage.height), 0, 0);
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




}

// Data structures for JSON serialization/deserialization



// Class for deserializing the response from the Gemini API
public class Response {
    public Candidate[] candidates;
}


// Structure of the request body to send to the Gemini API
public class ChatRequest {
    public Content[] contents; // List of contents (messages, images, result of functions)
    public Tool[] tools; // List of tools (function declarations)
}





// Represents a candidate (response) from the model
[System.Serializable]
public class Candidate {
    public Content content;
}

// Represents the content of a conversation (user or model utterance)
[System.Serializable]
public class Content {
    public string role; // "user", "model", or "function"
    public Part[] parts;
}

// Represents a part of the conversation content (text, image, function call, etc.)
[System.Serializable]
public class Part {
    public string text;
    public InlineData inlineData; // Represents inline data (e.g., images)
    public FileData fileData; // Represents file data (File API)
    public FunctionCall functionCall; // Represents a function call
    public FunctionResponse functionResponse; // Represents a user-defined function response
}

// Represents inline data (e.g., images)
[System.Serializable]
public class InlineData {
    public string mimeType;
    public string data;
}

// Represents file data (File API)
[System.Serializable]
public class FileData {
    public string fileUri;
    public string mimeType;
}

// Represents a function call
[System.Serializable]
public class FunctionCall {
    public string name;
    public Dictionary<string, string> args; // Use object to handle various types
}

// Represents a function response
[System.Serializable]
public class FunctionResponse {
    public string name;
    public Dictionary<string, object> response; // Use object to handle various types
}

// Top-level class for tools (FunctionDeclaration)
[System.Serializable]
public class Tool {
    public FunctionDeclaration[] functionDeclarations;
}

// Class representing a function declaration
[System.Serializable]
public class FunctionDeclaration {
    public string name;
    public string description;
    public Parameters parameters;
}

// Class representing function arguments (parameters)
[System.Serializable]
public class Parameters {
    public string type; // "object"
    public Dictionary<string, Property> properties;
    public string[] required;
}

// Class representing an argument property
[System.Serializable]
public class Property {
    public string type;
    public string description;
    public Items items; // Required if type is "array"
}

// Class representing the type of array elements
[System.Serializable]
public class Items {
    public string type;
}

