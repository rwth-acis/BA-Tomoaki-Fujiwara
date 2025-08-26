using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.SceneDocumentation;

using i5.VirtualAgents.Examples;
using i5.VirtualAgents; // For Item Component

public class FunctionCaller : MonoBehaviour {


    public KitchenFaucetFunction kitchenFaucetFunction;

    public ModifyAgentNavigationController agentController;

    public Dictionary<string, object> CallFunction(string functionName, Dictionary<string, object> parameters) {
        switch (functionName) {
            case "getObjectInformation":
                string objectName = parameters["objectName"] as string;
                return GetObjectInfoFromScene(objectName);

            case "getAllObjectsWithDocumentation":
                return GetAllObjectsWithDocumentation();

            case "setOnKitchenFaucetColdWater":
                return kitchenFaucetFunction.Set_Faucet_Cold_Water_On();

            case "get_Faucet_Water_Status":
                return kitchenFaucetFunction.Get_Faucet_Water_Status();

            case "getSelectedObjectName":
                return GetSelectedObjectName();

            case "carryItemToUser":
                string itemToCarryName = parameters["objectName"] as string;
                return CarryItemToUser(itemToCarryName);


            case "pointVirtualAgentArmToTarget":
                string targetToPointName = parameters["objectName"] as string;
                return PointAgentArmToTarget(targetToPointName);


            default:
                return new Dictionary<string, object> {
                { "status", "error" },
                { "message", $"Function '{functionName}' not found." }
            };
        }
    }

    public Dictionary<string, object> PointAgentArmToTarget(string targetName)
    {
        GameObject targetObject = GameObject.Find(targetName);
        if (targetObject == null)
        {
            Debug.LogError($"[FunctionCaller] PointAgentArmToTarget failed: Could not find an object named '{targetName}'.");
            return new Dictionary<string, object> {
                { "status", "error" },
                { "message", $"Could not find an object named '{targetName}'." }
            };
        }

        if (agentController == null)
        {
            Debug.LogError("[FunctionCaller] PointAgentArmToTarget failed: The agent controller is not assigned in the FunctionCaller.");
            return new Dictionary<string, object> {
                { "status", "error" },
                { "message", "The agent controller is not assigned in the FunctionCaller." }
            };
        }
        Debug.Log("PointAgentArmToTarget success");
        agentController.PointVirtualAgentArmToTarget(targetObject);
        return new Dictionary<string, object> {
            { "status", "success" },
            { "message", $"Agent is now pointing at {targetName}." }
        };
    }


    


    public Dictionary<string, object> CarryItemToUser(string itemToCarryName) {
       
        GameObject itemToCarry = GameObject.Find(itemToCarryName);

        // In case the item does not exists
        if (itemToCarry == null)
        {
            Debug.LogError($"[FunctionCaller] CarryItemToUser failed: Could not find an object named '{itemToCarryName}'.");
            return new Dictionary<string, object> {
                { "status", "error" },
                { "message", $"Could not find an object named '{itemToCarryName}'." }
            };
        }

        // In case the agentController is not set
        if (agentController == null)
        {
            Debug.LogError("[FunctionCaller] CarryItemToUser failed: The agent controller is not assigned in the FunctionCaller.");
            return new Dictionary<string, object> {
                { "status", "error" },
                { "message", "The agent controller is not assigned in the FunctionCaller." }
            };
        }

        // Item Component check
        if (itemToCarry.GetComponent<Item>() == null)
        {
            Debug.LogError($"[FunctionCaller] CarryItemToUser failed: The object '{itemToCarryName}' is not a pick-up-able item because it is missing the 'Item' component.");
            return new Dictionary<string, object> {
                { "status", "error" },
                { "message", $"The object '{itemToCarryName}' is not a pick-up-able item because it is missing the 'Item' component." }
            };
        }

        Debug.Log("CarryItemToUser success");
        // If everything works
        agentController.CarryTargetItemToUser(itemToCarry);
        return new Dictionary<string, object> {
            { "status", "success" },
            { "message", $"Agent is now carrying {itemToCarryName} to the user." }
        };
    }



    public Dictionary<string, object> GetSelectedObjectName() {
        Dictionary<string, object> result = new Dictionary<string, object>();
        Debug.Log("[SceneFunctionCaller] Retrieving selected objects name.");

        // Get all objects from scene.
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        List<Dictionary<string, object>> objectInfoList = new List<Dictionary<string, object>>();

        foreach (GameObject obj in allObjects) {
            // If the Tag is not "Highlighted", then ignore
            if (LayerMask.LayerToName(obj.layer) != "Selected") {
                continue;
            }

            Dictionary<string, object> objectInfo = new Dictionary<string, object>();
            objectInfo["name"] = obj.name;

            objectInfoList.Add(objectInfo);
        }

        result["objects"] = objectInfoList;
        result["status"] = "success";

        if (objectInfoList.Count == 0) {
            result["message"] = "No objects are selected";
        }

        return result;
    }


    public Dictionary<string, object> GetSceneObjectInfo(string objectName) {
        Dictionary<string, object> result = new Dictionary<string, object>();
        Debug.Log($"[SceneFunctionCaller] Attempting to get info for object: {objectName}");

        if (objectName.ToLower() == "table") {
            result["position"] = "x:1, y:0, z:2";
            result["size"] = "width:1.5, height:0.8, depth:0.7";
            result["type"] = "wooden";
            result["isInteractive"] = true;
            result["status"] = "success";
        } else {
            result["status"] = "error";
            result["message"] = $"Object '{objectName}' not found in scene.";
        }
        return result;
    }

