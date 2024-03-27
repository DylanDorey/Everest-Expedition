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

    //inventory slot reference
    public GameObject inventorySlots;

    //Item prefabs
    public GameObject waterPrefab;
    public GameObject medkitPrefab;
    public GameObject staminaPrefab;
    public GameObject[] itemsArray;

    //Item abilities/elements
    public IItemBehavior[] itemAbilities;
    public Water waterAbility;
    public Medkit medkitAbility;
    public Stamina staminaAbility;

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

        //arrays for the item game objects and the items abilities/uses
        itemsArray = new GameObject[] { waterPrefab, medkitPrefab, staminaPrefab };
        itemAbilities = new IItemBehavior[] { waterPrefab.GetComponent<Water>(), medkitPrefab.GetComponent<Medkit>(), staminaPrefab.GetComponent<Stamina>() };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"> the item that is being picked up </param>
    public void PickupItem(GameObject item)
    {
        for (int index = 0; index < inventorySlots.transform.childCount; index++)
        {
            //reference to inventory slot script
            InventorySlot inventorySlot = inventorySlots.transform.GetChild(index).gameObject.GetComponent<InventorySlot>();

            //if the slot does not have an item in it
            if (inventorySlot.hasItem == false)
            {
                //get the inventory slot image of the game object's image that the player just picked up
                inventorySlot.slotImage = item.GetComponent<Item>().itemImage;

                //set the inventory slot image to the game object's image that the player just picked up
                inventorySlot.SetInventoryImage();

                //set the game object in the inventory slot to the gameobject the player just picked up
                for (int index2 = 0; index2 < itemsArray.Length; index2++)
                {
                    //if the items name matches the items in the items array
                    if (item.name == itemsArray[index2].name)
                    {
                        //assign the items Use interface function to the inventory slot
                        inventorySlot.itemUse = itemAbilities[index2];
                        break;
                    }
                }

                //set hasItem to true for the index slot
                inventorySlot.hasItem = true;

                break;
            } //if all the slots are full
            else if (inventorySlot.hasItem && index == 4)
            {
                //DISPLAY ERROR MESSAGE SAYING INVENTORY IS FULL
            }
        }
    }

    /// <summary>
    /// Removes the item from the player's inventory when it is used
    /// </summary>
    /// <param name="slotIndex"> the slot the item is being removed from </param>
    public void RemoveItemOnUse(int slotIndex)
    {
        //reference to slot image component and inventory slot script
        Image slotImage = inventorySlots.transform.GetChild(slotIndex).gameObject.GetComponent<Image>();
        InventorySlot slot = inventorySlots.transform.GetChild(slotIndex).gameObject.GetComponent<InventorySlot>();

        //set all used/filled values to empty/false
        slotImage.sprite = null;
        slot.slotImage = null;
        slot.itemUse = null;
        slot.hasItem = false;
    }

}
