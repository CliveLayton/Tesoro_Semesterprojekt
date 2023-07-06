using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipMoveOnWall : MonoBehaviour
{
    [SerializeField] private SpriteRenderer moveSprite;

    [SerializeField] private GameObject moveText;

    private PlayerController player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && player.climbWall == false)
        {
            moveSprite.enabled = true;
            moveText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        moveSprite.enabled = false;
        moveText.SetActive(false);
    }
}
