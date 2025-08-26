using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LaserPointerParticle : MonoBehaviour
{
    public TextMeshProUGUI uiText;

    private GameObject currentCollidedObject = null;

    private void LateUpdate() {
        currentCollidedObject = null;
    }

    private void OnParticleCollision(GameObject obj) {
        currentCollidedObject = obj;

    }

    public void HighlightIfColliding() {
        if (currentCollidedObject != null) {
            if (uiText != null)
                uiText.text = currentCollidedObject.name;
        }

        var highlight = currentCollidedObject.GetComponent<HighLightObject>();
        if (highlight != null) {
            highlight.Highlight();
        }
    }

}
