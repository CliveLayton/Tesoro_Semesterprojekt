using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public Transform[] spawnPoint;

    public int spawnIndex;

    private Transform player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<Transform>();
            StartCoroutine(ResetPlayer());
        }
    }

    public void SetSpawnIndex(int newSpawnIndex)
    {
        spawnIndex = newSpawnIndex;
    }

    private IEnumerator ResetPlayer()
    {
        yield return new WaitForSeconds(1f);
        player.position = spawnPoint[spawnIndex].position;
    }
}
