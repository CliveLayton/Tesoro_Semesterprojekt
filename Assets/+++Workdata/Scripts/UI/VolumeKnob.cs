using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeKnob : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    /// <summary>
    /// image component of the button
    /// </summary>
    [SerializeField] private Image image;

    /// <summary>
    /// sprites for different states
    /// </summary>
    [SerializeField] private Sprite defaultSprite, pressedSprite;

    /// <summary>
    /// set the image sprite to pressedSprite
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.buttonSelected, this.transform.position);
        image.sprite = pressedSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.buttonHovered, this.transform.position);
    }

    /// <summary>
    /// set the image sprite back to defaultSprite
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = defaultSprite;
    }
}
