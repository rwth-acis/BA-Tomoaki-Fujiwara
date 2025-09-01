using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectableObjectComponent : MonoBehaviour
{

    public SelectObject selectableParent;

    // Start is called before the first frame update
    
    public void NoticeParent()
    {
        selectableParent.HitLaserpointer();
    }


}
