using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    private GameInput inputActions;
    private bool isInteracting;

    public GameObject player;
    private Rigidbody2D playerRb;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);

        inputActions = new GameInput();
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if(isInteracting && (playerRb.velocity.x == 0))
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Interact.performed += Interact;
        inputActions.Player.Interact.canceled += Interact;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Interact.performed -= Interact;
        inputActions.Player.Interact.canceled -= Interact;
    }

    void Interact(InputAction.CallbackContext context)
    {
        isInteracting = context.performed;
    }
}
