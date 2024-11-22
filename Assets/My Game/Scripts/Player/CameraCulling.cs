using System.Collections.Generic;
using UnityEngine;

public class CullingCamera : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject player;
    public string tagToCull = "Vegetation"; // The tag to cull
    public float checkInterval = 0.5f; // Time interval to check the objects
    public float cullingBuffer = 0.2f;
    public float cameraMoveThreshold = 0.1f;
    private List<GameObject> objecttoCull;
    private Vector3 lastCameraPostion;
    public float playerProximityRange = 20f;
    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Get the main camera if not assigned
        }
        objecttoCull = new List<GameObject>(GameObject.FindGameObjectsWithTag(tagToCull));
        lastCameraPostion = mainCamera.transform.position;

    }
    private void Update()
    {
        if (Vector3.Distance(mainCamera.transform.position, lastCameraPostion) > cameraMoveThreshold)
        {
            CheckCulling();
            lastCameraPostion = mainCamera.transform.position;
        }

    }
    private void CheckCulling()
    {


        foreach (GameObject obj in objecttoCull)
        {
            float distanceToPlayer = Vector3.Distance(obj.transform.position, player.transform.position);
            if (distanceToPlayer <= playerProximityRange)
            {
                if (!obj.activeSelf)
                {
                    obj.SetActive(true);
                }
                continue;
            }



            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(obj.transform.position);


            if (viewportPoint.x > 0 - cullingBuffer && viewportPoint.x < 1 + cullingBuffer && viewportPoint.y > 0 - cullingBuffer && viewportPoint.y < 1 + cullingBuffer && viewportPoint.z > -0.5f)
            {
                if (!obj.activeSelf)
                {
                    obj.SetActive(true);
                }
            }
            else
            {
                if (obj.activeSelf)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
