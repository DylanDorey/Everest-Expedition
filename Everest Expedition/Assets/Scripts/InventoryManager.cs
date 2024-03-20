using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Author: []
 * Last Updated: [03/19/2024]
 * [Manages the players inventory]
 */

public class InventoryManager : MonoBehaviour
{
    //singelton for InventoryManager
    private static InventoryManager _instance;
    public static InventoryManager Instance { get { return _instance; } }

    public GameObject inventorySlots;

    public GameObject[] itemsArray;

    private void Awake()
    {
        //if _instance contains something and it isn't this
        if (_instance != null && _instance != this)
        {
            //Destroy it
            Destroy(this.gameObject);
        }
        else
        {
            //otherwise set this to _instance
            _instance = this;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    public void PickupItem(GameObject item)
    {
        for (int index = 0; index < inventorySlots.transform.childCount; index++)
        {
            if (inventorySlots.transform.GetChild(index).gameObject.GetComponent<Image>().sprite == null)
            {

                inventorySlots.transform.GetChild(index).gameObject.GetComponent<InventorySlot>().slotImage = item.GetComponent<Item>().itemImage;

                //set the inventory slot image to the game object's image that the player just picked up
                inventorySlots.transform.GetChild(index).gameObject.GetComponent<InventorySlot>().SetInventoryImage();
                
                //set the game object in the inventory slot to the gameobject the player just picked up
                for (int index2 = 0; index2 < itemsArray.Length; index2++)
                {
                    if (itemsArray[index2].gameObject == item.gameObject)
                    {
                        inventorySlots.transform.GetChild(index).gameObject.GetComponent<InventorySlot>().slotItem = itemsArray[index2];
                    }
                }

                break;
            }
        }
    }
}
