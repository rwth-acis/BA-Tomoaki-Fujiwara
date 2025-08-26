using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KitchenFaucetFunction : MonoBehaviour {


    public FaucetWaterManager faucetWaterManager;

    public Dictionary<string, object> Set_Faucet_Cold_Water_On() {
        Dictionary<string, object> result = new Dictionary<string, object>();
        Debug.Log($"[SceneFunctionCaller] Attempting turn kitchen cold water handle on");
        faucetWaterManager.Set_Faucet_Cold_Water_On();

        result["status"] = "success";
        return result;

    }
    public Dictionary<string, object> Set_Faucet_Cold_Water_Off() {
        Dictionary<string, object> result = new Dictionary<string, object>();
        Debug.Log($"[SceneFunctionCaller] Attempting turn kitchen cold water handle on");
        faucetWaterManager.Set_Faucet_Cold_Water_Off();

        result["status"] = "success";
        return result;

    }
    public Dictionary<string, object> Set_Faucet_Hot_Water_On() {
        Dictionary<string, object> result = new Dictionary<string, object>();
        Debug.Log($"[SceneFunctionCaller] Attempting turn kitchen cold water handle on");
        faucetWaterManager.Set_Faucet_Hot_Water_On();

        result["status"] = "success";
        return result;

    }
    public Dictionary<string, object> Set_Faucet_Hot_Water_Off() {
        Dictionary<string, object> result = new Dictionary<string, object>();
        Debug.Log($"[SceneFunctionCaller] Attempting turn kitchen cold water handle on");
        faucetWaterManager.Set_Faucet_Hot_Water_Off();

        result["status"] = "success";
        return result;

    }
    public Dictionary<string, object> Set_Faucet_Water_Off() {
        Dictionary<string, object> result = new Dictionary<string, object>();
        Debug.Log($"[SceneFunctionCaller] Attempting turn kitchen cold water handle on");
        faucetWaterManager.Set_Faucet_Water_Off();

        result["status"] = "success";
        return result;

    }

    public Dictionary<string, object> Get_Faucet_Water_Status() {
        Dictionary<string, object> result = new Dictionary<string, object>();
        Debug.Log($"[SceneFunctionCaller] Attempting get status of water temperatur from faucet.");

        float coldWaterStatus = faucetWaterManager.Get_Faucet_Cold_Water_Status();
        float hotWaterStatus = faucetWaterManager.Get_Faucet_Hot_Water_Status();

        if ((coldWaterStatus == 0) && (hotWaterStatus == 0)) {

            result["FaucetWaterStatus"] = "no water";

        } else if ((coldWaterStatus != 0) && (hotWaterStatus == 0)) {

            result["FaucetWaterStatus"] = "cold water";

        } else if ((coldWaterStatus == 0) && (hotWaterStatus != 0)) {

            result["FaucetWaterStatus"] = "hot water";
        } else if ((coldWaterStatus != 0) && (hotWaterStatus != 0)) {

            result["FaucetWaterStatus"] = "warm water";
        } else {
            result["FaucetWaterStatus"] = "unknown";
        }


        result["status"] = "success";
        return result;

    }


}
