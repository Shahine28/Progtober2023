using Inventory.Model;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UI_Shop : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform shopItemTemplate;
    public List<ItemShop> itemSold = new List<ItemShop>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    [SerializeField] int priceItem;

}
