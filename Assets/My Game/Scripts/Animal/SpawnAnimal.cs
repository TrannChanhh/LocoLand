using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAnimal : MonoBehaviour
{
    private float timer = 0f;
    public float timeSpawn = 5f;
    public int maxSpawnCount = 5;
    public List<GameObject> posSpawnAnimalList = new List<GameObject>();
    public List<GameObject> animalList = new List<GameObject>();
    public List<GameObject> animalCount = new List<GameObject>();
    private void Start()
    {
        AddPosToList();
    }
    void AddPosToList()
    {
        foreach (Transform child in transform)
        {
            posSpawnAnimalList.Add(child.gameObject);
        }
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeSpawn && animalCount.Count < maxSpawnCount)
        {
            SpawningAnimal();
            timer = 0f;
        }
    }
    void SpawningAnimal()
    {
        int x = Random.Range(0, posSpawnAnimalList.Count);
        int y = Random.Range(0, animalList.Count);
        GameObject animal = Instantiate(animalList[y], posSpawnAnimalList[x].transform.position, posSpawnAnimalList[x].transform.rotation);
        animal.SetActive(true);
        animalCount.Add(animal);
    }
}
