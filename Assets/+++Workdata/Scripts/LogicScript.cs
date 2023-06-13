using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicScript : MonoBehaviour
{
    public int lifePoints = 2;

    private void FixedUpdate()
    {
        if(lifePoints == 0)
        {
            lifePoints -= 1;
        }
        else if(lifePoints <=-1)
        {
            lifePoints = 2;
        }
    }

    public void ReduceScore(int scoreToReduce)
    {
        if(lifePoints > 0)
        {
            lifePoints -= scoreToReduce;
        }
    }
}
