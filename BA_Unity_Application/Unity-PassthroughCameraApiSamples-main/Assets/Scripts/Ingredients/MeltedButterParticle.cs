using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltedButterParticle : MonoBehaviour
{

    [SerializeField] private GameObject mixerCupPourCollider;
    [SerializeField] private GameObject mixerCupPourparent;
    MixerCup mixerCup;
    void Start()
    {
        if (mixerCupPourCollider == null)
        {
            Debug.LogError("Mixer Cup Pour Collider is not assigned in the FlourParicle script.");
        }
        if (mixerCupPourparent == null)
        {
            Debug.LogError("Mixer Cup Pour Parent is not assigned in the FlourParicle script.");
        }
        else
        {
            mixerCup = mixerCupPourparent.GetComponent<MixerCup>();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        //Debug.Log("Melted Butter is hitting something");
        //Debug.Log(other.name);
        if (other == mixerCupPourCollider)
        {
            //MixerCup mixerCup = mixerCupPourparent.GetComponent<MixerCup>();
            if (mixerCup != null)
            {
                mixerCup.AddMeltedButterLiquid();
            }
        }
    }
}
