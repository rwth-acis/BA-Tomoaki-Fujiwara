using UnityEngine;

public class FlourBagController : MonoBehaviour
{
    public GameObject flourParicleObject;
    public ParticleSystem flourParticles;
    public Transform flourPour;
    public float emissionAngleThreshold = 45f; // Example: Emit if the angle with the world's downward direction is less than 45 degrees

    void Start()
    {
        if (flourParticles != null)
        {
            flourParticles.Stop();
        }

        flourParicleObject.SetActive(false);

        Debug.Log("FlourBagController started.");
        Debug.Log(-transform.up);
        Debug.Log(Vector3.down);
    }

    void Update()
    {
        // Calculate the direction from the bag's center to the spout (pour point)
        Vector3 spoutDirection = (flourPour.position - transform.position).normalized;
        float angle = Vector3.Angle(spoutDirection, Vector3.down);

        if (angle < emissionAngleThreshold)
        {
            flourParicleObject.SetActive(true);
            if (!flourParticles.isPlaying)
            {
                flourParticles.Play();
            }
            
        }
        else
        {
            flourParicleObject.SetActive(false);
            if (flourParticles.isPlaying)
            {
                flourParticles.Stop();
            }
        }
    }
}