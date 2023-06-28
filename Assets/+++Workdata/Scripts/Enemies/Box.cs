using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public ParticleSystem particle;
    public void DestroyMe()
    {
        Destroy(particle.gameObject);
        Destroy(gameObject);
    }
}
