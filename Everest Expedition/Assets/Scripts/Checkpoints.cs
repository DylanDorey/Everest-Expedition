using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: []
 * Last Updated: [04/02/2024]
 * [Grants the player an item when they reach a checkpoint]
 */

public class Checkpoints : MonoBehaviour
{
    //the awards that the player can receive
    public GameObject[] itemsToAward;

    /// <summary>
    /// Grants the player a random item when they reach the checkpoint
    /// </summary>
    public void GrantReward()
    {
        //random index for the item to be awarded
        int randomAwardIndex = Random.Range(0, itemsToAward.Length);
        
        //random index to determine the directional offset of the items spawn point
        int randomOffsetBool = Random.Range(0, 2);

        //the position offset value of the item when it spawns in relative to the checkpoints position
        float randomSpawnOffset = Random.Range(1f, 2f);

        //if the random 'bool' is 0
        if (randomOffsetBool == 0)
        {
            //spawn the random item opposite of the random offsset
            GameObject item = Instantiate(itemsToAward[randomAwardIndex], new Vector3(transform.position.x - randomSpawnOffset, transform.position.y, transform.position.z - randomSpawnOffset), Quaternion.identity);
            item.name = itemsToAward[randomAwardIndex].name;
        }
        else
        {
            //otherwise spawn the random item with the random offset
            GameObject item = Instantiate(itemsToAward[randomAwardIndex], new Vector3(transform.position.x + randomSpawnOffset, transform.position.y, transform.position.z + randomSpawnOffset), Quaternion.identity);
            item.name = itemsToAward[randomAwardIndex].name;
        }
    }
}
