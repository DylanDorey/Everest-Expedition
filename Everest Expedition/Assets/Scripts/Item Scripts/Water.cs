using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [03/19/2024]
 * [Water that the player can pickup, and have unlimited thirst for a specified duration]
 */

public class Water : Item, IItemBehavior
{
    //the duration for unlimited thirst
    readonly float unlimitedDuration = 10f;

    /// <summary>
    /// gives the player unlimited thirst for a desired duration
    /// </summary>
    /// <param name="playerData"> the playerData script that is being affected </param>
    public void UseItem(PlayerData playerData)
    {
        //Start the unlimited thirst coroutine in the player data class
        playerData.StartCoroutine(playerData.UnlimitedThirst(unlimitedDuration));
    }
}
