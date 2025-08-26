using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.SceneDocumentation;

using Oculus.Interaction.Grab;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction;

public class Ingredient : MonoBehaviour
{
    
    public virtual void cut() { }

    public virtual void cooking() { }

    public virtual void cooked() { }

    public virtual void seasoned() { }

    public virtual void served() { }

    public virtual void reset() { }

}
