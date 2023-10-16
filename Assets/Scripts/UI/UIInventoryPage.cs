using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class UIInventoryPage : MonoBehaviour
{
    [SerializeField] private UIInventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanel;
    private List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();

    [SerializeField] List<LootFortune> listOfObjects  = new List<LootFortune>();

    [SerializeField] private MouseFollower mouseFolower;

    private int currentlyDraggedItemIndex = -1;

    public event Action<int> OnItemActionRequested, OnStartDragging;
    public event Action<int, int> OnSwapItems;


    private void Awake()
    {
        Hide();
        mouseFolower.Toggle(false);
        
    }


    public void InitializeInventoryUI(int inventorysize)
    {
        for (int i = 0; i < inventorysize; i++)
        {
            UIInventoryItem uiItem =
                Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            listOfUIItems.Add(uiItem);
            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBeginDrag;
            uiItem.OnItemDroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseBtnClick += HandleShowItemActions;
        }
    }

    private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
    {
        
    }

    private void HandleSwap(UIInventoryItem inventoryItemUI)
    {
        Debug.Log("Swap");
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            mouseFolower.Toggle(false);
            currentlyDraggedItemIndex = -1;
            return;
        }

        listOfUIItems[currentlyDraggedItemIndex]
            .SetData(index == 0 ? listOfObjects[0].lootSprite : listOfObjects[1].lootSprite, 1);
        listOfUIItems[index]
            .SetData(currentlyDraggedItemIndex == 0 ? listOfObjects[0].lootSprite : listOfObjects[1].lootSprite, 1);
        mouseFolower.Toggle(false);
        currentlyDraggedItemIndex = -1;

    }

    private void HandleEndDrag(UIInventoryItem inventoryItemUI)
    {
        mouseFolower.Toggle(false);
    }

    private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
    {
        Debug.Log("Swap5");
        int index  = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1) return;
        currentlyDraggedItemIndex = index;

        mouseFolower.Toggle(true);
        mouseFolower.SetData(index == 0 ? listOfObjects[0].lootSprite : listOfObjects[1].lootSprite, 1);
    }

    private void HandleItemSelection(UIInventoryItem  inventoryItemUI)
    {
/*        inventoryItemUI.gameObject.SetActive(true);*/
        listOfUIItems[0].Select();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        listOfUIItems[0].SetData(listOfObjects[0].lootSprite, 1);
        listOfUIItems[1].SetData(listOfObjects[1].lootSprite, 1);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
