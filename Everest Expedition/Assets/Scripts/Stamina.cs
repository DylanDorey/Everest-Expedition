using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [03/19/2024]
 * [Stamina that the player can pickup, and heal thirst back to full]
 */

public class Stamina : Item, IItemBehavior
{
    /// <summary>
    /// heals the players thirst back to full
    /// </summary>
    /// /// <param name="playerData"> the playerData script that is being affected </param>
    public void UseItem(PlayerData playerData)
    {
        //set the itemHealAmount to the amount of health the player is missing
        itemHealAmount = 100 - playerData.playerThirst;

        //add the itemHealAmount to the player's health to bring them back to full health
        playerData.playerThirst += itemHealAmount;
    }
}
