using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private UIInventoryPage inventoryUI;
        [SerializeField] private UIInventoryPage inventoryUIBar;
        [SerializeField] private InputGame _playerInput;

        [SerializeField] private InventorySO inventoryData;
        [SerializeField] private InventorySO inventoryDataBar;

        [HideInInspector] public bool inventoryUIIsOpen;
        public List<InventoryItem> initialItems = new List<InventoryItem>();

        private void Awake()
        {
            _playerInput = new InputGame();

        }

        private void Start()
        {
            PrepareUI();
            PrepareUIBar();
            PrepareInventoryData();
            PrepareInventoryDataBar();
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

        private void PrepareInventoryDataBar()
        {
            inventoryDataBar.Initialize();
            inventoryDataBar.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty) continue;
                inventoryDataBar.AddItem(item);
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

        private void PrepareUIBar()
        {
            inventoryUIBar.Show();
            inventoryUIBar.InitializeInventoryUI(inventoryDataBar.Size);
            inventoryUIBar.OnItemClicked += HandleItemClick;
            inventoryUIBar.OnStartDragging += HandleDragging;
            inventoryUIBar.OnSwapItems += HandleSwapItems;
            inventoryUIBar.OnItemActionRequested += HandleItemActionRequested;
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
            string description = PrepareDescription(inventoryItem);
            inventoryUI.UdpateClick(itemIndex, description);
            

        }

        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem.item.lootName);
            sb.AppendLine();
            for (int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.parameterName} : " +
                    $"{inventoryItem.itemState[i].value} /" +
                    $" {inventoryItem.item.DefaultParametersList[i].value}");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private void HandleItemActionRequested(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {

                inventoryUI.ShowItemActions(itemIndex);
                inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
            }

        }

        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();   
        }

        public void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty) return;

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject, inventoryItem.itemState);
                if(inventoryData.GetItemAt(itemIndex).IsEmpty)
                {
                    inventoryUI.ResetSelection();
                }
            }
            
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
                    inventoryUIIsOpen = true;
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UdpateData(item.Key, item.Value.item.lootSprite, item.Value.quantity);
                    }
                }
                else
                {
                    inventoryUI.Hide();
                    inventoryUIIsOpen = false;
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