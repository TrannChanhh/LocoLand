using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int health = 100; 
    public int currenthealth ;

    void Start()
    {
        currenthealth = health;
    }

    public void TakeDamage(int damage)
    {
        AudioManager.Instance.playerSource.PlayOneShot(AudioManager.Instance.playerAttacked);
        AudioManager.Instance.enemySource.PlayOneShot(AudioManager.Instance.enemyAttack);
        currenthealth -= damage;
        if(currenthealth <= 0 )
        {
            currenthealth = 0;
            Debug.Log("Player is dead!");
        }
        Debug.Log("Current Health: " + currenthealth);
        
    }
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
