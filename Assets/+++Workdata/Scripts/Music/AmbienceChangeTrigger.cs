using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceChangeTrigger : MonoBehaviour
{
    [Header("Parameter Change")]
    //name of the paramter we want to change
    [SerializeField] private string parameterName;

    //the value we want to set to the paramter
    [SerializeField] private float parameterValue;

    /// <summary>
    /// if player enters the trigger, sets the paramtervalue of the parameter
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.SetAmbienceParameter(parameterName, parameterValue);
        }
    }
}
