using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using UnityEngine.UI;

public class VCAController : MonoBehaviour
{
    private VCA vCAController;

    public string vCAName;

    [SerializeField] private float vCAVolume;

    private Slider slider;

    private void Start()
    {
        vCAController = FMODUnity.RuntimeManager.GetVCA("vca:/" + vCAName);
        slider = GetComponent<Slider>();
        vCAController.getVolume(out vCAVolume);
        slider.value = vCAVolume;
    }

    public void SetVolume(float volume)
    {
        vCAController.setVolume(volume);
        vCAController.getVolume(out vCAVolume);
    }
}
