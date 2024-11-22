using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform character;
    public RectTransform minimapMarker;

    private Vector2 initialMarkerPosition;
    public float speedMultiplier = 2f;
    private Vector2 mapSize;
    private void Start()
    {
        initialMarkerPosition = minimapMarker.anchoredPosition;

    }
    private void Update()
    {
        Vector2 characterWorldPosition = new Vector2(character.position.x, character.position.z);
        Vector2 ajustPosition = characterWorldPosition * speedMultiplier;
        minimapMarker.anchoredPosition = initialMarkerPosition + ajustPosition;


    }


}