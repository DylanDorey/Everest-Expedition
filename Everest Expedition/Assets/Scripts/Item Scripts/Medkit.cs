using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [03/19/2024]
 * [A medkit that the player can pickup, and bring health back to full]
 */

public class Medkit : Item, IItemBehavior
{
    /// <summary>
    /// heals the players health back to full
    /// </summary>
    /// /// <param name="playerData"> the playerData script that is being affected </param>
    public void UseItem(PlayerData playerData)
    {
        //set the itemHealAmount to the amount of health the player is missing
        itemHealAmount = 100 - playerData.playerHealth;

        //add the itemHealAmount to the player's health to bring them back to full health
        playerData.playerHealth += itemHealAmount;
    }
}
