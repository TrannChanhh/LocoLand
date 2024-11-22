using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomItemInChest : MonoBehaviour
{
    [System.Serializable]
    public class Item
    {
        public string itemName;
        public int minQuantity;
        public int maxQuantity;
        public GameObject itemSlotUI;
        public GameObject itemPrefab;

    }

    public List<Item> items = new List<Item>();

    void Start()
    {
        GenerateRandomItem();
    }

    private void GenerateRandomItem()
    {
        foreach (Item item in items)
        {
            if (item.itemSlotUI.transform.childCount == 0)
            {
                int quantity = UnityEngine.Random.Range(item.minQuantity, item.maxQuantity);
                GameObject itemInstance = Instantiate(item.itemPrefab, item.itemSlotUI.transform);
                itemInstance.transform.localPosition = Vector3.zero;
                TMP_Text quantityText = itemInstance.GetComponentInChildren<TMP_Text>();
                if (quantityText != null)
                {
                    quantityText.text = quantity.ToString();
                }
            }
        }
    }

    public void ResetChest()
    {
        foreach (Item item in items)
        {
            // Reset the slot UI by destroying existing children (if any)
            foreach (Transform child in item.itemSlotUI.transform)
            {
                Destroy(child.gameObject);  // Destroy the previous item
            }

            // Generate new item after clearing the slot
            GenerateRandomItem();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
}
