using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// image component of the button
    /// </summary>
    [SerializeField] private Image image;

    /// <summary>
    /// sprites for different states
    /// </summary>
    [SerializeField] private Sprite defaultSprite, hoveredSprite, pressedSprite;

    /// <summary>
    /// set the image sprite to pressedSprite
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!GetComponent<Button>().interactable)
            return;

        AudioManager.instance.PlayOneShot(FMODEvents.instance.buttonSelected, this.transform.position);
        image.sprite = pressedSprite;
    }

    /// <summary>
    /// set the image sprite to hoveredSprite
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!GetComponent<Button>().interactable)
            return;

        AudioManager.instance.PlayOneShot(FMODEvents.instance.buttonHovered, this.transform.position);
        image.sprite = hoveredSprite;
    }

    /// <summary>
    /// set the image sprite back to defaultSprite
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!GetComponent<Button>().interactable)
            return;

        image.sprite = defaultSprite;
    }

    /// <summary>
    /// set the image sprite back to defaultSprite
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!GetComponent<Button>().interactable)
            return;

        image.sprite = defaultSprite;
    }
}
