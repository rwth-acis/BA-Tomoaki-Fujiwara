using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaschineSceneManager : MonoBehaviour
{
    public int finishedWheelNumber = 0;

    public RobotArmManager robotArmManager;

    public void DeliveryFinishedWheel()
    {
        finishedWheelNumber += 1;

        if (finishedWheelNumber == 1)
        {
            // Break Foundation
            robotArmManager.PartsBreakeFoundation();

        }
        else if (finishedWheelNumber == 2)
        {
            // Break Lower upper arm
            robotArmManager.PartsBreakeLowerUpperArm();

        }
        else if (finishedWheelNumber == 3)
        {
            // Break LowerArm
            robotArmManager.PartsBreakeLowerArm();

        }

        else if (finishedWheelNumber == 4)
        {
            // Break hand grab
            robotArmManager.PartsBreakeHandGrabber();

        }

        else if (finishedWheelNumber == 5)
        {
            // Enough wheels are delivered
            Debug.Log("10 wheels are delivered");
        }

    }
}
