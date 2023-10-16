using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private UIInventoryPage inventoryUI;
        [SerializeField] private InputGame _playerInput;

        [SerializeField] private InventorySO inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        private void Awake()
        {
            _playerInput = new InputGame();

        }

        private void Start()
        {
            PrepareUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty) continue;
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UdpateData(item.Key, item.Value.item.lootSprite, item.Value.quantity);
            }

        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            inventoryUI.OnItemClicked += HandleItemClick;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnItemActionRequested += HandleItemActionRequested;
        }

        private void HandleItemClick(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            LootFortune item = inventoryItem.item;
            inventoryUI.UdpateClick(itemIndex);
        }

        private void HandleItemActionRequested(int itemIndex)
        {
            throw new NotImplementedException();
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty) return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.lootSprite, inventoryItem.quantity);
        }

        public void inventory(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                /*Debug.Log("J'appuie");*/
                if (!inventoryUI.isActiveAndEnabled)
                {
                    inventoryUI.Show();
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UdpateData(item.Key, item.Value.item.lootSprite, item.Value.quantity);
                    }
                }
                else
                {
                    inventoryUI.Hide();
                }

            }
            else if (context.canceled)
            {
                /*Debug.Log("Je relache");*/
            }
        }

        private void OnEnable()
        {
            _playerInput.Player.Inventory.Enable();
        }

        private void OnDisable()
        {
            _playerInput.Player.Inventory.Disable();

        }

    }
}