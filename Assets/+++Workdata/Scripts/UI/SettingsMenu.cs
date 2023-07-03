using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class SettingsMenu : MonoBehaviour
{
    List<int> widths = new List<int>() { 640, 1280, 1920, 2560 };

    List<int> heights = new List<int>() { 480, 720, 1200, 1600 };

    private const string resolutionWidth_KEY = "ResolutionWidth";
    private const string resolutionHeight_KEY = "ResolutionHeigth";
    private const string resolutionIndex_KEY = "ResolutionIndex";
    private const string fullScreen_KEY = "FullScreen";

    public Toggle fullScreenToggle;
    public TMP_Dropdown resolutionDropdown;

    private void Start()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        resolutionDropdown.value = PlayerPrefs.GetInt(resolutionIndex_KEY, 2);
        fullScreenToggle.isOn = PlayerPrefs.GetInt(fullScreen_KEY, Screen.fullScreen ? 1 : 0) > 0;
        Screen.SetResolution(PlayerPrefs.GetInt(resolutionWidth_KEY, 1920),
            PlayerPrefs.GetInt(resolutionHeight_KEY, 1200),
            fullScreenToggle.isOn);
    }

    public void SetResolutions(int resolutionIndex)
    {
        int width = widths[resolutionIndex];
        int height = heights[resolutionIndex];
        Screen.SetResolution(width, height, Screen.fullScreen);
        PlayerPrefs.SetInt(resolutionWidth_KEY, width);
        PlayerPrefs.SetInt(resolutionHeight_KEY, height);
        PlayerPrefs.SetInt(resolutionIndex_KEY, resolutionIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt(fullScreen_KEY, isFullscreen ? 1 : 0);
    }
}
