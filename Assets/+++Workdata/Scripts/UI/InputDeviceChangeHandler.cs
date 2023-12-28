using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputDeviceChangeHandler : MonoBehaviour
{
    /// <summary>
    /// sprite renderer for move image
    /// </summary>
    [SerializeField] private SpriteRenderer move;

    /// <summary>
    /// sprite renderer for interact image
    /// </summary>
    [SerializeField] private SpriteRenderer interact;

    /// <summary>
    /// sprite renderer for crouch image
    /// </summary>
    [SerializeField] private SpriteRenderer crouch;

    /// <summary>
    /// sprite renderer for climb image
    /// </summary>
    [SerializeField] private SpriteRenderer climb;

    /// <summary>
    /// animator for dialogueicon
    /// </summary>
    [SerializeField] private Animator dialogue;

    /// <summary>
    /// firstjump trigger
    /// </summary>
    [SerializeField] private GameObject firstJump;

    /// <summary>
    /// sprinting trigger
    /// </summary>
    [SerializeField] private GameObject sprinting;

    /// <summary>
    /// crouching trigger
    /// </summary>
    [SerializeField] private GameObject crouching;

    /// <summary>
    /// firstjump for gamepad trigger
    /// </summary>
    [SerializeField] private GameObject firstJumpGamepad;

    /// <summary>
    /// sprinting for gamepad trigger
    /// </summary>
    [SerializeField] private GameObject sprintingGamepad;

    /// <summary>
    /// crouching for gamepad trigger
    /// </summary>
    [SerializeField] private GameObject crouchingGamepad;


    /// <summary>
    /// sprite for move with keyboard
    /// </summary>
    [SerializeField] private Sprite keyboardImagemove;

    /// <summary>
    /// sprite for move with gamepad
    /// </summary>
    [SerializeField] private Sprite gamepadImagemove;

    /// <summary>
    /// sprite for interact with keyboard
    /// </summary>
    [SerializeField] private Sprite keyboardImageinteract;

    /// <summary>
    /// sprite for interact with gamepad
    /// </summary>
    [SerializeField] private Sprite gamepadImageinteract;

    /// <summary>
    /// sprite for crouch with keyboard
    /// </summary>
    [SerializeField] private Sprite keyboardImagecrouch;

    /// <summary>
    /// sprite for crouch with gamepad
    /// </summary>
    [SerializeField] private Sprite gamepadImagecrouch;

    /// <summary>
    /// sprite for climb with keyboard
    /// </summary>
    [SerializeField] private Sprite keyboardImageclimb;

    /// <summary>
    /// sprite for climb with gamepad
    /// </summary>
    [SerializeField] private Sprite gamepadImageclimb;

    /// <summary>
    /// set sprites to state of device input
    /// </summary>
    /// <param name="isKeyboardUsing">bool for if keyboard is currently using</param>
    public void SetImageState(bool isKeyboardUsing)
    {
        if (isKeyboardUsing)
        {
            move.sprite = keyboardImagemove;
            interact.sprite = keyboardImageinteract;
            crouch.sprite = keyboardImagecrouch;
            climb.sprite = keyboardImageclimb;
            SetTriggerZonesForKeyboard();           
        }
        else if (!isKeyboardUsing)
        {
            move.sprite = gamepadImagemove;
            interact.sprite = gamepadImageinteract;
            crouch.sprite = gamepadImagecrouch;
            climb.sprite = gamepadImageclimb;
            SetTriggerForGamepad();
        }

        if (dialogue.gameObject.activeSelf)
        {
            ChangeContinueButtonAnim(isKeyboardUsing);
        }
    }

    public void ChangeContinueButtonAnim(bool isKeyboardUsing)
    {
        dialogue.SetBool("Keyboard", isKeyboardUsing);
    }

    /// <summary>
    /// sets triggerzones active for keyboard input
    /// </summary>
    private void SetTriggerZonesForKeyboard()
    {
        firstJump.SetActive(true);
        sprinting.SetActive(true);
        crouching.SetActive(true);
        firstJumpGamepad.SetActive(false);
        sprintingGamepad.SetActive(false);
        crouchingGamepad.SetActive(false);
    }

    /// <summary>
    /// sets triggerzones active for gamepad input
    /// </summary>
    private void SetTriggerForGamepad()
    {
        firstJump.SetActive(false);
        sprinting.SetActive(false);
        crouching.SetActive(false);
        firstJumpGamepad.SetActive(true);
        sprintingGamepad.SetActive(true);
        crouchingGamepad.SetActive(true);
    }
}
