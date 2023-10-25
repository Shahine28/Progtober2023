using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPickUpSystem : MonoBehaviour
{
    [SerializeField] public InventorySO mainInventoryData;
    [SerializeField] public InventorySO toolbarInventoryData;

    public void HasSpaceToStoreItem(GameObject obj)
    {
        Item item = obj.GetComponent<Item>();
        if (mainInventoryData.IsInventoryFull())
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
                AddItem(item);
            }
        }
    }

    public void AddItem(Item item)
    {
        int reminder;
        if (!toolbarInventoryData.IsInventoryFull())
        {
            reminder = toolbarInventoryData.AddItem(item.InventoryItem, item.Quantity);
            if (reminder == 0)
            {
                StartCoroutine(item.AnimateItemPickup());
            }
            else
            {
                item.Quantity = reminder;
            }
        }
        else
        {
            reminder = mainInventoryData.AddItem(item.InventoryItem, item.Quantity);
            if (reminder == 0)
            {
                StartCoroutine(item.AnimateItemPickup());
            }
            else
            {
                item.Quantity = reminder;
            }
        }
    }
}
