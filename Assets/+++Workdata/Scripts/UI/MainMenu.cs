using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //link to a item in the music area enum
    [SerializeField] private MusicArea area;

    /// <summary>
    /// sets the music area at the start of the scene
    /// </summary>
    private void Start()
    {
        AudioManager.instance.SetMusicArea(area);
    }

    /// <summary>
    /// play the audio for pressing the startgame button and start coroutine for entering the game
    /// </summary>
    public void PlayGame()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.startGameSound, this.transform.position);
        StartCoroutine(GoToGame());
    }

    /// <summary>
    /// quit the application
    /// </summary>
    public void QuitGame()
    {
        print("Quit!");
        Application.Quit();
    }

    /// <summary>
    /// load the level scene after 0.5 seconds
    /// </summary>
    /// <returns>wait for 0.5 seconds</returns>
    private IEnumerator GoToGame()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
