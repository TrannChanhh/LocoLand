using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_Reference : MonoBehaviour
{
    public GameObject fxImpactDamage;
    public GameObject fxImpactAnimal;
    public static FX_Reference Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
