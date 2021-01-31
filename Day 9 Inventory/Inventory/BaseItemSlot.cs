using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BaseItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // this item slot is for crafting which cant be add to inventory, item Chest etc.
    [SerializeField] protected Image image; // Slot Image
    [SerializeField] protected Text amountText;

    public event Action<BaseItemSlot> OnRightClickEvent;
    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;

    protected Color normalColor = Color.white;
    protected Color disableColor = Color.clear;

    protected bool isPointerOver;

    protected ItemSO _item; // 슬롯에 채워져있는 아이템과 비어있는 아이템을 구분하기 위해 따로 _Item를 쓴다
    public ItemSO Item
    {
        get
        {
            return _item;
        }
        set
        {
            _item = value;

            if (_item == null && Amount != 0) Amount = 0; // 아이템은 없는데 갯수가 있다면 갯수를 0으로 처리함

            if (_item == null)
            {
                image.sprite = null;
                image.color = disableColor;
            }
            else
            {
                image.sprite = _item.Icon; // 슬롯의 그림을 아이템의 그림으로 변환한다
                image.color = normalColor; 
            }

            if (isPointerOver)
            {
                OnPointerExit(null);
                OnPointerEnter(null);
            }
        }
    }

    private int _amount;
    public int Amount
    {
        get { return _amount; }
        set
        {
            _amount = value;

            if (_amount < 0) _amount = 0; // 갯수가 0 미만일시 0으로 설정

            if (_amount == 0 && Item != null) Item = null;

            if (amountText != null)
            {
                amountText.enabled = _item != null && _amount > 1;

                if (amountText.enabled)
                {
                    amountText.text = _amount.ToString();
                }
            }
        }

    }

    public virtual bool CanAddStack(ItemSO item, int amount = 1)
    {
        return Item != null && Item.ID == item.ID;
    }

    public virtual bool CanReceiveItem(ItemSO item)
    {
        return false;
    }

    protected virtual void OnValidate()
    {
        if (image == null)
        {
            image = GetComponent<Image>(); // 현 슬롯의 이미지를 가져온다
        }
        if (amountText == null)
        {
            amountText = GetComponentInChildren<Text>();
        }

        Item = _item;
        Amount = _amount;
    }

    protected virtual void OnDisable()
    {
        if (isPointerOver)
        {
            OnPointerExit(null);
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null && eventData.button == PointerEventData.InputButton.Right) // if clicked rightmousebutton on itemSlot
        {
            if (OnRightClickEvent != null)
            {
                OnRightClickEvent(this); // inventory & InventoryManager & EquipmentPanel Scripts Check
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;

        if (OnPointerEnterEvent != null)
        {
            OnPointerEnterEvent(this);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;

        if (OnPointerExitEvent != null)
        {
            OnPointerExitEvent(this);
        }
    }
}
