using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlPourController : MonoBehaviour
{

    public Transform floorPlaneColliderTransform;
    public Transform emitterEggTransform;
    public Transform emitterButterTransform;

    //public GameObject pourWaterParicleObject;
    public ParticleSystem beatedEggParticles;
    public ParticleSystem meltedButterParticles;
    public float emissionAngleThreshold = 45f; // Example: Emit if the angle with the world's downward direction is less than 45 degrees

    void Start()
    {
        if (beatedEggParticles != null)
        {
            beatedEggParticles.Stop();
        }

        if (meltedButterParticles != null)
        {
            meltedButterParticles.Stop();
        }
        //pourWaterParicleObject.SetActive(false);
        //Debug.Log("BowlPourController started.");
        //Debug.Log(transform.up);
        //Debug.Log(Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the angle between the bowl's up direction and the world's up direction
        float angle = Vector3.Angle(transform.forward, Vector3.up);
        //Debug.Log("Bowl Transform Up: "+ transform.forward);
        if (angle > 30f) 
        {
            //pourWaterParicleObject.SetActive(true);
            if (!beatedEggParticles.isPlaying)
            {
                beatedEggParticles.Play();
            }

            if (!meltedButterParticles.isPlaying)
            {
                meltedButterParticles.Play();
            }
        }
        else
        {
            //pourWaterParicleObject.SetActive(false);
            if (beatedEggParticles.isPlaying)
            {
                beatedEggParticles.Stop();
            }

            if (meltedButterParticles.isPlaying)
            {
                meltedButterParticles.Stop();
            }
        }

        // Setup for particle direction
        //Vector3 desiredUp = floorPlaneColliderTransform.up;
        //emitterTransform.up = desiredUp;

        Vector3 desiredUp = floorPlaneColliderTransform.forward;
        emitterEggTransform.forward = desiredUp;

        emitterButterTransform.forward = desiredUp;
    }
}
