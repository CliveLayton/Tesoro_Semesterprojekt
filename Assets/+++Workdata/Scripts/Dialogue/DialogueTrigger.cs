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
    [SerializeField] private string inkPath;

    private DialogueManager dialogueManager;

    /// <summary>
    /// link to the player
    /// </summary>
    public GameObject player;

    #endregion

    #region Functions

    /// <summary>
    /// get components, set playerInRange to false and disables the visual cue
    /// </summary>
    private void Awake()
    {
        visualCue.SetActive(false);

        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            //if player is in range and dialogue is not playing enable visual cue
            if (!dialogueManager.dialogueIsPlaying)
            {
                visualCue.SetActive(true);

                dialogueManager.startDialogue = true;
                dialogueManager.dialoguePath = inkPath;            
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dialogueManager.startDialogue = false;
            visualCue.SetActive(false);
        }
    }

    #endregion
}
