using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingEnemy : MonoBehaviour
{
    /// <summary>
    /// Destroys the gameobject
    /// </summary>
    public void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }
}
