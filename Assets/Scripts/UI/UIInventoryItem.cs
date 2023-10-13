using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using System;
using UnityEngine.EventSystems;

public class UIInventoryItem : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;
    [SerializeField] private TextMeshProUGUI quantityTxt;
    [SerializeField] private Image borderImage;

    public event Action<UIInventoryItem> OnItemClicked, OnItemDropped, OnItemBeginDrag, OnItemEndDrag, OnRightMouseBtnClick;
    private bool empty = true;

    private void Awake()
    {
        ResetData();
        Deselect();
    }

    public void ResetData()
    {
        this.itemImage.gameObject.SetActive(false);
        this.empty = true;
    }
    public void Deselect()
    {
        borderImage.enabled = false; 
    }

    public void Select()
    {
        borderImage.enabled = true;
    }
    

    public void SetData(Sprite sprite, int quantity)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.quantityTxt.text = quantity + "";
        this.empty = false;
    }

    public void OnBeginDrag()
    {
        if (empty) return;
        OnItemBeginDrag?.Invoke(this);
    }
    public void OnDrop()
    {
        OnItemDropped?.Invoke(this);
    }

    public void OnEndDrag()
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnPointerClick(BaseEventData data)
    {
        if (empty) return;
        PointerEventData pointerData = (PointerEventData)data;
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseBtnClick?.Invoke(this);
        }
        else
        {
            OnItemClicked?.Invoke(this);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}
