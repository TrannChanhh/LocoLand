using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlybBird : MonoBehaviour
{
    public float speed = 2.0f;
    public float flightDistance = 20f;

    Vector3 spawnPosition;
    Vector3 moveDirection;
    private float distanceTraveled = 0f;
    private Rigidbody rb;
    bool isDead = false;
    Animator anim;
    private int isDeadID;
    private Collider birdCollider;
    private SunDirection SunDirection;

    void Start()
    {
        birdCollider = GetComponent<Collider>();
        spawnPosition = transform.position;
        moveDirection = new Vector3(-speed, 0f, 0f);
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rb.useGravity = false;
        isDeadID = Animator.StringToHash("IsDead");
        SunDirection = FindAnyObjectByType<SunDirection>();
    }

    void Update()
    {
        // Fly the bird until it has traveled the designated distance
        if (!isDead)
        {
            transform.Translate(moveDirection * speed * Time.deltaTime);
            distanceTraveled = Vector3.Distance(spawnPosition, transform.position);

            if (distanceTraveled >= flightDistance)
            {
                Destroy(gameObject); // Destroy the bird object
            }
        }

        // Check if it’s night and deactivate the bird
        DeactivateOnNight();
    }

    private void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject.CompareTag("Arrow") && !isDead)
        {
            anim.SetTrigger(isDeadID);
            isDead = true;
            speed = 0f;
            rb.useGravity = true;
            rb.isKinematic = false;
            birdCollider.isTrigger = false;

            Destroy(gameObject, 10f);
        }
    }

    private void DeactivateOnNight()
    {
        if (SunDirection != null && !SunDirection.IsSunny())
        {
            gameObject.SetActive(false); // Deactivate the bird at night
        }
    }
}
