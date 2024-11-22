using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Dragitem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform dragItem;
    [SerializeField] private Canvas canvas;
    public Vector3 originalPosition;
    private CanvasGroup canvasGroup;
    public Transform originalParent;

    void Awake()
    {
        dragItem = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = dragItem.parent;
        originalPosition = dragItem.anchoredPosition;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;


    }
    public void OnDrag(PointerEventData eventData)
    {

        RectTransformUtility.ScreenPointToLocalPointInRectangle(dragItem.parent as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
        dragItem.localPosition = localPoint;

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if (eventData.pointerEnter != null)
        {
            dragItem.anchoredPosition = originalPosition;
        }
    }
}



