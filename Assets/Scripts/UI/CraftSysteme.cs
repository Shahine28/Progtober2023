using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;
using Unity.VisualScripting;
using System.Diagnostics;

public class CraftSysteme : MonoBehaviour
{
    [Header("Gestion des inventaires")]
    [SerializeField] private InventorySO mainInventory;
    [SerializeField] private InventorySO toolbarInventory;

    [Header("Gestion de l'affichage des items craftables")]
    [SerializeField] private Transform itemListPage;
    [SerializeField] private GameObject craftItemPrefab;
    public List<ItemCraft> listItemCraft = new List<ItemCraft>();

    [Header("Gestion de la description des items")]
    public TextMeshProUGUI nomItem;
    public TextMeshProUGUI descriptionItem;
    public Image imageItem;

    [Header("Gestion du tableau des ressources")]
    public Transform contentTable;
    [SerializeField] private GameObject contentItemTablePrefab;

    private bool firstFramePassed;
    void Start()
    {
        InitializeData();
        InitializeTable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!firstFramePassed)
        {
            firstFramePassed = true;
            itemListPage.GetChild(0).gameObject.GetComponent<UI_CraftItem>().Select();

        }
    }

    void InitializeData()
    {
        for (int i = 0; i < itemListPage.childCount; i++)
        {
            Destroy(itemListPage.GetChild(i).gameObject);
        }
        foreach (ItemCraft craftableItem in listItemCraft)
        {
            GameObject item = Instantiate(craftItemPrefab, Vector3.zero, Quaternion.identity);
            item.transform.SetParent(itemListPage);
            item.GetComponent<UI_CraftItem>().SetData(craftableItem.item.lootSprite, craftableItem.name);
            

        }
        SetData(listItemCraft[0].name);
        UnityEngine.Debug.Log(itemListPage.GetChild(0).gameObject.GetComponent<UI_CraftItem>().nameItem);
        itemListPage.GetChild(0).gameObject.GetComponent<UI_CraftItem>().Select();



    }

    public void SetData(string itemName)
    {
        foreach (ItemCraft craft in listItemCraft)
        {
            if (craft.name == itemName)
            {
                nomItem.text = craft.name;
                descriptionItem.text = craft.description;
                imageItem.sprite = craft.item.lootSprite;
                break;
                
            }
        }
    }

    private void InitializeTable()
    {
        for (int i = 0; i < contentTable.childCount; i++)
        {
            Destroy(contentTable.GetChild(i).gameObject);
        }
        for (int i = 0; i<4; i++)
        {
            GameObject tableLine = Instantiate(contentItemTablePrefab, Vector3.zero, Quaternion.identity);
            tableLine.transform.SetParent(contentTable);
            
        }
    }
    private void SetTable(ItemCraft item)
    {

    }

    [Serializable]
    public struct ItemCraft
    {
        public LootFortune item;
        public string name;
        [TextArea(1,5)]
        public string description;
        public int quantity;
        public List<ItemCraftRequirements> ItemRequirements;
    }
    [Serializable]
    public struct ItemCraftRequirements
    {
        public LootFortune itemRequired;
        public int quantityRequired;
    }
}
