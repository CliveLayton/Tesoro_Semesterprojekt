using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{
    [Header("Area")]
    //link to a item in the music area enum
    [SerializeField] private MusicArea area;

    /// <summary>
    /// if player enters the trigger sets the music area 
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.SetMusicArea(area);
        }
    }
}
