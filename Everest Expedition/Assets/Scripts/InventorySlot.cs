using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Author: []
 * Last Updated: [03/19/2024]
 * [Store information regarding an inventories slot image and the game object in the slot]
 */

public class InventorySlot : MonoBehaviour
{
    public Sprite slotImage;
    public GameObject slotItem;

    public void SetInventoryImage()
    {
        GetComponent<Image>().sprite = slotImage;
    }
}
