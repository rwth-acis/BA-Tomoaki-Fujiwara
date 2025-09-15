using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetToolPosition : MonoBehaviour
{

    public Spanner spannerItem;
    public ToolTurret toolTurretItem;
    public RobotArmHandGrabber robotArmHandGrabberItem;
    public LatheSpindle latheSpindleItem;
    public MillingCutter millingCutter;

    public void ResetAllToolPositions()
    {
        if (spannerItem != null)
        {
            spannerItem.ResetPosition();
        }
        if (toolTurretItem != null)
        {
            toolTurretItem.ResetPosition();
        }
        if (robotArmHandGrabberItem != null)
        {
            robotArmHandGrabberItem.ResetPosition();
        }
        if (latheSpindleItem != null)
        {
            latheSpindleItem.ResetPosition();
        }
        if (millingCutter != null)
        {
            millingCutter.ResetPosition();
        }
    }
}
