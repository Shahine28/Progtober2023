using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class UIInventoryPage : MonoBehaviour
{
    [SerializeField] private UIInventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanel;
    private List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();

    [SerializeField] List<LootFortune> listOfObjects  = new List<LootFortune>();

    public void InitializeInventoryUI(int inventorySize)
    {
        for(int i = 0; i < inventorySize; i++)
        {
            UIInventoryItem uiItem = Instantiate(itemPrefab,Vector3.zero,Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            listOfUIItems.Add(uiItem);
            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBeginDrag;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnItemDropped += HandleSwap;
            uiItem.OnRightMouseBtnClick += HandleShowItemActions;
        }
    }

    private void HandleShowItemActions(UIInventoryItem item)
    {
        
    }

    private void HandleSwap(UIInventoryItem item)
    {
        
    }

    private void HandleEndDrag(UIInventoryItem item)
    {
        
    }

    private void HandleBeginDrag(UIInventoryItem item)
    {
        
    }

    private void HandleItemSelection(UIInventoryItem item)
    {
        Debug.Log(item.name);
        item.gameObject.SetActive(true);
        listOfUIItems[0].Select();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        listOfUIItems[0].SetData(listOfObjects[0].lootSprite, 1);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
