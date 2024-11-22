using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Inventor Slot")]
    public GameObject weaponSlot;



    [Header("Bow and Arrow")]
    public GameObject arrowPrefabs;
    private GameObject currentarrow;
    public Transform arrowSpawnPoint;
    public float arrowForce = 10f;
    public float aimSpeed = 0.5f;
    public float maxAimTime = 0.1f;

    public bool usesBows = false;
    private int isDrawBowID;
    private int isSteadyShotID;
    private int isShotID;
    private int isHoldShot;

    private Animator anim;
    private bool isAiming = false;
    private Coroutine aimCoroutine;
    public Vector3 ArrowOrihental;
    private float currentAimTime;
    private float hitButtonTime;

    [Header("Sword and Sheild")]
    public bool usesSwordAndSheild = false;
    private int isDrawSwordandSheildID;
    private int isSteadySwordID;
    private int isSplashID;
    private int isBlockID;
    private int isHoldBlockID;

    [Header("One Hand")]
    public bool usesOneHand = false;
    private int isDrawOneHandID;
    private bool isOneHandDrawn = false;

    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponentInChildren<Animator>();
        isDrawBowID = Animator.StringToHash("IsDrawBow");
        isSteadyShotID = Animator.StringToHash("IsSteadyShot");
        isShotID = Animator.StringToHash("IsShot");
        isSplashID = Animator.StringToHash("IsSplash");
        isDrawSwordandSheildID = Animator.StringToHash("IsDrawSword");
        isBlockID = Animator.StringToHash("IsBlock");
        isDrawOneHandID = Animator.StringToHash("IsDrawOneHand");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBowEquipped() && weaponSlot.transform.GetChild(0).CompareTag("Bow"))
        {
            HandleBow();
        }
        if (IsSwordandShiedEquipped() && weaponSlot.transform.GetChild(0).CompareTag("SwordAndSheild"))
        {
            HandleSwordAndSheild();
        }
        if (IsOneHandEquipped() && weaponSlot.transform.GetChild(0).CompareTag("OneHand"))
        {
            HandleOneHand();
        }
    }

    private void HandleOneHand()
    {
       
            if (Input.GetKeyDown(KeyCode.F))
            {
                isOneHandDrawn = !isOneHandDrawn;
                anim.SetBool(isDrawOneHandID, isOneHandDrawn);
        }
            
        
    }
    private bool IsOneHandEquipped()
    {
        if (weaponSlot.transform.childCount > 0)
        {
            Transform onehand = weaponSlot.transform.GetChild(0);
            if (onehand.CompareTag("OneHand"))
            {
                return true;
            }
        }
        return false;
    }

    private void HandleBow()
    {
        isAiming = false;
        if (Input.GetKeyDown(KeyCode.F))
        {
            bool isCurrentDrawBow = anim.GetBool(isDrawBowID);
            if (isCurrentDrawBow)
            {
                anim.SetBool(isDrawBowID, false);
                usesBows = false;
            }
            else
            {
                anim.SetBool(isDrawBowID, true);
                usesBows = true;
            }
        }
        if (usesBows == true && Input.GetButtonDown("Fire1"))
        {
            if (!isAiming)
            {
                aimCoroutine = StartCoroutine(AimAndShoot());
                hitButtonTime = Time.time;
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            float currentAimTime = Time.time - hitButtonTime;
            if (aimCoroutine != null)
            {
                StopCoroutine(aimCoroutine);
                isAiming = false;
                FireArrow(currentAimTime);
            }
        }
    }

    private void TriggerQuickShot()
    {
        anim.SetTrigger(isShotID);
        isAiming = false;
    }
    IEnumerator AimAndShoot()
    {
        isAiming = true;
        anim.SetTrigger(isSteadyShotID);
        yield return new WaitForSeconds(0.2f);
        yield return null;

    }
    private void FireArrow(float currentaimTime)
    {
        if (usesBows && !usesSwordAndSheild && anim.GetBool(isDrawBowID))
        {
            AudioManager.Instance.playerSource.PlayOneShot(AudioManager.Instance.Bows);
            GameObject arrow = Instantiate(arrowPrefabs, arrowSpawnPoint.position, Quaternion.identity);
            Rigidbody rb = arrow.GetComponentInChildren<Rigidbody>();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 targetPoint;
            rb.useGravity = true;
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(100);
            }
            float finalArrowForce = 0f;

            if (currentaimTime <= maxAimTime)
            {
                finalArrowForce = arrowForce * aimSpeed;
            }
            else
            {
                finalArrowForce = arrowForce;
            }
            Vector3 direction = (targetPoint - arrowSpawnPoint.position).normalized;
            rb.AddForce(direction * finalArrowForce, ForceMode.Impulse);
            arrow.transform.forward = direction;
            arrow.transform.Rotate(ArrowOrihental);
            TriggerQuickShot();
            Destroy(arrow, 3f);
            isAiming = false;
        }
    }
    private bool IsBowEquipped()
    {
        if (weaponSlot.transform.childCount > 0)
        {
            Transform bow = weaponSlot.transform.GetChild(0);
            if (bow.CompareTag("Bow"))
            {
                return true;
            }
        }
        return false;
    }
    private bool IsSwordandShiedEquipped()
    {
        if (weaponSlot.transform.childCount > 0)
        {
            Transform ss = weaponSlot.transform.GetChild(0);
            if (ss.CompareTag("SwordAndSheild"))
            {
                return true;
            }
        }
        return false;
    }
    private void HandleSwordAndSheild()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            bool isCurrentHoldSwordAndShield = anim.GetBool(isDrawSwordandSheildID);
            if (isCurrentHoldSwordAndShield)
            {
                anim.SetBool(isDrawSwordandSheildID, false);
                usesSwordAndSheild = false;
            }
            else
            {
                anim.SetBool(isDrawSwordandSheildID, true);
                usesSwordAndSheild = true;
                usesBows = false;
            }
        }
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger(isSplashID);
        }
        if (Input.GetButton("Fire2"))
        {
            anim.SetBool(isBlockID, true);
        }
        else
        {
            anim.SetBool(isBlockID, false);
        }
    }
}


