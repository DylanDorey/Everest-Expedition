using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: []
 * Last Updated: [03/19/2024]
 * [Water that the player can pickup, and have unlimited thirst for a specified duration]
 */

public class Water : Item
{
    public float unlimitedDuration = 10f;

    public void StartUnlimitedThirst()
    {
        StartCoroutine(PlayerData.Instance.UnlimitedThirst(unlimitedDuration));
    }
}
