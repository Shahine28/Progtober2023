using Inventory.Model;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UI_Shop : MonoBehaviour
{
    [SerializeField] private Transform contentPanel;
    [SerializeField] private UI_ShopItem shopItemTemplate;
    public List<ItemShop> itemSold = new List<ItemShop>();
    void Start()
    {
        ClearShop();
        for (int i = 0; i < itemSold.Count; i++)
        {
            if (itemSold[i].isEquipable)
            {
                CreateItemButton(itemSold[i].dataItemStuff.lootSprite, 
                    itemSold[i].dataItemStuff.lootName, itemSold[i].priceItem);
            }
            if (!itemSold[i].isEquipable)
            {
                CreateItemButton(itemSold[i].dataItemFood.lootSprite,
                    itemSold[i].dataItemFood.lootName, itemSold[i].priceItem);
            }
        }
    }

    private void CreateItemButton(Sprite sprite, string itemName, int itemCost)
    {
        UI_ShopItem itemButton = Instantiate(shopItemTemplate, Vector3.zero, Quaternion.identity);
        itemButton.imageItem.sprite = sprite;
        itemButton.nameItem.text = itemName;
        itemButton.prixItem.text = itemCost.ToString();
        itemButton.transform.SetParent(contentPanel);
    }

    void ClearShop()
    {
        for (int i = contentPanel.childCount - 1; i >= 0; i--)
        {
            Transform child = contentPanel.GetChild(i);
            Destroy(child.gameObject);
        }
    }

}

[Serializable]
public struct ItemShop
{
    
    public bool isEquipable;
    [ShowIf("isEquipable")]
    [AllowNesting]
    public EquipableItemSo dataItemStuff;
    [HideIf("isEquipable")]
    [AllowNesting]
    public EdibleItemSO dataItemFood;
    public int priceItem;

}
