using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookedBoiled : MonoBehaviour
{

    public Ingredient ingredient;

    // Start is called before the first frame update
    public void Cooked()
    {
        Debug.Log("CookedBoiled: Cooked");
        ingredient.cooked();
    }
}
