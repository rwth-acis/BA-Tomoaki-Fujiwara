using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoClipRotateToUser : MonoBehaviour
{
    public Transform userHead;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (userHead != null)
        {
            // Make this object look at the user's head, but flipped
            transform.LookAt(2 * transform.position - userHead.position);
        }
    }
}
