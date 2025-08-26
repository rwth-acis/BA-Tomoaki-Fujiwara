using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{


    //public GameObject self;
    public GameObject screenShotCamera;
    public GameObject takeShotButton;
    public GameObject retakeButton;
    public GameObject accectButton;

    public GameObject preview;


    public void PrepareScreenShot() {
        //self.SetActive(true);
        screenShotCamera.SetActive(true);
        takeShotButton.SetActive(true);
        retakeButton.SetActive(false);
        accectButton.SetActive(false);
    }

    public void CaptureScreenShot()
    {
        
        screenShotCamera.SetActive(false);
        takeShotButton.SetActive(false);
        retakeButton.SetActive(true);
        accectButton.SetActive(true);
        preview.SetActive(true);

    }



    public void OnEnable() {
        PrepareScreenShot();
    }

    public void OnDisable() {
        screenShotCamera.SetActive(false);
        takeShotButton.SetActive(false);
        retakeButton.SetActive(false);
        accectButton.SetActive(false);
    }

}


