using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScroller : MonoBehaviour
{
    //link to the raw image component
    [SerializeField] RawImage image;

    //x position to add to the uvRect position
    [SerializeField] float x;

    /// <summary>
    /// adds the x variable to the uvRect x position and keeps the y position
    /// </summary>
    private void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(x, image.uvRect.y) * Time.deltaTime, image.uvRect.size);
    }
}
