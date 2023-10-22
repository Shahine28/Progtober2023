using Inventory.Model;
using Inventory.UI;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        // Dictionnaire pour stocker les inventaires actifs
        private Dictionary<string, InventorySO> Inventories = new Dictionary<string, InventorySO>();
/*        private Dictionary<string, InventorySO> activeInventories = new Dictionary<string, InventorySO>();*/

        // Dictionnaire pour stocker l'état de chaque inventaire actif
        private Dictionary<string, Dictionary<int, InventoryItem>> inventoryStates = new Dictionary<string, Dictionary<int, InventoryItem>>();

        private string activeInventoryKey = "ToolbarInventory"; // Inventaire actif par défaut



        [SerializeField] private UIInventoryPage inventoryUI;
        [SerializeField] private UIInventoryPage inventoryUIBar;
        [SerializeField] private InputGame _playerInput;

        [SerializeField] private InventorySO mainInventoryData;
        [SerializeField] private InventorySO toolbarInventoryData;
        private string lastOverhauledInventory;
        private int indexOfLastItemDragged;

        [HideInInspector] public bool inventoryUIIsOpen;
        public List<InventoryItem> initialItems = new List<InventoryItem>();
        public List<InventoryItem> initialBarItems = new List<InventoryItem>();

        private InventorySO initializeInventory;

        private Dictionary<InventorySO, UIInventoryPage> _inventoryUI = new Dictionary<InventorySO, UIInventoryPage>();
        private Dictionary<InventorySO, List<InventoryItem>> _initialItems = new Dictionary<InventorySO, List<InventoryItem>>();

        private string lastInventoryRecorded = "ToolBarInventory"; // A utiliser uniquement lorsque je ne peux pas accéder à l'inventaire survolé par ma souris.
        private int lastItemSelected;

        // Gestion du drop
        [SerializeField] private GameObject droppedItemPrefab;

        private void Awake()
        {
            _playerInput = new InputGame();

        }

        private void Start()
        {
           
            // Ajoutez les inventaires existants
            Inventories.Add("MainInventory", mainInventoryData);
            Inventories.Add("ToolbarInventory", toolbarInventoryData);

/*            // Ajoute l'inventaire actif
            activeInventories.Add("ToolbarInventory", toolbarInventoryData);*/

            // Associe à l'inventaire MainInventory son UI et ses items initiaux
            _inventoryUI.Add(mainInventoryData, inventoryUI);
            _initialItems.Add(mainInventoryData, initialItems);

            // Associe à l'inventaire ToolbarInventory son UI et ses items initiaux
            _inventoryUI.Add(toolbarInventoryData, inventoryUIBar);
            _initialItems.Add(toolbarInventoryData, initialBarItems);

/*            Debug.Log(GetItemsNumber("ToolbarInventory"));*/


            PrepareUI();
            PrepareInventoryData();

            string description = PrepareDescription(Inventories["ToolbarInventory"].GetItemAt(0));
            inventoryUIBar.Show();
            inventoryUIBar.UdpateClick(0, description);

            foreach (var inventoryEntry in Inventories)
            {
                string inventoryKey = inventoryEntry.Key;
                InventorySO inventoryData = inventoryEntry.Value;

                // Associer le tag du nom de l'inventaire à l'UI correspondante
                CreateTag(inventoryKey);
                _inventoryUI[inventoryData].gameObject.tag = inventoryKey;
            } // Je crée un tag pour chaque inventaire.

        }

