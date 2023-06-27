using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }
    [field: SerializeField] public EventReference blobJump { get; private set; }

    [field: Header("Enemy SFX")]
    [field: SerializeField] public EventReference enemyGetHit { get; private set; }
    [field: SerializeField] public EventReference enemyFootsteps { get; private set; }

    [field: Header("UI SFX")]
    [field: SerializeField] public EventReference startGameSound { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}