    public Dictionary<string, object> SetObjectColor(string objectName, string color) {
        Dictionary<string, object> result = new Dictionary<string, object>();
        Debug.Log($"[SceneFunctionCaller] Attempting to set color of {objectName} to {color}");

        if (objectName.ToLower() == "table") {
            // This is test.
            //   : GameObject table = GameObject.Find("Table");
            // if (table != null) table.GetComponent<Renderer>().material.color = GetColorFromString(color);
            result["status"] = "success";
            result["message"] = $"Table color changed to {color}.";
        } else {
            result["status"] = "error";
            result["message"] = $"Could not change color of object '{objectName}'.";
        }
        return result;
    }

    // This function send the object name to get the object information.
    public Dictionary<string, object> GetObjectInfoFromScene(string objectName) {
        Dictionary<string, object> result = new Dictionary<string, object>();
        Debug.Log($"[SceneFunctionCaller] Retrieving details for object: {objectName}");

        GameObject foundObject = GameObject.Find(objectName);
        if (foundObject != null) {
            // Component Name
            Component[] components = foundObject.GetComponents<Component>();
            List<string> componentNames = new List<string>();
            foreach (Component component in components) {
                componentNames.Add(component.GetType().Name);
            }
            result["components"] = componentNames;

            // Child Object Name
            List<string> childNames = new List<string>();
            foreach (Transform child in foundObject.transform) {
                childNames.Add(child.name);
            }
            result["children"] = childNames;

            // Parent Object Name
            Transform parentTransform = foundObject.transform.parent;
            if (parentTransform != null) {
                result["parent"] = parentTransform.name;
            } else {
                result["parent"] = "No parent object found.";
            }

            
            DocumentationObject documentation = foundObject.GetComponent<DocumentationObject>();
            if (documentation != null) {
                result["documentation"] = new Dictionary<string, string> {
                { "title", documentation.title },
                { "description", documentation.description }
                };
            } else {
                result["documentation"] = "No Documentation component found.";
            }

            // Material Information
            Renderer renderer = foundObject.GetComponent<Renderer>();
            if (renderer != null && renderer.material != null) {
                result["material"] = new Dictionary<string, string> {
                { "name", renderer.material.name },
                { "albedoColor", renderer.material.color.ToString() }//, // Albedo
                //{ "metallic", renderer.material.HasProperty("_Metallic") ? renderer.material.GetFloat("_Metallic") : 0f } // Metallic
                
                //{ "shader", renderer.material.shader.name }
            };
            } else {
                result["material"] = "No material found.";
            }

            result["status"] = "success";
        } else {
            result["status"] = "error";
            result["message"] = $"Object '{objectName}' not found in scene.";
        }

        return result;
    }

    // Send all ObjectName with Documentation information.
    public Dictionary<string, object> GetAllObjectsWithDocumentation() {
        Dictionary<string, object> result = new Dictionary<string, object>();
        Debug.Log("[SceneFunctionCaller] Retrieving all objects and their Documentation info.");

        // Get all objects from scene.
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        List<Dictionary<string, object>> objectInfoList = new List<Dictionary<string, object>>();

        foreach (GameObject obj in allObjects) {

            // Skip if object is inactive
            if (!obj.activeInHierarchy)
            {
                continue;
            }

            // If the layer is "Internal_EditorOnly_LLM_Ignore", then ignore
            if (LayerMask.LayerToName(obj.layer) == "Internal_EditorOnly_LLM_Ignore") {
                continue;
            } else if (LayerMask.LayerToName(obj.layer) == "UI") {
                continue;
            }
            else if (LayerMask.LayerToName(obj.layer) == "Default")
            {
                continue;
            }

            Dictionary<string, object> objectInfo = new Dictionary<string, object>();
            objectInfo["name"] = obj.name;

            // Documentation component
            DocumentationObject documentation = obj.GetComponent<DocumentationObject>();
            if (documentation != null) {
                objectInfo["documentation"] = new Dictionary<string, string> {
                { "title", documentation.title },
                { "description", documentation.description }
            };
            } else {
                objectInfo["documentation"] = "No Documentation component found.";
            }

            objectInfoList.Add(objectInfo);
        }

        result["objects"] = objectInfoList;
        result["status"] = "success";

        return result;
    }

    public Dictionary<string, object> PutWheelInLatheMachine()
    {
        /*
        GameObject targetObject = GameObject.Find("");
        if (targetObject == null)
        {
            Debug.LogError($"[FunctionCaller] PointAgentArmToTarget failed: Could not find an object named '{targetName}'.");
            return new Dictionary<string, object> {
                { "status", "error" },
                { "message", $"Could not find an object named '{targetName}'." }
            };
        }

        if (agentController == null)
        {
            Debug.LogError("[FunctionCaller] PointAgentArmToTarget failed: The agent controller is not assigned in the FunctionCaller.");
            return new Dictionary<string, object> {
                { "status", "error" },
                { "message", "The agent controller is not assigned in the FunctionCaller." }
            };
        }
        Debug.Log("PointAgentArmToTarget success");
        agentController.PointVirtualAgentArmToTarget(targetObject);
        return new Dictionary<string, object> {
            { "status", "success" },
            { "message", $"Agent is now pointing at {targetName}." }
        };
        */

        return new Dictionary<string, object> {
                { "status", "error" },
                { "message", "The agent controller is not assigned in the FunctionCaller." }
        };
    }


}


public class Documentation : MonoBehaviour {
    public string title;
    public string description;
}

