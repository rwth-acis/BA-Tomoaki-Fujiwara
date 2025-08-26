using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class ScreenShotImagePreview : MonoBehaviour
{

    public RawImage rawImage;
    public RenderTexture renderTexture;

    // Start is called before the first frame update
    public void OnEnable() {
        rawImage.texture = renderTexture;
    }

    public void OnDisable() {
        rawImage.texture = null;
    }

}
