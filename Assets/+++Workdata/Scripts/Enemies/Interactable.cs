using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    /// <summary>
    /// bool to check if the player is in range
    /// </summary>
    private bool isInRange;

    /// <summary>
    /// Unity Event variable
    /// </summary>
    public UnityEvent interactAction;

    /// <summary>
    /// Input Action Asset
    /// </summary>
    private GameInput inputActions;

    /// <summary>
    /// get components
    /// </summary>
    private void Awake()
    {
        inputActions = new GameInput();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
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
    /// get the input of the player and perform the Unity Event
    /// </summary>
    /// <param name="context"></param>
    private void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && isInRange)
        {
            interactAction.Invoke();
        }
    }
}
