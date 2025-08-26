using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.Voice; 
using Meta.WitAi;
using Meta.WitAi.Configuration;
using Meta.WitAi.Data;
using Meta.WitAi.Data.Configuration;
using Meta.WitAi.Interfaces;
using Meta.WitAi.Json;
using Meta.WitAi.Requests;
using Oculus.Voice;
using UnityEngine.UI;

public class ChangeSTTConfig : MonoBehaviour {
    public AppVoiceExperience STTConfig; 

    public WitRuntimeConfiguration englishConfiguration;
    public WitRuntimeConfiguration japaneseConfiguration;
    public WitRuntimeConfiguration germanConfiguration;





    public void changeToJapanese(Toggle myToggle) {
        if (myToggle.isOn) {
            STTConfig.RuntimeConfiguration = japaneseConfiguration;
        }
    }

    public void changeToEnglish(Toggle myToggle) {
        if (myToggle.isOn) {
            STTConfig.RuntimeConfiguration = englishConfiguration;
        }
    }

    public void changeToGerman(Toggle myToggle) {
        if (myToggle.isOn) {
            STTConfig.RuntimeConfiguration = germanConfiguration;
        }
    }
}
