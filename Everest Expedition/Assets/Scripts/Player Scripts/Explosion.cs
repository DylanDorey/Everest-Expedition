using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [04/29/2024]
 * [Plays the particle effect when the player switches states]
 */

public class Explode : MonoBehaviour
{
    private void Start()
    {
        //play the explosion particle system
        GetComponent<ParticleSystem>().Play();
    }
}
