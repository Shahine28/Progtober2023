using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPickUpSystem : MonoBehaviour
{
    [SerializeField] public InventorySO inventoryData;

    public void HasSpaceToStoreItem(GameObject obj)
    {
        Item item = obj.GetComponent<Item>();
        if (inventoryData.IsInventoryFull())
        {
            item.hasSpaceToBePickUp = false;
        }
        else
        {
            item.hasSpaceToBePickUp = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType().ToString() == "UnityEngine.CircleCollider2D")
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                int reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
                if (reminder == 0)
                {
                    Debug.Log("Truman Show");
                    StartCoroutine(item.AnimateItemPickup());
                }
                else 
                {
                    item.Quantity = reminder;
                }
            }
        }
    }
}
