using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLeakFaucet : MonoBehaviour
{


    void OnParticleCollision(GameObject other)
    {
        if (other.name == "Pot_01")
        {
            Pot pot = other.GetComponent<Pot>();
            if (pot != null)
            {
                pot.FillWithWater(); 
            }
        }
    }
}
