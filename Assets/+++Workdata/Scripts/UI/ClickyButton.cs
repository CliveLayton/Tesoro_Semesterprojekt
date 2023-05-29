using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite defaultSprite, pressedSprite;

    public void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = pressedSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = defaultSprite;
    }
}
