using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightObject : MonoBehaviour
{
    private bool isHighlighting = false;

    [SerializeField]private Material normalMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private string highlightLayerName = "Selected";
    [SerializeField] private string normalLayerName = "Selectable";

    private int selectedLayer;

    private int selectableLayer;

    public void Start() {
        int layerIndex = LayerMask.NameToLayer(highlightLayerName);
        if (layerIndex != -1) {
            selectedLayer = layerIndex;
        }

        int layerIndex2 = LayerMask.NameToLayer(normalLayerName);
        if (layerIndex2 != -1) {
            selectableLayer = layerIndex2;
        }
    }

    public void Highlight() {
        if (!isHighlighting) {
            isHighlighting = true;
            GetComponent<Renderer>().material = highlightMaterial;
            gameObject.layer = selectedLayer;
        } else {
            isHighlighting = false;
            // Revert to normal state
            GetComponent<Renderer>().material = normalMaterial;
            gameObject.layer = selectableLayer;
        }

    }
}
