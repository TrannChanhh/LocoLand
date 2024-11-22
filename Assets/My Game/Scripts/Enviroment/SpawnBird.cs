using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBird : MonoBehaviour
{
    public GameObject Bird;
    public int numberofBirds = 2;
    public Vector2 spawnAreaSize ;
    public float flyingHeight = 5.0f;
    public Transform spawnPosition;
    public float spawnInterval;
    void Start()
    {
       StartCoroutine( SpawnBirds());
    }

    IEnumerator  SpawnBirds()
    {   while (true)
        {
            for (int i = 0; i < numberofBirds; i++)
            {
                float randomX = UnityEngine.Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
                float randomZ = UnityEngine.Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
                Vector3 newPos = new Vector3(spawnPosition.position.x + randomX, flyingHeight, spawnPosition.position.z + randomZ);

                GameObject spawedbird = Instantiate(Bird, newPos, Quaternion.identity);
                float randomRotationY = UnityEngine.Random.Range(0, 360);
                spawedbird.transform.Rotate(0, randomRotationY, 0);

            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
   
}
