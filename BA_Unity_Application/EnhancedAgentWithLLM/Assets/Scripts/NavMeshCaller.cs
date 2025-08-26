using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using Meta.XR.Samples;
public class NavMeshCaller : MonoBehaviour
{

    public GameObject navMeshSurf;
    private bool meshGenerate = false;



    public GameObject setUpCameraRig;

    public GameObject playerCameraRig;


    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (!meshGenerate) {
            navMeshSurf.SetActive(true);
            meshGenerate = true;


            //This is a test
            //setUpCameraRig.SetActive(false);
            //playerCameraRig.SetActive(true);
        }
    }
}
