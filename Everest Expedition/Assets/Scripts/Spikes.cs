using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public int damageAmount = 10; //Damage to the player

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            
        }
    }
}
