using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Ambience")]

    //audio reference for ambience music 
    [field: SerializeField] public EventReference ambience { get; private set; }

    [field: Header("Music")]

    //audio reference for main music in the background
    [field: SerializeField] public EventReference music { get; private set; }

    [field: Header("Player SFX")]

    //audio reference for playerfootsteps sound
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }

    [field: SerializeField] public EventReference playerSprint { get; private set; }

    //audio reference for figurejump sound
    [field: SerializeField] public EventReference figureJump { get; private set; }

    //audio reference for landing sound
    [field: SerializeField] public EventReference landing { get; private set; }

    //audio reference for blob stuck to wall
    [field: SerializeField] public EventReference blobWall { get; private set; }

    //audio reference for changing state
    [field: SerializeField] public EventReference changeStateSound { get; private set; }

    //audio reference for player dying
    [field: SerializeField] public EventReference playerDying { get; private set; }

    //audio reference for player get hurt
    [field: SerializeField] public EventReference playerHurt { get; private set; }

    //audio reference for game over sound
    [field: SerializeField] public EventReference gameOver { get; private set; }

    [field: Header("Enemy SFX")]

    //audio reference for enemygethit sound
    [field: SerializeField] public EventReference enemyGetHit { get; private set; }

    //audio reference for enemyfootsteps sound
    [field: SerializeField] public EventReference enemyFootsteps { get; private set; }

    [field: Header("Environment")]

    //audio reference for jumping on a shroom
    [field: SerializeField] public EventReference shroomSound { get; private set; }


    [field: Header("UI SFX")]

    //audio reference for startgamesound sound
    [field: SerializeField] public EventReference startGameSound { get; private set; }

    //audio reference for button hovered sound
    [field: SerializeField] public EventReference buttonHovered { get; private set; }

    //audio reference for button selected sound
    [field: SerializeField] public EventReference buttonSelected { get; private set; }

    /// <summary>
    /// static FMODEvents variable 
    /// </summary>
    public static FMODEvents instance { get; private set; }

    /// <summary>
    /// check if there are more than 1 FMODEvents script in the scene
    /// </summary>
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}
