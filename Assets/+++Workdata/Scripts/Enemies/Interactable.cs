using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    private bool isInRange;
    public UnityEvent interactAction;

    private GameInput inputActions;

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

    private void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && isInRange)
        {
            interactAction.Invoke();
        }
    }
}
