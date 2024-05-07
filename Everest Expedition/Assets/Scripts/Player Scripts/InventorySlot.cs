using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [03/19/2024]
 * [Store information regarding an inventories slot image and the items behavior in the slot]
 */

public class InventorySlot : MonoBehaviour
{
    //inventory slot parameters
    public bool hasItem;
    public Sprite slotImage;
    public IItemBehavior itemUse;

    /// <summary>
    /// Sets the inventrory image to what is in the slot image
    /// </summary>
    public void SetInventoryImage()
    {
        //get the sprite component of the game object and set it to the current slot image
        GetComponent<Image>().sprite = slotImage;
    }
}
