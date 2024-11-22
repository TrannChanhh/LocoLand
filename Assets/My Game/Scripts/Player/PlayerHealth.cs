using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour
{
    private PlayerInfo playerData;
    private PlayerMovement playerMovement;
    public UnityEngine.UI.Image redOverPlay;
    public float flashDuration = 0.2f;
    public Camera mainCamera;
    private Animator cameraAnim;
    public bool isDeadTriggered = false;
    private MouseMovement mouseMovement;
    private UIManager uiManager;
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        mouseMovement = FindAnyObjectByType<MouseMovement>();
        playerMovement = GetComponent<PlayerMovement>();
        redOverPlay.gameObject.SetActive(false);
        playerData = GetComponent<PlayerInfo>();
        if (mainCamera != null)
        {
            cameraAnim = mainCamera.GetComponentInChildren<Animator>();
        }
        else
        {
            Debug.LogError("Main Camera not assigned!");
        }
        UpDateHealthUI();
    }

   
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            AudioManager.Instance.playerSource.PlayOneShot(AudioManager.Instance.playerAttacked);
            AudioManager.Instance.enemySource.PlayOneShot(AudioManager.Instance.enemyAttack);

            TriggerCameraShake();
            TriggerFlash();
            playerData.TakeDamage(10);
            UpDateHealthUI();
        }
        
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            cameraAnim.SetBool("IsHit", false);
        }
    }
    private void TriggerFlash()
    {
        redOverPlay.gameObject.SetActive(true);
        StartCoroutine(FlashCorotine());
    }

    IEnumerator FlashCorotine()
    {
     
       redOverPlay.color = new Color(redOverPlay.color.r, redOverPlay.color.g, redOverPlay.color.b, 0.5f);
       yield return new WaitForSeconds(flashDuration);
       redOverPlay.color = new Color(redOverPlay.color.r, redOverPlay.color.g, redOverPlay.color.b, 0);
        redOverPlay.gameObject.SetActive(false);
    }
    private void TriggerCameraShake()
    {
        if (cameraAnim != null)
        {
            cameraAnim.SetBool("IsHit", true);
        }
    }
    private void Update()
    {
        if(playerData.currenthealth <= 0 && !isDeadTriggered)
        {
            AudioManager.Instance.playerSource.PlayOneShot(AudioManager.Instance.playerDead);
            isDeadTriggered = true;
            cameraAnim.SetBool("IsDead",true);
            playerMovement.speedMove = 0;
            mouseMovement.mouseSensivity = 0;
            UpDateHealthUI();
        }
    }
    public void UpDateHealthUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateHealthUI(playerData.currenthealth);
        }
    }
}
