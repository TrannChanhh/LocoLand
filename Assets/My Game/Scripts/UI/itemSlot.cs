using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class itemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            RectTransform draggedItem = eventData.pointerDrag.GetComponent<RectTransform>();
            Dragitem dragItemScript = eventData.pointerDrag.GetComponent<Dragitem>();

            // Check coi co ton tai gi trong slot ko , neu co thi thay no 
            if (transform.childCount > 0)
            {
                Transform existingItem = transform.GetChild(0);
                if(existingItem.CompareTag("Coin"))
                {
                    Destroy(existingItem.gameObject );
                }
                Dragitem existingItemScript = existingItem.GetComponent<Dragitem>();
                existingItem.SetParent(existingItemScript.originalParent, true);
                RectTransform existingItemRectTransform = existingItem.GetComponent<RectTransform>();
                existingItemRectTransform.anchoredPosition = existingItemScript.originalPosition;  // tra no ve vi tri cia item
            }


            draggedItem.SetParent(transform);
            draggedItem.localPosition = Vector3.zero;
            TextMeshProUGUI coinText = draggedItem.GetComponentInChildren<TextMeshProUGUI>();
            if (coinText != null)
            {
                int coinAmount;
                if (int.TryParse(coinText.text, out coinAmount))
                {
                    Debug.Log("Coin amount from chest: " + coinAmount);
                    DataKey.Instance.Coin += coinAmount;
                    Debug.Log("Total coins in DataKey: " + DataKey.Instance.Coin);
                    if(transform.childCount > 0)
                    {
                        Transform existingItem = transform.GetChild(0);
                        TextMeshProUGUI existingCointext = existingItem.GetComponent<TextMeshProUGUI>();
                        if(existingCointext != null)
                        {
                            int existingCoinAmount;
                            if(int.TryParse(existingCointext.text, out existingCoinAmount))
                            {
                                existingCointext.text = (existingCointext.text + coinAmount).ToString();
                            }
                        }
                    }
                }
            }   
        }
    }
}
