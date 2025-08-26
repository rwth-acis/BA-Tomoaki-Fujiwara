using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public bool isWheelMilled = false;
    public bool isWheelWashed = false;
    public bool isWheelLathe = false;


    public RobotArmManager robotArmManager;

    private bool isHeld = false;



    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Hand_Grabber[Rotate:y]")
        {
            isHeld = true;

            robotArmManager.SetHoldWheel(this);

            // if the object is target, then set the parent from self
            transform.SetParent(other.transform);
            // Set the position and rotation to zero
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        else if (!isHeld)
        {

            if (other.gameObject.name == "WheelPosition[Lathe]")
            {
                // if the object is target, then set the parent from self
                transform.SetParent(other.transform);
                // Set the position
                transform.localPosition = Vector3.zero;
                // Set the rotation to 90 degrees on the Z axis
                transform.localRotation = Quaternion.Euler(0, 0, 90);
            }

            else if (other.gameObject.name == "WheelSetPlace1[Table]")
            {
                // if the object is target, then set the parent from self
                transform.SetParent(other.transform);
                // Set the position and rotation to zero
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }

            else if (other.gameObject.name == "WheelSetPlace2[Table]")
            {
                // if the object is target, then set the parent from self
                transform.SetParent(other.transform);
                // Set the position and rotation to zero
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }

            else if (other.gameObject.name == "WheelPlace[Mill]")
            {
                // if the object is target, then set the parent from self
                transform.SetParent(other.transform);
                // Set the position and rotation to zero
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }

            else if (other.gameObject.name == "WheelSpawn[Band2]")
            {
                // if the object is target, then set the parent from self
                transform.SetParent(other.transform);
                // Set the position and rotation to zero
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
        }
    }

    public void Release()
    {
        isHeld = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
