using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixedFlourParticle : MonoBehaviour
{
    [SerializeField] private GameObject bakePlatePourInCollider;
    [SerializeField] private GameObject bakePlatePourparent;
    public BakePlate bakeplate;

    public MixerCup mixerCup;

    void Start()
    {
        if (bakePlatePourInCollider == null)
        {
            Debug.LogError("Mixer Cup Pour Collider is not assigned in the FlourParicle script.");
        }
        if (bakePlatePourparent == null)
        {
            Debug.LogError("Mixer Cup Pour Parent is not assigned in the FlourParicle script.");
        }
        else
        {
            bakeplate = bakePlatePourparent.GetComponent<BakePlate>();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        //Debug.Log("MixedFlour is hitting something");
        //Debug.Log(other.name);
        if (other == bakePlatePourInCollider)
        {

            //MixerCup mixerCup = mixerCupPourparent.GetComponent<MixerCup>();
            if (bakeplate != null)
            {
                bakeplate.PourFlour();
                mixerCup.ResetCupFood();
            }
        }
    }
}
