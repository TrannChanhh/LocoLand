using UnityEngine;

public class DustParticleController : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    public ParticleSystem dustParticles; // Reference to the dust particle system
    public ParticleSystem LeafParticles;
    public Vector3 offset; // Offset to position the particles

    void Update()
    {
        // Follow the player's position with the specified offset
        Vector3 targetPosition = player.position + offset;

        // Set the particle system position
        dustParticles.transform.position = targetPosition;
        LeafParticles.transform.position = targetPosition;
        // Ensure the dust particle system does not rotate
        dustParticles.transform.rotation = Quaternion.Euler(-90, 0, 0);
        LeafParticles.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }
}