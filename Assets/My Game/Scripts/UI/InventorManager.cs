using UnityEngine;
using UnityEngine.EventSystems;
using TMPro; // Import TextMeshPro for using TextMeshProUGUI

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;
        TextMeshProUGUI coinText = droppedItem.GetComponentInChildren<TextMeshProUGUI>();
        if (coinText != null)
        {
           
            int coinAmount;
            if (int.TryParse(coinText.text, out coinAmount))
            {
                
                Debug.Log("Coin amount from chest: " + coinAmount);
            }
            else
            {
                Debug.LogWarning("Could not parse coin amount from text: " + coinText.text);
            }
        }
        else
        {
            Debug.LogWarning("No TextMeshProUGUI component found in the dropped item's children.");
        }
    }
}