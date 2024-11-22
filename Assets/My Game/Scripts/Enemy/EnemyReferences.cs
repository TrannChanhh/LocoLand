using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[AddComponentMenu("TrannChanhh/EnemyReferences")]
[DisallowMultipleComponent]
public class EnemyReferences : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent navMeshagent;
    public float navMeshSampleDistance = 5.0f;
    public float chaseDistance = 10.0f;
    [HideInInspector]
    public Animator animator;

    [Header("Stats")]
    public float pathUpdateDelay = 0.2f;

    private void Awake()
    {
        navMeshagent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
}
