using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{


    //public GameObject self;
    public GameObject vrScreenShotCamera;
    public GameObject arScreenShotCamera;

    public GameObject changeToARButton;
    public GameObject changeToVRButton;

    public GameObject takeShotButton;
    public GameObject retakeButton;
    public GameObject accectButton;

    public GameObject preview;

    public enum CameraMode
    {
        VRMode,
        ARMode
    }

    public CameraMode cameraMode = CameraMode.VRMode;

    public void SetARMode()
    {
        //Debug.Log("Setting AR Mode");

        changeToARButton.SetActive(false);
        changeToVRButton.SetActive(true);

        cameraMode = CameraMode.ARMode;
        
        PrepareScreenShot();
    }

    public void SetVRMode()
    {
        //Debug.Log("Setting VR Mode");

        changeToARButton.SetActive(true);
        changeToVRButton.SetActive(false);
        cameraMode = CameraMode.VRMode;
        PrepareScreenShot();
    }

    public void PrepareScreenShot() {
        //self.SetActive(true);

        if (cameraMode == CameraMode.VRMode) {
            arScreenShotCamera.SetActive(false);
            vrScreenShotCamera.SetActive(true);
            
            changeToARButton.SetActive(true);
            changeToVRButton.SetActive(false);
        } else if (cameraMode == CameraMode.ARMode) {
            vrScreenShotCamera.SetActive(false);
            arScreenShotCamera.SetActive(true);
            changeToARButton.SetActive(false);
            changeToVRButton.SetActive(true);
        }


        takeShotButton.SetActive(true);
        retakeButton.SetActive(false);
        accectButton.SetActive(false);
    }

    public void CaptureScreenShot()
    {

        vrScreenShotCamera.SetActive(false);
        arScreenShotCamera.SetActive(false);
        takeShotButton.SetActive(false);
        retakeButton.SetActive(true);
        accectButton.SetActive(true);
        preview.SetActive(true);

    }



    public void OnEnable() {
        PrepareScreenShot();
    }

    public void OnDisable() {
        vrScreenShotCamera.SetActive(false);
        arScreenShotCamera.SetActive(false);
        takeShotButton.SetActive(false);
        retakeButton.SetActive(false);
        accectButton.SetActive(false);
    }

}


