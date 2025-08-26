using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{


    public PodLiquid waterLiquid;

    public float waterFillRate = 0f;


    public void Start()
    {
        if (waterLiquid != null)
        {
            waterLiquid.SetFillingRate(waterFillRate);
        }
    }

    public void FillWithWater()
    {
        if (waterLiquid != null)
        {
            waterFillRate= waterFillRate + 0.01f; // Increase the fill rate by 0.1 each time water is added
            waterLiquid.SetFillingRate(waterFillRate);

            if (waterFillRate >= 1f)
            {
                waterFillRate = 1f; // Cap the fill rate at 1
            }
        }
    }
}
