using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    /// <summary>
    /// main camera
    /// </summary>
    public Camera cam;

    /// <summary>
    /// transform of the player
    /// </summary>
    public Transform player;

    /// <summary>
    /// startposition of the background
    /// </summary>
    private Vector2 startPosition;

    /// <summary>
    /// z startposition of the background
    /// </summary>
    private float startZ;

    /// <summary>
    /// the distance the camera has moved from the startposition of the background
    /// </summary>
    private Vector2 travel => (Vector2)cam.transform.position - startPosition;

    /// <summary>
    /// z distance from the background to the player
    /// </summary>
    private float distanceFromPlayer => transform.position.z - player.position.z;

    /// <summary>
    /// z position of the clipping plane we want to use
    /// </summary>
    private float clippingPlane => (cam.transform.position.z + (distanceFromPlayer > 0 ? cam.farClipPlane : cam.nearClipPlane));

    /// <summary>
    /// parallax factor to move the background
    /// </summary>
    private float parallaxFactor => Mathf.Abs(distanceFromPlayer) / clippingPlane;

    /// <summary>
    /// set the startposition to the position of the object in scene
    /// </summary>
    private void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
    }

    /// <summary>
    /// only setting the x and y axis from the background
    /// </summary>
    private void Update()
    {
        Vector2 newPos = startPosition + travel * parallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }
}
