using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    /// <summary>
    /// link to the particle system on the object
    /// </summary>
    public ParticleSystem particle;

    /// <summary>
    /// Destroys Gameobject and particle system
    /// </summary>
    public void DestroyMe()
    {
        Destroy(particle.gameObject);
        Destroy(gameObject);
    }
}
