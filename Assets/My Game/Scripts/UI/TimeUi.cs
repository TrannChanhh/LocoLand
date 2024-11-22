using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class TimeUi : MonoBehaviour
{
    public GameObject clockbar;
    private SunDirection SunDirection;
    // Start is called before the first frame update
    void Start()
    {
        SunDirection = FindObjectOfType < SunDirection>();
        
    }

    // Update is called once per frame
    void Update()
    {
      
        clockbar.transform.Rotate(0, 0,-( SunDirection.sunSpeed * Time.deltaTime));
    }
}
