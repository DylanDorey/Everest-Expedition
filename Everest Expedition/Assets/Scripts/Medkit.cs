using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: []
 * Last Updated: [03/18/2024]
 * [A medkit that the player can pickup, and bring health back to full]
 */

public class Medkit : Item
{
    /// <summary>
    /// heals the players health back to full
    /// </summary>
    public void HealPlayer()
    {
        PlayerData.Instance.playerHealth += itemHealAmount;
    }
}
