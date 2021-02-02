using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : BaseItemSlot, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public event Action<BaseItemSlot> OnBeginDragEvent;
    public event Action<BaseItemSlot> OnDragEvent;
    public event Action<BaseItemSlot> OnEndDragEvent;
    public event Action<BaseItemSlot> OnDropEvent;

    private Color dragColor = new Color(1f, 1f, 1f, 0.5f); // 반투명한 색깔
    private bool isDragging;

    
    public override bool CanAddStack(ItemSO item, int amount = 1)
    {
        return base.CanAddStack(item, amount) && Amount + amount <= item.MaxStacks;
    }

    public override bool CanReceiveItem(ItemSO item) // baseItemSlot 스크립트는 아이템 회수가 불가능하지만 itemSlot은 가능하게 했다
    {
        return true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;

        if (Item != null)
        {
            image.color = dragColor;
        }
        if (OnBeginDragEvent != null)
        {
            OnBeginDragEvent(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragEvent != null)
        {
            OnDragEvent(this);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        if (Item != null)
        {
            image.color = normalColor; // Image => BaseItemSlot Script
        }

        if (OnEndDragEvent != null)
        {
            OnEndDragEvent(this);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (OnDropEvent != null)
        {
            OnDropEvent(this);
        }
    }
}
