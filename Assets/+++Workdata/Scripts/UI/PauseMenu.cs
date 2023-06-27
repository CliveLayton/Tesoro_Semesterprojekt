using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// Game Input Asset
    /// </summary>
    private GameInput inputActions;

    /// <summary>
    /// pause menu object
    /// </summary>
    public GameObject pauseMenu;

    /// <summary>
    /// option menu object
    /// </summary>
    public GameObject optionMenu;

    /// <summary>
    /// player object
    /// </summary>
    public GameObject player;

    /// <summary>
    /// set the timescale of the scene to 1 and disable pause menu
    /// </summary>
    private void Awake()
    {
        inputActions = new GameInput();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// set the timesclae of the scene to 1 and enable the player
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        player.SetActive(true);
    }

    /// <summary>
    /// set the time scale back to 1 and load main menu
    /// </summary>
    public void QuitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// enable input actions for the pause menu
    /// </summary>
    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.UI.PauseGame.performed += PauseGame;
        inputActions.UI.PauseGame.canceled += PauseGame;
    }

    /// <summary>
    /// disable input actions for the pause menu
    /// </summary>
    private void OnDisable()
    {
        inputActions.Disable();

        inputActions.UI.PauseGame.performed -= PauseGame;
        inputActions.UI.PauseGame.canceled -= PauseGame;
    }

    /// <summary>
    /// set time scale to 0, disable player and enable pause menu if pressing pause button
    /// set time scale to 1, enable player and disable pause menu if pressing pause button and pause menu is already active
    /// </summary>
    /// <param name="context"></param>
    void PauseGame(InputAction.CallbackContext context)
    {
        if(context.performed && (pauseMenu.activeSelf == false))
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            optionMenu.SetActive(false);
            player.SetActive(false);
        }
        else if(context.performed && (pauseMenu.activeSelf == true))
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            player.SetActive(true);
        }
    }
}
