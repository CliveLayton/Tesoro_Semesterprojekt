using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputDeviceChangeHandler : MonoBehaviour
{
    private SpriteRenderer icon;

    [SerializeField] private Sprite keyboardImage;

    [SerializeField] private Sprite gamepadImage;

    private void Awake()
    {
        icon = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        InputUser.onChange += OnInputDeviceChange;
    }

    private void OnDisable()
    {
        InputUser.onChange -= OnInputDeviceChange;
    }

    private void OnInputDeviceChange(InputUser user, InputUserChange change, InputDevice device)
    {
        if(change == InputUserChange.ControlSchemeChanged)
        {
            UpdateButtonImage(user.controlScheme.Value.name);
        }
    }

    private void UpdateButtonImage(string schemeName)
    {
        if (schemeName.Equals("Gamepad"))
        {
            icon.sprite = gamepadImage;
        }
        else
        {
            icon.sprite = keyboardImage;
        }
    }
}
