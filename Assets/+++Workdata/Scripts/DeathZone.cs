using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoint;

    public int spawnIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = spawnPoint[spawnIndex].position;
        }
    }

    public void SetSpawnIndex(int newSpawnIndex)
    {
        spawnIndex = newSpawnIndex;
    }
}
