using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPickUpSystem : MonoBehaviour
{
    [SerializeField] public InventorySO inventoryData;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType().ToString() == "UnityEngine.CircleCollider2D")
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                item.hasSpaceToBePickUp = false;
                int reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
                if (reminder == 0)
                {
                    item.hasSpaceToBePickUp = true;
                    StartCoroutine(item.AnimateItemPickup());
                }
                else 
                {
                    item.Quantity = reminder;
                    item.hasSpaceToBePickUp = false;

                }
            }
        }
    }
}