/*        public void Update()
        {
            if (GetActiveInventoryUnderMouseTag() != null)
            {
                
            }
                
        }*/


        /*        private void PrepareInventoryData()
                {
                    foreach(var inventory in Inventories)
                    {
                        inventory.Value.Initialize();
                        initializeInventory = inventory.Value;
                        inventory.Value.OnInventoryUpdated += UpdateInventoryUI;
                        List<InventoryItem> InitialItems = _initialItems[inventory.Value];
                        foreach (InventoryItem item in InitialItems)
                        {
                            if (item.IsEmpty) continue;
                            inventory.Value.AddItem(item);
                        }

                    }
                }*/

        private void PrepareInventoryData()
                {
                    foreach (var inventoryEntry in Inventories)
                    {
                        var inventoryKey = inventoryEntry.Key;
                        var inventoryData = inventoryEntry.Value;

                        inventoryData.Initialize();
                        inventoryData.OnInventoryUpdated += (state) => UpdateInventoryUI(inventoryKey, state);

                        var initialItems = _initialItems[inventoryData];
                        foreach (InventoryItem item in initialItems)
                        {
                            if (item.IsEmpty) continue;
                            inventoryData.AddItem(item);
                        }
                    }
                }

        /*        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
                {
                    string InventaireSouris = GetActiveInventoryUnderMouseTag();
                    UIInventoryPage UIinventory = _inventoryUI[initializeInventory];
                    UIinventory.ResetAllItems();
                    foreach (var item in inventoryState)
                    {
                        UIinventory.UdpateData(item.Key, item.Value.item.lootSprite, item.Value.quantity);
                    }

                }*/

        private void UpdateInventoryUI(string inventoryKey, Dictionary<int, InventoryItem> inventoryState)
        {
            UIInventoryPage UIinventory = _inventoryUI[Inventories[inventoryKey]];
            UIinventory.ResetAllItems();
            foreach (var item in inventoryState)
            {
                UIinventory.UdpateData(item.Key, item.Value.item.lootSprite, item.Value.quantity);
            }
        }



        private void PrepareUI()
        {
            foreach (var inventory in Inventories)
            {
                UIInventoryPage UIinventory = _inventoryUI[inventory.Value];
                UIinventory.InitializeInventoryUI(inventory.Value.Size);
                UIinventory.OnItemClicked += HandleItemClick;
                UIinventory.OnStartDragging += HandleDragging;
                UIinventory.OnSwapItems += HandleSwapItems;
                UIinventory.OnItemActionRequested += HandleItemActionRequested;
            }

        }

        private void HandleItemClick(int itemIndex)
        {

            string InventaireSouris = GetActiveInventoryUnderMouseTag();
            InventoryItem inventoryItem = Inventories[InventaireSouris].GetItemAt(itemIndex);
            UIInventoryPage UIinventory = _inventoryUI[Inventories[InventaireSouris]]; // j'obtiens l'UI relié à mon inventaire
            if (inventoryItem.IsEmpty)
            {
                foreach(var inventory in Inventories)
                {
                    _inventoryUI[inventory.Value].ResetSelection();
                }
                return;
            }
            LootFortune item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem);
            if (InventaireSouris == "ToolbarInventory") lastItemSelected = itemIndex;
            UIinventory.UdpateClick(itemIndex, description);
            foreach (var UI in _inventoryUI)
            {
                if (UI.Value != UIinventory)
                {
                    UI.Value.ResetSelection();
                }
            }

        }


        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();
            if (inventoryItem.item != null)
            {
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
            return null;
        }

        private void HandleItemActionRequested(int itemIndex)
        {
            
            string InventaireSouris = GetActiveInventoryUnderMouseTag();
            InventoryItem inventoryItem = Inventories[lastInventoryRecorded].GetItemAt(itemIndex);
            UIInventoryPage UIinventory = _inventoryUI[Inventories[lastInventoryRecorded]]; // j'obtiens l'UI relié à mon inventaire
            if (inventoryItem.IsEmpty)
                return;

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                foreach (var inventory in Inventories)
                {
                    _inventoryUI[inventory.Value].ResetSelection();
                }
                UIinventory.listOfUIItems[itemIndex].Select();
                UIinventory.ShowItemActions(itemIndex);
                UIinventory.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                UIinventory.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
            }

        }

        private void DropItem(int itemIndex, int quantity)
        {
            /*string InventaireSouris = GetActiveInventoryUnderMouseTag();*/
            UIInventoryPage UIinventory = _inventoryUI[Inventories[lastInventoryRecorded]]; // j'obtiens l'UI relié à mon inventaire
            for (int i = 0; i<quantity; i++)
            {
                GameObject lootGameObject = Instantiate(droppedItemPrefab, new Vector2(transform.position.x + 1f, 
                    transform.position.y + 1f), Quaternion.identity);
                lootGameObject.GetComponent<Item>().InventoryItem = Inventories[lastInventoryRecorded].GetItemAt(itemIndex).item;
                lootGameObject.GetComponent<Loot>().hasNotBeDropped = false;
                lootGameObject.GetComponent<SpriteRenderer>().sprite = Inventories[lastInventoryRecorded].GetItemAt(itemIndex).item.lootSprite;
            }
            Inventories[lastInventoryRecorded].RemoveItem(itemIndex, quantity);
            UIinventory.ResetSelection();   
        }

        public void PerformAction(int itemIndex)
        {
/*            string InventaireSouris = GetActiveInventoryUnderMouseTag();*/
            InventoryItem inventoryItem = Inventories[lastInventoryRecorded].GetItemAt(itemIndex);
            UIInventoryPage UIinventory = _inventoryUI[Inventories[lastInventoryRecorded]]; // j'obtiens l'UI relié à mon inventaire

            if (inventoryItem.IsEmpty) return;

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                Inventories[lastInventoryRecorded].RemoveItem(itemIndex, 1);
                UIinventory.ResetSelection();
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject, inventoryItem.itemState);
                if(inventoryItem.IsEmpty)
                {
                    UIinventory.ResetSelection();
                }
            }
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {

            string InventaireSouris = GetActiveInventoryUnderMouseTag();
            if (InventaireSouris == lastOverhauledInventory)
            {
                Inventories[InventaireSouris].SwapItems(itemIndex_1, itemIndex_2);
            }
            else
            {
/*                Debug.Log("itemIndex1 " + indexOfLastItemDragged + ", itemIndex2 " + itemIndex_2);
                Debug.Log("Swap Between inventories");
                Debug.Log("Inventaire 1 " + lastOverhauledInventory + ", Inventaire 2 " + InventaireSouris);*/
                if (InventaireSouris == null)
                {
                    DropItem(itemIndex_1, Inventories[InventaireSouris].GetItemAt(itemIndex_1).quantity);

                }
                else
                {
                    List<InventoryItem> Inventories1Item = Inventories[lastOverhauledInventory].inventoryItems;
                    List<InventoryItem> Inventories2Item = Inventories[InventaireSouris].inventoryItems;
                    Inventories[InventaireSouris].SwapItemsBetweenInventories(indexOfLastItemDragged, Inventories1Item,
                        itemIndex_2, Inventories2Item);
                    Inventories[lastOverhauledInventory].InformAboutChange();
                }


            }
            

            /*inventoryData.SwapItems(itemIndex_1, itemIndex_2);*/
        }

        private void HandleDragging(int itemIndex)
        {
            string InventaireSouris = GetActiveInventoryUnderMouseTag();
            lastOverhauledInventory = InventaireSouris;
            InventoryItem inventoryItem = Inventories[InventaireSouris].GetItemAt(itemIndex);
            indexOfLastItemDragged = itemIndex;
            UIInventoryPage UIinventory = _inventoryUI[Inventories[InventaireSouris]]; // j'obtiens l'UI relié à mon inventaire
            if (inventoryItem.IsEmpty) return;
            UIinventory.CreateDraggedItem(inventoryItem.item.lootSprite, inventoryItem.quantity);

/*            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
              if (inventoryItem.IsEmpty)
                  return;
              inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);*/
        }

        public void inventory(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                foreach (var inventory in Inventories)
                {
                    _inventoryUI[inventory.Value].ResetSelection();
                }
                /*Debug.Log("J'appuie");*/
                if (!inventoryUI.isActiveAndEnabled)
                {
/*                    activeInventories.Add("MainInventory", mainInventoryData);*/
                    inventoryUI.Show();
                    inventoryUIIsOpen = true;
                    foreach (var item in mainInventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UdpateData(item.Key, item.Value.item.lootSprite, item.Value.quantity);
                    }
                }
                else
                {
/*                    activeInventories.Remove("MainInventory");*/
                    inventoryUI.Hide();
                    string description = PrepareDescription(Inventories["ToolbarInventory"].GetItemAt(0));
                    inventoryUIBar.UdpateClick(lastItemSelected, description); // Lorsque que la
                    inventoryUIIsOpen = false;
                    /*lastItemSelected = 0;*/
                }

            }
            else if (context.canceled)
            {
                /*Debug.Log("Je relache");*/
            }
        }


        public void OnScroll(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (!inventoryUI.isActiveAndEnabled)
                {
                    if (context.ReadValue<float>() > 0f)
                    {
                        Debug.Log("Scroll Up");
                        lastItemSelected += 1;
                    }
                    if (context.ReadValue<float>() < 0f)
                    {
                        Debug.Log("Scroll Down");
                        lastItemSelected -= 1;
                    }
                    if (lastItemSelected < 0)
                    {
                        lastItemSelected = Inventories["ToolbarInventory"].inventoryItems.Count - 1;
                    }
                    else if (lastItemSelected > Inventories["ToolbarInventory"].inventoryItems.Count - 1)
                    {
                        lastItemSelected = 0;
                    }
                    string description = PrepareDescription(Inventories["ToolbarInventory"].GetItemAt(lastItemSelected));
                    inventoryUIBar.UdpateClick(lastItemSelected, description);

                }

            }
        }
        public string GetActiveInventoryUnderMouseTag() // Renvoie le nom de l'inventaire au dessus duquel je passe ma souris
        {
            Vector2 mousePosition = Input.mousePosition;
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
            };

            pointerData.position = mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
            {
                // Vérifiez si le GameObject possède un composant Image avec Raycast Target activé
                Image inventoryImage = result.gameObject.GetComponent<Image>();

                if (inventoryImage != null && inventoryImage.raycastTarget)
                {
                    if (result.gameObject.GetComponent<UIInventoryPage>() != null)
                    {
                        lastInventoryRecorded = result.gameObject.tag;
                        return result.gameObject.tag;
                    }
                    GameObject parent = result.gameObject.transform.parent.gameObject;
                    while (parent != null && parent.GetComponent<UIInventoryPage>() == null)
                    {
                        parent = parent.transform.parent.gameObject;
                    }
                    if (parent!= null) lastInventoryRecorded = parent.tag;
                    return parent.tag; // La souris est au-dessus de cet inventaire
                }
            }

            return null; // Aucun inventaire actif sous la souris
        }


        private int GetItemsNumber(string nameInventory)
        {
            int numberItems = 0;
            for (int i = 0; i < Inventories[nameInventory].inventoryItems.Count - 1; i++)
            {
                if (!Inventories[nameInventory].inventoryItems[i].IsEmpty)
                {
                    numberItems++;
                }
            }
            return numberItems;
        }
        private void OnEnable()
        {
            _playerInput.Player.Inventory.Enable();
            _playerInput.Player.MouseScrollY.Enable();
        }

        private void OnDisable()
        {
            _playerInput.Player.Inventory.Disable();
            _playerInput.Player.MouseScrollY.Disable();

        }

        public static void CreateTag(string tagName)
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            // Vérifiez si le tag existe déjà
            bool tagExists = false;
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty tag = tagsProp.GetArrayElementAtIndex(i);
                if (tag.stringValue == tagName)
                {
                    tagExists = true;
                    break;
                }
            }

            if (!tagExists)
            {
                // Ajoutez un nouveau tag
                int newIndex = tagsProp.arraySize;
                tagsProp.InsertArrayElementAtIndex(newIndex);
                SerializedProperty newTag = tagsProp.GetArrayElementAtIndex(newIndex);
                newTag.stringValue = tagName;

                tagManager.ApplyModifiedProperties();
            }
        }

    }
}