using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameInput inputActions;

    public GameObject pauseMenu;
    public GameObject optionMenu;
    public GameObject player;

    private void Awake()
    {
        inputActions = new GameInput();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        player.SetActive(true);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.UI.PauseGame.performed += PauseGame;
        inputActions.UI.PauseGame.canceled += PauseGame;
    }

    private void OnDisable()
    {
        inputActions.Disable();

        inputActions.UI.PauseGame.performed -= PauseGame;
        inputActions.UI.PauseGame.canceled -= PauseGame;
    }

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
