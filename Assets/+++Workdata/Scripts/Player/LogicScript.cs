using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogicScript : MonoBehaviour
{
    /// <summary>
    /// current lifepoints of the player
    /// </summary>
    public int lifePoints;

    /// <summary>
    /// displays current lifepoints in text
    /// </summary>
    public TextMeshProUGUI lifePointsCounter;

    /// <summary>
    /// set lifepoints back to 5 if they are 0 and set the current lifepoints to the text
    /// </summary>
    private void FixedUpdate()
    {
        if(lifePoints == 0)
        {
            lifePoints = 5;
        }
        lifePointsCounter.text = lifePoints.ToString();
    }

    /// <summary>
    /// divide current lifepoints with the number 
    /// </summary>
    /// <param name="scoreToReduce">number to divide from current lifepoints</param>
    public void ReduceScore(int scoreToReduce)
    {
        if(lifePoints > 0)
        {
            lifePoints -= scoreToReduce;
        }
    }
}
