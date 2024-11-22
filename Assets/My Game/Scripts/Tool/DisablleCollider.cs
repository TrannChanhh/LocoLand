using UnityEngine;
using UnityEditor;

public class DisableBoxCollidersTool : Editor
{
    [MenuItem("Tools/Disable Selected Box Colliders")]
    static void DisableSelectedBoxColliders()
    {
        // Loop through the selected objects in the Hierarchy
        foreach (GameObject obj in Selection.gameObjects)
        {
            // Get the BoxCollider component (if it exists) on the selected GameObject
            BoxCollider collider = obj.GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.enabled = false; // Disable the collider
            }
        }

        // Log the action
        Debug.Log("Disabled Box Colliders for selected GameObjects.");
    }

    [MenuItem("Tools/Enable Selected Box Colliders")]
    static void EnableSelectedBoxColliders()
    {
        // Loop through the selected objects in the Hierarchy
        foreach (GameObject obj in Selection.gameObjects)
        {
            // Get the BoxCollider component (if it exists) on the selected GameObject
            BoxCollider collider = obj.GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.enabled = true; // Enable the collider
            }
        }

        // Log the action
        Debug.Log("Enabled Box Colliders for selected GameObjects.");
    }
}
