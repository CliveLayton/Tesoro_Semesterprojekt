using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class SettingsMenu : MonoBehaviour
{
    /// <summary>
    /// List of ints for the width screensize
    /// </summary>
    List<int> widths = new List<int>() { 1280, 1920, 2560 };

    /// <summary>
    /// List of ints for the height screensize
    /// </summary>
    List<int> heights = new List<int>() { 720, 1200, 1600 };

    /// <summary>
    /// Key to save selected width in playerprefs
    /// </summary>
    private const string resolutionWidth_KEY = "ResolutionWidth";

    /// <summary>
    /// Key to save selected height in playerprefs
    /// </summary>
    private const string resolutionHeight_KEY = "ResolutionHeigth";

    /// <summary>
    /// Key to save selected index of the lists in playerprefs
    /// </summary>
    private const string resolutionIndex_KEY = "ResolutionIndex";

    /// <summary>
    /// Key to save bool for fullscreen in playerprefs
    /// </summary>
    private const string fullScreen_KEY = "FullScreen";

    /// <summary>
    /// link to the Toggle component for fullscreen
    /// </summary>
    public Toggle fullScreenToggle;

    /// <summary>
    /// link to the dropdown component for resolutions
    /// </summary>
    public TMP_Dropdown resolutionDropdown;

    /// <summary>
    /// loads the settings saved in playerprefs
    /// </summary>
    private void Start()
    {
        LoadSettings();
    }

    /// <summary>
    /// sets resolution and fullscreen to saved variables in playerprefs
    /// </summary>
    private void LoadSettings()
    {
        resolutionDropdown.value = PlayerPrefs.GetInt(resolutionIndex_KEY, 2);
        fullScreenToggle.isOn = PlayerPrefs.GetInt(fullScreen_KEY, Screen.fullScreen ? 1 : 0) > 0;
        Screen.SetResolution(PlayerPrefs.GetInt(resolutionWidth_KEY, 1920),
            PlayerPrefs.GetInt(resolutionHeight_KEY, 1200),
            fullScreenToggle.isOn);
    }

    /// <summary>
    /// sets the screen resolution to the int in the list
    /// </summary>
    /// <param name="resolutionIndex">index for lists</param>
    public void SetResolutions(int resolutionIndex)
    {
        int width = widths[resolutionIndex];
        int height = heights[resolutionIndex];
        Screen.SetResolution(width, height, Screen.fullScreen);
        PlayerPrefs.SetInt(resolutionWidth_KEY, width);
        PlayerPrefs.SetInt(resolutionHeight_KEY, height);
        PlayerPrefs.SetInt(resolutionIndex_KEY, resolutionIndex);
    }

    /// <summary>
    /// sets Fullscreen to given bool
    /// </summary>
    /// <param name="isFullscreen">bool for infullscreen</param>
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt(fullScreen_KEY, isFullscreen ? 1 : 0);
    }
}
