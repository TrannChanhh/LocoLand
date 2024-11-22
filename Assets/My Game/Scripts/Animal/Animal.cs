using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [Header("Enemy Health")]
    public int maxHealth = 100;
    public int currentHealth;
    [Header("Attack")]
    public float timeDestroy = 2.5f;
    public bool isDead = false;
    private int isDeadId;
    private int isWalkId;
    private Animator anim;
    private AnimalMove animalMove;
    //private Rigidbody rb;
    [Header("Item Drop")]
    public List<GameObject> items;
    public GameObject childObject;
    public GameObject geo;
    // luu vi tri
    private Vector3 deathPosition;
    private Quaternion deathRos;
    public int damageWeapon = 100;
    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponentInChildren<Animator>();
        animalMove = GetComponentInChildren<AnimalMove>();
        isDeadId = Animator.StringToHash("isDead");
        isWalkId = Animator.StringToHash("isWalking");
        //rb = GetComponentInChildren<Rigidbody>();
    }
    private void Update()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            DeadEnemy();
        }
    }
    public void TakeDamage(int damage, Vector3 force, GameObject instigator)
    {
        if (isDead) return;
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isDead = true;
            DeadEnemy();
        }
    }
    private void OnTriggerEnter(Collider collide)
    {

        if (collide.gameObject.CompareTag("Arrow") && !isDead)
        {
            
            TakeDamage(damageWeapon, Vector3.zero, collide.gameObject);

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow") && !isDead)
        {

            ImpactFx(collision);

        }
    }
    private void DeadEnemy()
    {
        anim.SetBool(isWalkId, false);
        anim.SetBool(isDeadId, true);
        animalMove.enabled = false;
        //rb.isKinematic = true;

        deathPosition = childObject.transform.position;
        deathRos = childObject.transform.rotation;

        Destroy(gameObject, timeDestroy);
        Invoke("SpawnItem", 2.49f);
    }

    private void SpawnItem()
    {
        geo.SetActive(false);
        foreach (GameObject item in items)
        {
            Vector3 spawnPosition = deathPosition + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
            Instantiate(item, spawnPosition, deathRos);
        }
    }
    private void ImpactFx(Collision collision)
    {
        ContactPoint contactPoint = collision.contacts[0];
        GameObject ImpactFX = Instantiate(FX_Reference.Instance.fxImpactDamage, contactPoint.point, Quaternion.identity);
        ImpactFX.transform.SetParent(collision.gameObject.transform);
    }
}
