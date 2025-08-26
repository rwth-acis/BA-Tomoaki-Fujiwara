using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtUser : MonoBehaviour
{

    public GameObject user;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (user != null)
        {
            Vector3 targetPosition = new Vector3(user.transform.position.x,
                                                 this.transform.position.y,
                                                 user.transform.position.z);


            Vector3 lookAtPosition = 2 * transform.position - targetPosition;


            transform.LookAt(lookAtPosition);
        }
    }
}
