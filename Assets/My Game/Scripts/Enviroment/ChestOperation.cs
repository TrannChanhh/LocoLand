using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChestOperation : MonoBehaviour
{
    private Animator anim;
    private int isOpenID;
    public bool isOpenChest;
    private bool playerInRange;

    public GameObject itemMenu;   
    public GameObject chestMenu;
    private RandomItemInChest randomItemInChest;
    private MouseMovement mouseMovement; 
    void Start()
    {

        anim = GetComponent<Animator>();
        isOpenID = Animator.StringToHash("IsOpen");
        mouseMovement = FindAnyObjectByType<MouseMovement>();
        randomItemInChest = FindAnyObjectByType<RandomItemInChest>();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && !isOpenChest && playerInRange)
        {
            OpenChest();
        }
        
        else if (Input.GetKeyDown(KeyCode.Escape) && isOpenChest)
        {
            CloseChest();
        }
    }

    private void OpenChest()
    {
        anim.SetTrigger(isOpenID);      
        itemMenu.SetActive(true);      
        chestMenu.SetActive(true);      
        isOpenChest = true;             

        Cursor.visible = true;          
        Cursor.lockState = CursorLockMode.None;  
        mouseMovement.mouseSensivity = 0; 
    }

    private void CloseChest()
    {
        itemMenu.SetActive(false);      
        chestMenu.SetActive(false);    
        isOpenChest = false;            

        Cursor.visible = false;         
        Cursor.lockState = CursorLockMode.Locked;
        mouseMovement.mouseSensivity = 300f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            randomItemInChest.ResetChest();
            chestMenu.SetActive(false);
        }
    }
}
