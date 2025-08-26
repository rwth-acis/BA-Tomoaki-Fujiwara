using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Convyor : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator convyorAnimator;
    public bool hasWheelOnIt = false;


    public void Start()
    {
        convyorAnimator.Play("Convyor_Moving");
    }

    public void MoveTheBelt() { 
        convyorAnimator.Play("Convyor_Moving");
    }

    public void StopTheBelt() {
        hasWheelOnIt = true;
    }

    public void WheelIsTakenAway() {
        hasWheelOnIt = false;
    }
}
