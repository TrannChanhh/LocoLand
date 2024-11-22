using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[AddComponentMenu("TrannChanhh/Enemy")]
public class Enemy : MonoBehaviour
{
    [Header("Enemy Health")]
    public int maxHealth = 100;

    private int currentHealth;
    private bool isEnemyCheck = false;
    public bool isDead = false;
    public int damageShot = 80 ;
    private Animator anim;
    private int isDeadID;
    public float timeDestroy = 3f;

    [Header("Item Drop")]
    public List<GameObject> items;
    public GameObject childObject;
    public GameObject geo;
    private Vector3 deathPosition;
    private Quaternion deathRos;
    private NavMeshAgent navMeshAgent;
    private EnemyBrain_Stupid brain;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponentInChildren<Animator>();
        isDeadID = Animator.StringToHash("IsDead");
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        brain = GetComponentInChildren<EnemyBrain_Stupid>();
    }

    public void TakeDamage (int damage , Vector3 force, GameObject instigator)
    {
        if (!isEnemyCheck) 
        {
            if (currentHealth > 0)
            {
                currentHealth -= damage;
            }
            if (currentHealth < 0) 
            {
                currentHealth = 0;
                DeadEnemy();

                AudioManager.Instance.enemySource.PlayOneShot(AudioManager.Instance.enemyDead);
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {

        if (!isDead && collision.gameObject.CompareTag("Arrow"))
        {
            AudioManager.Instance.playerSource.PlayOneShot(AudioManager.Instance.ArrowSound);
            ImpactFx(collision);
            TakeDamage(damageShot, Vector3.zero, collision.gameObject);
        }
    }
    private void DeadEnemy()
    {   
        isDead = true;
        anim.SetBool(isDeadID,true);
        navMeshAgent.enabled = false;
        deathPosition = childObject.transform.position;
        deathRos = childObject.transform.rotation;
        if (brain != null) brain.enabled = false;
        Destroy(gameObject, timeDestroy);
        Invoke("SpawnItem", 2.49f);
    }
    private void SpawnItem()
    {
        geo.SetActive(false);
        foreach (GameObject item in items)
        {
            Vector3 spawnPosition = deathPosition + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 0, UnityEngine.Random.Range(-0.5f, 0.5f));
            Instantiate(item, spawnPosition, deathRos);
        }
    }
    private void ImpactFx(Collision collision)
    {
        ContactPoint contactPoint= collision.contacts[0];
        GameObject ImpactFX = Instantiate(FX_Reference.Instance.fxImpactDamage, contactPoint.point , Quaternion.identity);
        ImpactFX.transform.SetParent(collision.gameObject.transform);
    }
}
