using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager1 : MonoBehaviour
{
    public GameObject mapMenu;
    public GameObject inventorMenu;
    public GameObject deadMenu;
    public GameObject pauseMenu;
    private PlayerHealth health;
    private bool isSection = false;
    MouseMovement mouseMovement;
    // Start is called before the first frame update
    void Start()
    {
        isSection = false;
        mouseMovement = FindAnyObjectByType<MouseMovement>();
        health = FindAnyObjectByType<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mapMenu.SetActive(true);
            ToggleCursor(true);
            isSection = true;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventorMenu.SetActive(true);
            ToggleCursor(true);
            isSection = true;
        }

        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.I))
        {
            Time.timeScale = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isSection)
            {
                
                mapMenu.SetActive(false);
                inventorMenu.SetActive(false);
                isSection = false;
                Time.timeScale = 1f;
                ToggleCursor(false);
            }
            else
            {
                
                pauseMenu.SetActive(true);
                Time.timeScale = pauseMenu.activeSelf ? 0f : 1f;
                ToggleCursor(pauseMenu.activeSelf);
            }
        }

        StartCoroutine(DeadScene());
    }
    public void OnHitReturn()
    {
        pauseMenu.SetActive(false);
        mapMenu.SetActive(false);
        inventorMenu.SetActive(false);
        ToggleCursor(false);
        Time.timeScale = 1f;
        mouseMovement.mouseSensivity = 300f;
    }
    private void ToggleCursor(bool isVisibility)
    {
        Cursor.visible = isVisibility;
        Cursor.lockState = isVisibility ? CursorLockMode.None : CursorLockMode.Locked;
    }
    IEnumerator DeadScene()
    {
        yield return new WaitForSeconds(0.2f);
        if (health.isDeadTriggered)
        {
            deadMenu.SetActive(true);
            ToggleCursor(true);
        }

    }

}
