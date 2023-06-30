using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScroller : MonoBehaviour
{
    [SerializeField] RawImage image;

    [SerializeField] float x;

    private void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(x, image.uvRect.y) * Time.deltaTime, image.uvRect.size);
    }
}
