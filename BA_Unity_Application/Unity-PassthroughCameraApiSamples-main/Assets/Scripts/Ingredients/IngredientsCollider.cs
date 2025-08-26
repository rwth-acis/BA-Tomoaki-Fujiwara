using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsCollider : MonoBehaviour
{

    //public string knifeName = "Knife";
    public Ingredient ingredientParent; 

    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision happens");
        Debug.Log(other.gameObject.name);
        
        if (other.gameObject.GetComponent<KitchenKnife>() != null)
        {
            ingredientParent.cut();
        }


    }
}
