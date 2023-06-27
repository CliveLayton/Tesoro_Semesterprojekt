using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    /// <summary>
    /// radius of the ledge detection circle
    /// </summary>
    [SerializeField] private float radius;

    /// <summary>
    /// ground layer
    /// </summary>
    [SerializeField] private LayerMask groundLayer;

    /// <summary>
    /// playercontroller script
    /// </summary>
    [SerializeField] private PlayerController player;

    /// <summary>
    /// position of the ledge detection circle
    /// </summary>
    [SerializeField] private Vector3 ledgeDetectionPos;

    /// <summary>
    /// bool for checking walls
    /// </summary>
    public bool canDetected;

    /// <summary>
    /// set the ledge detection with a circle
    /// </summary>
    private void Update()
    {
        transform.eulerAngles = player.leftMovement ? new Vector3(0, 180, 0) : new Vector3(0, 0, 0);
        ledgeDetectionPos.x = player.leftMovement ? -0.387f : 0.387f;
        if (canDetected)
        {
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position + ledgeDetectionPos, radius, groundLayer);
        }

    }

    /// <summary>
    /// if collide with a ground set canDetected on false
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetected = false;
        }
    }

    /// <summary>
    /// if not collide with a ground set canDetected to true
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetected = true;
        }
    }

    /// <summary>
    /// draws a wiresphere to visualize the ledge detection check
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + ledgeDetectionPos, radius);
    }
}
