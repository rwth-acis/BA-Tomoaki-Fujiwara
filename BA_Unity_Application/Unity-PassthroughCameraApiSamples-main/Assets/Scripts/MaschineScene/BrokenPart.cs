using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BrokenPart : MonoBehaviour
{

    //public GameObject visualCollider;
    //public GameObject canvas;
    public GameObject brokeParticle;

    public Image fixingBarImage;

    public bool isBroken = false;

    private float currentFixedAmount = 0.0f;

    public bool isSpannerInPlace = false;

    public BoxCollider boxCollider;


    void Update() {
        
        if (isSpannerInPlace) {
            FixingJoint();
        
        }
        
    }

    public void SpannerInPlace() { 
        isSpannerInPlace=true;
    }

    public void SpannerOutOfPlace() { 
        isSpannerInPlace=false;
    }


    public void BrokeTheJoint()
    {
        //gameObject.SetActive(true);
        //visualCollider.SetActive(true);
        //canvas.SetActive(true);
        brokeParticle.SetActive(true);
        isBroken = true;
        currentFixedAmount = 0.0f;

        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }

    }

    public void VisibleTheCollider()
    {
        gameObject.SetActive(true);
    }

    public void FixedTheJoint()
    {
        //visualCollider.SetActive(false);
        //canvas.SetActive(false);
        brokeParticle.SetActive(false);
        gameObject.SetActive(false);
        isBroken = false;

        if (boxCollider != null) { 
            boxCollider.enabled = true;
        }

    }

    public void FixingJoint()
    {
        currentFixedAmount = currentFixedAmount + 0.01f;
        if (currentFixedAmount >= 1.0f)
        {
            currentFixedAmount = 1.0f;
            fixingBarImage.fillAmount = currentFixedAmount;

            FixedTheJoint();
        }
        fixingBarImage.fillAmount = currentFixedAmount;
    }

    public bool brokenStatus() { return isBroken; }

}
