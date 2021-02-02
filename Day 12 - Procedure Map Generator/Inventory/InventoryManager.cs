using StatMod.Stat;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public RangedWeapon weaponScript;
    [SerializeField] InvPanel inventory;
    [SerializeField] WeaponPanel weaponPanel;
    [SerializeField] DropItemArea dropItemArea;
    [SerializeField] DestoryQuestionDialog destroyQuestionDialog;
    [SerializeField] Image draggingItem; // 시야적으로 drag중인지 확인해줄 수 있게 해주는 것, raycast Target를 off해둘 것
    BaseItemSlot dragItemSlot; // drag와 drop할 곳을 분별하기 위한 slot
    /*
    public bool isInvOpened()
    {
        if(gameObject.activeSelf == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    */
    private void Awake()
    {
        inventory.OnRightClickEvent += InvRightClick;
        weaponPanel.OnRightClickEvent += WeaponPanelRightClick;

        inventory.OnBeginDragEvent += BeginDrag;
        weaponPanel.OnBeginDragEvent += BeginDrag;

        inventory.OnDragEvent += Drag;
        weaponPanel.OnDragEvent += Drag;

        inventory.OnEndDragEvent += EndDrag;
        weaponPanel.OnEndDragEvent += EndDrag;

        inventory.OnDropEvent += Drop;
        weaponPanel.OnDropEvent += Drop;
        dropItemArea.OnDropEvent += DropItemOutsideUI;
    }
    void InvRightClick(BaseItemSlot itemSlot)
    {
        if(itemSlot.Item is WeaponSO)   EquipWeapon((WeaponSO)itemSlot.Item);
    }

    void WeaponPanelRightClick(BaseItemSlot itemSlot)
    {
        if(itemSlot.Item is WeaponSO)   UnequipWeapon((WeaponSO)itemSlot.Item);
    }

    #region Drag&Drop

    void BeginDrag(BaseItemSlot _itemSlot)
    {
        if(_itemSlot.Item != null)
        {
            dragItemSlot = _itemSlot;

            draggingItem.sprite = _itemSlot.Item.Icon;
            draggingItem.transform.position = Input.mousePosition;
            draggingItem.gameObject.SetActive(true);
        }
    }

    void Drag(BaseItemSlot _itemSlot)
    {
        draggingItem.transform.position = Input.mousePosition;
    }

    void EndDrag(BaseItemSlot _itemSlot)
    {
        dragItemSlot = null;

        draggingItem.gameObject.SetActive(false);
    }

    void Drop(BaseItemSlot _itemSlot)
    {
        if (dragItemSlot == null) return; //만약 drop한 슬롯에 아무도 없으면 반환, drag한 건 전부다 되돌려진다

        if (_itemSlot.CanAddStack(dragItemSlot.Item))
        {
            AddStacks(_itemSlot);
        }
        else if(_itemSlot.CanReceiveItem(dragItemSlot.Item) &&dragItemSlot.CanReceiveItem(_itemSlot.Item)) // 처음 dragSlot과 dropSlot 둘다 아이템이 들어갈 수 있다면
        {
            SwapItem(_itemSlot);
        }
            // 마지막으로 drag한 아이템슬롯 = _itemSlot, 처음 drag를 시작 아이템슬록 = dragItemSlot
    }

    void DropItemOutsideUI()
    {
        if (dragItemSlot == null) return;

        destroyQuestionDialog.Show();
        BaseItemSlot baseItemSlot = dragItemSlot;
        destroyQuestionDialog.OnYesEvent += () => DestroyItem(baseItemSlot);

        BaseItemSlot slot = dragItemSlot;
    }
    void DestroyItem(BaseItemSlot baseItemSlot)
    {
        baseItemSlot.Item.Destroy();
        baseItemSlot.Item = null;
        destroyQuestionDialog.gameObject.SetActive(false);
    }

    void AddStacks(BaseItemSlot _itemSlot)
    {
        int numAddableStack = _itemSlot.Item.MaxStacks - _itemSlot.Amount;
        int stackToAdd = Mathf.Min(numAddableStack, dragItemSlot.Amount);

        _itemSlot.Amount += stackToAdd;
        dragItemSlot.Amount -= stackToAdd;
    }

    void SwapItem(BaseItemSlot _itemSlot)
    {
        //as = 만약 A = B as A라면 B를 A로 형변환하고, 아니면 null값을 준다
        WeaponSO dragWeaponItem = dragItemSlot.Item as WeaponSO;
        WeaponSO dropWeaponItem = _itemSlot.Item as WeaponSO;
        // dragWeapon = 처음 drag를 시작한 슬롯에 있던 아이템 
        // dropWeapon = drag를 끝내고 drop할려는 슬롯에 있는 아이템

        if (_itemSlot is WeaponSlot) // 드롭한 곳이 무기슬롯일 때
        {
            if (dragWeaponItem != null) dragWeaponItem.Equip(this,weaponPanel);
            if (dropWeaponItem != null) dropWeaponItem.Unequip(this,weaponPanel);
        }
        if (dragItemSlot is WeaponSlot) // 드래그한 곳이 무기슬롯 일 때
        {
            if (dragWeaponItem != null) dragWeaponItem.Unequip(this,weaponPanel);
            if (dropWeaponItem != null) dropWeaponItem.Equip(this, weaponPanel);
        }

        // 인벤토리나 무기창 아무상관 없이 아이템을 swap & drop하게 해줌

        ItemSO draggedItem = _itemSlot.Item; 
        int draggedItemAmount = dragItemSlot.Amount;

        _itemSlot.Item = dragItemSlot.Item; 
        dragItemSlot.Amount = _itemSlot.Amount;

        dragItemSlot.Item = draggedItem; 
        _itemSlot.Amount = draggedItemAmount;
    }

    #endregion

    public void EquipWeapon(WeaponSO item)
    {
        if (inventory.RemoveItem(item))
        {
            WeaponSO previousItem;
            if (weaponPanel.AddItem(item,out previousItem))
            {
                if(previousItem != null)
                {
                    inventory.AddItem(previousItem);
                    previousItem.Unequip(this,weaponPanel);
                }
                item.Equip(this, weaponPanel);
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }

    public void UnequipWeapon(WeaponSO item)
    {
        if(inventory.CanAddItem(item))
        {
            if ( weaponPanel.RemoveItem(item))
            {
                item.Unequip(this,weaponPanel); // Remove Weapon item Stat
                inventory.AddItem(item);
            }
        }
    }


    #region send Item To Another Inventory

    private ItemContainer openItemContainer;

    private void TransferToItemContainer(BaseItemSlot itemSlot)
    {
        ItemSO item = itemSlot.Item;
        if(item!=null && openItemContainer.CanAddItem(item))
        {
            inventory.RemoveItem(item);
            openItemContainer.AddItem(item);
        }
    }
    private void TransferToInventory(BaseItemSlot itemSlot)
    {
        ItemSO item = itemSlot.Item;
        if(item!=null && inventory.CanAddItem(item))
        {
            openItemContainer.RemoveItem(item);
            inventory.AddItem(item);
        }
    }

    public void OpenItemContainer(ItemContainer itemContainer)
    {
        openItemContainer = itemContainer;

        inventory.OnRightClickEvent -= InvRightClick;
        inventory.OnRightClickEvent += TransferToItemContainer;

        itemContainer.OnRightClickEvent += TransferToInventory;
        itemContainer.OnBeginDragEvent += BeginDrag;
        itemContainer.OnEndDragEvent += EndDrag;
        itemContainer.OnDragEvent += Drag;
        itemContainer.OnDropEvent += Drop;
    }

    public void CloseItemContainer(ItemContainer itemContainer)
    {
        openItemContainer = null;

        inventory.OnRightClickEvent += InvRightClick;
        inventory.OnRightClickEvent -= TransferToItemContainer;

        itemContainer.OnRightClickEvent -= TransferToInventory;
        itemContainer.OnBeginDragEvent -= BeginDrag;
        itemContainer.OnEndDragEvent -= EndDrag;
        itemContainer.OnDragEvent -= Drag;
        itemContainer.OnDropEvent -= Drop;
    }
    #endregion
}
