using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Inventory.UI;
using System;
using Unity.VisualScripting;

public class UI_CraftItem : MonoBehaviour, IPointerClickHandler
{

    public Image imageItem;
    public string nameItem;
    public Image borderImage;
    [HideInInspector]
    public int index;
    


    public event Action<UIInventoryItem> OnItemClicked;

    private void Awake()
    {
        ResetData();
        Deselect();
    }
    private void Start()
    {

    }

    public void SetData(Sprite sprite, string name)
    {
        imageItem.gameObject.SetActive(true);
        imageItem.sprite = sprite;
        nameItem = name;
    }
    public void ResetData()
    {
        imageItem.gameObject.SetActive(false);
    }
    public void Deselect()
    {
        borderImage.enabled = false;
    }
    public void OnPointerClick(PointerEventData pointerData)
    {
        for (int i = 0; i< gameObject.transform.parent.childCount; i++)
        {
            gameObject.transform.parent.GetChild(i).GetComponent<UI_CraftItem>().Deselect();
        }
        Select();
    }

    public void Select()
    {
        borderImage.enabled = true;
        gameObject.transform.parent.transform.parent.GetComponent<CraftSysteme>().SetData(nameItem);
    }
}
