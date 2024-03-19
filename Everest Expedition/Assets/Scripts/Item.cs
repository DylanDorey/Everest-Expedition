using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: []
 * Last Updated: [03/18/2024]
 * [Base class for all items the player can pickup/use]
 */
public class Item : MonoBehaviour
{
    public Sprite itemSprite;
    public float itemHealAmount;
    public GameObject groundPickup;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().PickupItem(gameObject);
        }
    }
}
