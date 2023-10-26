using Inventory;
using Inventory.Model;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Shop : MonoBehaviour
{
    [SerializeField] private Transform contentPanel;
    [SerializeField] private GameObject shopParent;
    [SerializeField] private UI_ShopItem shopItemTemplate;
    [SerializeField] private Coin goldCount;
    [SerializeField] private InventoryPickUpSystem inventoryPickUp;
    public List<ItemShop> itemSold = new List<ItemShop>();
    public InputGame _playerInput;

    private void Awake()
    {
        _playerInput = new InputGame();
    }
    void Start()
    {
        ClearShop();
        CreateShop();
        
    }

    private void CreateItemButton(Sprite sprite, string itemName, int itemCost, int index, LootFortune lootData)
    {
        UI_ShopItem itemButton = Instantiate(shopItemTemplate, Vector3.zero, Quaternion.identity);
        itemButton.imageItem.sprite = sprite;
        itemButton.nameItem.text = itemName;
        itemButton.prixItem.text = itemCost.ToString();
        itemButton.prix = itemCost;
        itemButton.index = index;
        itemButton.transform.SetParent(contentPanel);
        itemButton.GetComponent<Item>().InventoryItem = lootData;
    }

    public void BuyItem(UI_ShopItem itemButton)
    {
        if (itemButton != null)
        {
            if (itemButton.prix <= goldCount.Gold)
            {
                goldCount.Gold -= itemButton.prix;
                inventoryPickUp.AddItemFromShop(itemButton.GetComponent<Item>());
            }
        }
    }

    void ClearShop()
    {
        for (int i = contentPanel.childCount - 1; i >= 0; i--)
        {
            Transform child = contentPanel.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    

    void CreateShop()
    {
        for (int i = 0; i < itemSold.Count; i++)
        {
            if (itemSold[i].isEquipable)
            {
                CreateItemButton(itemSold[i].dataItemStuff.lootSprite,
                    itemSold[i].dataItemStuff.lootName, itemSold[i].priceItem, i, itemSold[i].dataItemStuff);
            }
            if (!itemSold[i].isEquipable)
            {
                CreateItemButton(itemSold[i].dataItemFood.lootSprite,
                    itemSold[i].dataItemFood.lootName, itemSold[i].priceItem, i, itemSold[i].dataItemFood);
            }
        }
    }
    #region Show/Hide
    public void Show()
    {
        Debug.Log(GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryController>().MainInventoryPanel.activeSelf);
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryController>().MainInventoryPanel.activeSelf)
        {
            // Inverse l'�tat de visibilit� de shopParent
            shopParent.SetActive(!shopParent.activeSelf);
        }


    }

    public void Hide()
    {
        shopParent.SetActive(false);
    }
    #endregion

    public void HideInventory(InputAction.CallbackContext context)
    {
        if (context.performed && shopParent.activeInHierarchy)
        {
            Hide();
        }
    }
    #region OnEnable/OnDisable
    public void OnEnable()
    {
        _playerInput.Player.Escape.Enable();
    }
    public void OnDisable()
    {
        _playerInput.Player.Escape.Disable();
    }
    #endregion
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
