using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerCupPourController : MonoBehaviour
{
    public Transform floorPlaneColliderTransform;
    public Transform emitterMixedFlourTransform;

    //public GameObject pourWaterParicleObject;
    public ParticleSystem mixedFlourParticles;
    public float emissionAngleThreshold = 45f; // Example: Emit if the angle with the world's downward direction is less than 45 degrees

    void Start()
    {
        if (mixedFlourParticles != null)
        {
            mixedFlourParticles.Stop();
        }

        //pourWaterParicleObject.SetActive(false);
        //Debug.Log("BowlPourController started.");
        //Debug.Log(transform.up);
        //Debug.Log(Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {

        // Vector3 cupUpDirection = transform.up;
        // Vector3 cupUpDirection = -transform.up;
        // Vector3 cupUpDirection = transform.forward;
        Vector3 cupUpDirection = -transform.forward; 
        // Vector3 cupUpDirection = transform.right;
        // Vector3 cupUpDirection = -transform.right;



      
        float angle = Vector3.Angle(cupUpDirection, Vector3.up);

        if (angle > 30f)
        {
            //pourWaterParicleObject.SetActive(true);
            if (!mixedFlourParticles.isPlaying)
            {
                mixedFlourParticles.Play();
            }
        }
        else
        {
            //pourWaterParicleObject.SetActive(false);
            if (mixedFlourParticles.isPlaying)
            {
                mixedFlourParticles.Stop();
            }

        }

        // Setup for particle direction
        //Vector3 desiredUp = floorPlaneColliderTransform.up;
        //emitterTransform.up = desiredUp;

        Vector3 desiredUp = floorPlaneColliderTransform.forward;
        emitterMixedFlourTransform.forward = desiredUp;

    }


}
