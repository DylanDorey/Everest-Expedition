using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [03/18/2024]
 * [Base class for all items the player can pickup/use]
 */

public class Item : MonoBehaviour
{
    //the items parameters
    public Sprite itemImage;
    public float itemHealAmount;

    public void OnCollisionEnter(Collision collision)
    {
        //if the game object that collides with the item is tagged "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            //Access the player controller class and call the, PickupItem method, passing in the item gameobject as the item to pick up
            collision.gameObject.GetComponent<InventoryManager>().PickupItem(gameObject);

            //Destroy the game object
            Destroy(gameObject);
        }
    }
}
