using Oculus.Interaction.HandGrab;
using UnityEngine;
using TMPro;
public class LaserPointer : MonoBehaviour, IHandGrabUseDelegate
{
    [SerializeField] private GameObject laserBeam; // Laser beam particle object
    //[SerializeField] private ParticleSystem laserParticle;
    [SerializeField] private LaserPointerParticle laserPointerParticle;
    private bool isTriggerPressed = false;

    public TextMeshProUGUI uiText;

    public Transform startPosition;



    public void Respawn() { 
        transform.position = startPosition.position;
    }


    // Called when the hand starts grabbing the laser pointer object
    public void BeginUse()
    {
        laserBeam.SetActive(true);
        uiText.text = "laser activate";
    }

    // Called when the hand stops grabbing the laser pointer object
    public void EndUse()
    {
        laserBeam.SetActive(false);
        //uiText.text = "Laser pointer released";
        laserPointerParticle.HighlightIfColliding();
    }

    public float ComputeUseStrength(float strength) {
        //isTriggerPressed = (strength > 0.9f);
        return strength;
    }


    private void OnParticleCollision(GameObject obj) {
        uiText.text = obj.name;


    }

    public void UnselectAllObject() {
        int selectedLayer = LayerMask.NameToLayer("Selected");
        if (selectedLayer == -1) {
            Debug.LogWarning("Selected Layer not exists");
            return;
        }

       
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects) {
            if (obj.layer == selectedLayer) {

                var unselectable = obj.GetComponent<HighLightObject>();
                if (unselectable != null) {
                    // This will unselect the object
                    unselectable.Highlight();
                }
            }
        }

    }

}