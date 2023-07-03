using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    /// <summary>
    /// array of tramsforms for different spawnpoints
    /// </summary>
    public Transform[] spawnPoint;

    /// <summary>
    /// index for the transform array
    /// </summary>
    public int spawnIndex;

    /// <summary>
    /// transform of the player
    /// </summary>
    private Transform player;

    /// <summary>
    /// if the player enters the triggerzone reset him to the last spawnpoint
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<Transform>();
            StartCoroutine(ResetPlayer());
        }
    }

    /// <summary>
    /// sets the spawnindex of the transform array
    /// </summary>
    /// <param name="newSpawnIndex">new int</param>
    public void SetSpawnIndex(int newSpawnIndex)
    {
        spawnIndex = newSpawnIndex;
    }

    /// <summary>
    /// wait for 1 second and sets player position to a spawnpoint in the transform array
    /// </summary>
    /// <returns>waits for 1 second</returns>
    private IEnumerator ResetPlayer()
    {
        yield return new WaitForSeconds(1f);
        player.position = spawnPoint[spawnIndex].position;
    }
}
