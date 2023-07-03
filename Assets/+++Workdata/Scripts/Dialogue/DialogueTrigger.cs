using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class DialogueTrigger : MonoBehaviour
{
    #region Variables
    [Header("Visual Cue")]
    //link to the visual cue object
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    //link to the inkJson file
    [SerializeField] private TextAsset inkJSON;

    /// <summary>
    /// bool to check if the player is in range to interact
    /// </summary>
    private bool playerInRange;

    /// <summary>
    /// Input Action Asset
    /// </summary>
    private GameInput inputActions;

    /// <summary>
    /// bool for check if the player presses the interact button
    /// </summary>
    private bool isInteracting;

    /// <summary>
    /// link to the player
    /// </summary>
    public GameObject player;

    /// <summary>
    /// link to the player rigidbody
    /// </summary>
    private Rigidbody2D playerRb;

    #endregion

    #region Functions

    /// <summary>
    /// get components, set playerInRange to false and disables the visual cue
    /// </summary>
    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);

        inputActions = new GameInput();
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //if player is in range and dialogue is not playing enable visual cue
        if(playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            //if the player interacts in the range enter dialogue mode with the linked inkJson file on the script
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

    /// <summary>
    /// enable input action for interact
    /// </summary>
    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Interact.performed += Interact;
        inputActions.Player.Interact.canceled += Interact;
    }

    /// <summary>
    /// disable input action for interact
    /// </summary>
    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Interact.performed -= Interact;
        inputActions.Player.Interact.canceled -= Interact;
    }

    /// <summary>
    /// gets the input of the player
    /// </summary>
    /// <param name="context"></param>
    void Interact(InputAction.CallbackContext context)
    {
        isInteracting = context.performed;
    }

    #endregion
}
