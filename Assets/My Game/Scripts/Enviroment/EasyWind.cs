using UnityEngine;

public class WindSway : MonoBehaviour
{
    public float swayAmount = 0.1f; // Amount of sway
    public float swaySpeed = 1f;     // Speed of the sway
    public float swayHeightThreshold = 2.0f; // Height limit for swaying

    private Mesh mesh;
    private Vector3[] originalVertices;

    void Start()
    {
        // Get the MeshFilter and original vertices
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            mesh = meshFilter.mesh;
            originalVertices = mesh.vertices;
        }
    }

    void Update()
    {
        if (mesh != null)
        {
            Vector3[] vertices = new Vector3[originalVertices.Length];
            for (int i = 0; i < originalVertices.Length; i++)
            {
                // Get the vertex's world position
                Vector3 worldPosition = transform.TransformPoint(originalVertices[i]);

                // Check if the vertex is above the swayHeightThreshold
                if (worldPosition.y > swayHeightThreshold)
                {
                    float swayX = Mathf.Sin(Time.time * swaySpeed + i) * swayAmount;
                    float swayZ = Mathf.Sin(Time.time * swaySpeed + i) * swayAmount;

                    // Sway the vertex
                    vertices[i] = originalVertices[i] + new Vector3(swayX, 0, swayZ);
                }
                else
                {
                    // Keep the original position for lower vertices
                    vertices[i] = originalVertices[i];
                }
            }
            // Update the mesh vertices
            mesh.vertices = vertices;
        }
    }
}
