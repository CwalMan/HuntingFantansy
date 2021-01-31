using System;
using UnityEngine;

public class WeaponPanel : MonoBehaviour
{
    [SerializeField] Transform WeaponSlotsPanel;
    [SerializeField] WeaponSlot[] weaponSlots;
    [SerializeField] RangedWeapon[] gunScript;

    public event Action<BaseItemSlot> OnRightClickEvent;

    public event Action<BaseItemSlot> OnPointerEnterEvent;
    public event Action<BaseItemSlot> OnPointerExitEvent;

    public event Action<BaseItemSlot> OnBeginDragEvent;
    public event Action<BaseItemSlot> OnDragEvent;
    public event Action<BaseItemSlot> OnEndDragEvent;
    public event Action<BaseItemSlot> OnDropEvent;


    private void Start()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            // go to Main Canvas Script
            weaponSlots[i].OnRightClickEvent += slot => OnRightClickEvent(slot);
            //weaponSlots[i].OnPointerEnterEvent += slot => OnPointerEnterEvent(slot);
            //weaponSlots[i].OnPointerExitEvent += slot => OnPointerExitEvent(slot);
            weaponSlots[i].OnBeginDragEvent += slot => OnBeginDragEvent(slot);
            weaponSlots[i].OnDragEvent += slot => OnDragEvent(slot);
            weaponSlots[i].OnEndDragEvent += slot => OnEndDragEvent(slot);
            weaponSlots[i].OnDropEvent += slot => OnDropEvent(slot);
        }
    }

    private void OnValidate()
    {
        weaponSlots = WeaponSlotsPanel.GetComponentsInChildren<WeaponSlot>();
    }

    public bool AddItem(WeaponSO item, out WeaponSO previousItem)
    {
        for(int i=0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i].weaponType == item.type)
            {
                previousItem = (WeaponSO)weaponSlots[i].Item;
                weaponSlots[i].Item = item;
                weaponSlots[i].Amount = 1;
                return true;
            }
        }

        previousItem = null;
        return false;
    }
    public bool RemoveItem(WeaponSO item)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {

            if (weaponSlots[i].weaponType == item.type)
            {
                weaponSlots[i].Item = null;
                weaponSlots[i].Amount = 0;
                return true;
            }
        }
        return false;
    }

    public void Equip(WeaponSO item)
    {
        for(int i=0; i < gunScript.Length; i++)
        {
            if(gunScript[i].weaponType == item.type)
            {
                if (gunScript != null)
                {
                    gunScript[i].enabled = true;

                    //gunScript[i].maxAmmo = item.MaxAmmo;
                    gunScript[i].sr.sprite = item.Icon;

                    gunScript[i].RPM = item.RPM;

                    //gunScript[i].resetAmmo();
                }
            }
        }
    }

    public void UnEquip(WeaponSO item)
    {
        for (int i = 0; i < gunScript.Length; i++)
        {
            if (gunScript[i].weaponType == item.type)
            {
                if (gunScript != null)
                {
                    gunScript[i].sr.sprite = null;
                    gunScript[i].enabled = false;
                }
            }
        }
    }
}
