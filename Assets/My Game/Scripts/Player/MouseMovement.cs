using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
[AddComponentMenu("TrannChanhh/MouseMovement")]

public class MouseMovement : MonoBehaviour
{
    [Header("Mouse")]
    public float mouseSensivity = 450f;
    private float xRotation;
    private float yRotation;
    [Header("Clamp")]
    public float topClamp = -90f;
    public float bottonClamp = 90f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;
        xRotation -= mouseY;
        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottonClamp);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
