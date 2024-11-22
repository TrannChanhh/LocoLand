using UnityEngine;

public class DataKey : MonoBehaviour
{
   
    public static DataKey Instance;

    private int _coin;


    
    public int Coin
    {
        get { return _coin; }
        set { _coin = Mathf.Max(0, value); } 
    }

    

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }


    public void ResetItems()
    {
        Coin = 0;
    }
}
