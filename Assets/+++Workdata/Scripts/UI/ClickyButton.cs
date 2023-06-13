using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite defaultSprite, hoveredSprite, pressedSprite;

    public void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = pressedSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = hoveredSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = defaultSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = defaultSprite;
    }
}
