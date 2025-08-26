using System.Collections;
using System.Collections.Generic;
//using TMPro.Examples;
//using TMPro.Examples;
//using TMPro.Examples;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Oculus.Interaction;

public class FaucetWaterManager : MonoBehaviour {
    // Start is called before the first frame update
    public CustomOneGrabRotateTransformer coldHandle;
    public CustomOneGrabRotateTransformer hotHandle;

    public ParticleSystem waterLeakParticleSystem;
    public ParticleSystem waterSplashParticleSystem;
    public ParticleSystem waterDropsParticleSystem;

    public GameObject waterLeakObject;

    //public TextMeshProUGUI uiColdText;

    //public TextMeshProUGUI uiHotText;

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

        //uiColdText.text = coldHandle.CurrentConstrainedAngle.ToString();

        //uiHotText.text = hotHandle.CurrentConstrainedAngle.ToString();


        waterLeakParticleSystem.startSize = 1 * ((Mathf.Abs(coldHandle.CurrentConstrainedAngle) / 90 + Mathf.Abs(hotHandle.CurrentConstrainedAngle) / 90) / 2);
        waterSplashParticleSystem.startSize = 0.4f * ((Mathf.Abs(coldHandle.CurrentConstrainedAngle) / 90 + Mathf.Abs(hotHandle.CurrentConstrainedAngle) / 90) / 2);
        waterDropsParticleSystem.startSize = 0.01f * ((Mathf.Abs(coldHandle.CurrentConstrainedAngle) / 90 + Mathf.Abs(hotHandle.CurrentConstrainedAngle) / 90) / 2);
        //waterParticleSystem.startSize = 0;
        //waterParticleSystem.startSize = 0;

        if (waterLeakParticleSystem.startSize == 0) {
            waterLeakObject.SetActive(false);
        } else {
            waterLeakObject.SetActive(true);
        }
    }

    public void Set_Faucet_Cold_Water_On() {
        coldHandle.SetRelativeAngle(-90);
    }

    public void Set_Faucet_Cold_Water_Off() {
        coldHandle.SetRelativeAngle(0);
    }

    public void Set_Faucet_Hot_Water_On() {
        hotHandle.SetRelativeAngle(90);
    }

    public void Set_Faucet_Hot_Water_Off() {
        hotHandle.SetRelativeAngle(0);
    }

    public void Set_Faucet_Water_Off() {
        Set_Faucet_Hot_Water_Off();
        Set_Faucet_Cold_Water_Off();
    }

    public float Get_Faucet_Cold_Water_Status() {
        return Mathf.Abs(coldHandle.CurrentConstrainedAngle);
    }

    public float Get_Faucet_Hot_Water_Status() {
        return Mathf.Abs(hotHandle.CurrentConstrainedAngle);
    }

}
