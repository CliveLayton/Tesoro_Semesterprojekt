using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    /// <summary>
    /// current health
    /// </summary>
    public int health;

    /// <summary>
    /// number of current hearts available
    /// </summary>
    public int numOfHearts;

    /// <summary>
    /// link to the LogicScript script
    /// </summary>
    public LogicScript logic;

    /// <summary>
    /// array of how much heart images are given
    /// </summary>
    public Image[] hearts;

    private void Awake()
    {
        logic = GameObject.FindGameObjectWithTag("Counter").GetComponent<LogicScript>();
    }

    private void FixedUpdate()
    {
        if(health > numOfHearts)
        {
            health = numOfHearts;
        }

        if(health == 0)
        {
            health = 3;
        }

        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    /// <summary>
    /// divide current health with the number
    /// </summary>
    /// <param name="reducePoints">number to divide from current health</param>
    public void ReduceHealth(int reducePoints)
    {
        if(health > 0)
        {
            health -= reducePoints;
        }
    }
}
