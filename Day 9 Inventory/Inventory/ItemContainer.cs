using UnityEngine;
using System.Collections.Generic;
using System;

//abstract class = 미완성 클래스로, 자식이든 누군가가 반드시 재정의(override) 해야함
public abstract class ItemContainer : MonoBehaviour, IItemContainer
{
    public List<ItemSlot> itemSlots;

    public event Action<BaseItemSlot> OnRightClickEvent;

    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;

    public event Action<BaseItemSlot> OnBeginDragEvent;
    public event Action<BaseItemSlot> OnDragEvent;
    public event Action<BaseItemSlot> OnEndDragEvent;
    public event Action<BaseItemSlot> OnDropEvent;

    protected virtual void OnValidate()
    {
        // find all of ItemSlots in childrens and inactive
        GetComponentsInChildren(includeInactive: true, result: itemSlots);
    }

    protected virtual void Awake()
    {
        for(int i= 0; i < itemSlots.Count; i++) // 찾은 모든 아이템슬롯에
        {
            // Item slot 오른쪽클릭이벤트에 람다식으로 Base Item Slot의 action을 실행한다
            itemSlots[i].OnRightClickEvent += slot => EventHelper(slot, OnRightClickEvent);
            itemSlots[i].OnPointerEnterEvent += slot => EventHelper(slot, OnPointerEnterEvent);
            itemSlots[i].OnPointerExitEvent += slot => EventHelper(slot, OnPointerExitEvent);
            itemSlots[i].OnBeginDragEvent += slot => EventHelper(slot, OnBeginDragEvent);
            itemSlots[i].OnDragEvent += slot => EventHelper(slot, OnDragEvent);
            itemSlots[i].OnEndDragEvent += slot => EventHelper(slot, OnEndDragEvent);
            itemSlots[i].OnDropEvent += slot => EventHelper(slot, OnDropEvent);
        }
    }

    private void EventHelper(BaseItemSlot itemSlot, Action<BaseItemSlot> action)
    {
        if (action != null)
        {
            action(itemSlot);
        }
    }

    #region IItemContainer
    public virtual bool CanAddItem(ItemSO item, int amount = 1)
    {
        int freeSpaces = 0;

        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.Item == null || itemSlot.Item.ID == item.ID)
            {
                freeSpaces += item.MaxStacks - itemSlot.Amount;
            }
        }

        return freeSpaces >= amount;
    }

    public virtual bool AddItem(ItemSO item)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].CanAddStack(item))
            {
                itemSlots[i].Item = item;
                itemSlots[i].Amount++;
                return true;
            }
        }

        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].Item == null)
            {
                itemSlots[i].Item = item;
                itemSlots[i].Amount++;
                return true;
            }
        }

        return false;
    }

    public virtual bool RemoveItem(ItemSO item)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].Item == item)
            {
                itemSlots[i].Amount--;
                return true;
            }
        }
        return false;
    }

    public virtual ItemSO RemoveItem(string ItemID)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            ItemSO item = itemSlots[i].Item;

            if (item != null && item.ID == ItemID)
            {
                itemSlots[i].Amount--;
                return item;
            }
        }
        return null;
    }

    public virtual int ItemCount(string itemID)
    {
        int number = 0;

        for (int i = 0; i < itemSlots.Count; i++)
        {
            ItemSO item = itemSlots[i].Item;
            if (item != null && item.ID == itemID)
            {
                number += itemSlots[i].Amount;
            }
        }
        return number;
    }

    public void Clear()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].Item != null && Application.isPlaying)
            {
                itemSlots[i].Item.Destroy();
            }
            itemSlots[i].Item = null;
            itemSlots[i].Amount = 0;
        }
    }

    #endregion
}
