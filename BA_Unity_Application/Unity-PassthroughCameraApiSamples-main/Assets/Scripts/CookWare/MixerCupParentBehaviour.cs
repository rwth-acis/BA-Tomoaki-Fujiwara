using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Interaction.Grab;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction;

public class MixerCupParentBehaviour : MonoBehaviour
{

    public MixerCup mixerCup;


    public Grabbable grabbableScript;   

    public bool isCupInHolder = false;
    

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "ResetCollider")
        {
            mixerCup.ResetCupFood();
        }

        else if (other.gameObject.name == "MixerCupHolder")
        {
            grabbableScript.enabled = false;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(90, 0, 180);


            grabbableScript.enabled = true;

            isCupInHolder = true;
        }


    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.name == "MixerCupHolder")
        {
            isCupInHolder = false;
        }
    }

    public bool IsCupInHolder()
    {
        return isCupInHolder;
    }

 }
