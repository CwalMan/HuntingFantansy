using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class InvPanel :ItemContainer 
{
    [FormerlySerializedAs("Items")]
    [SerializeField] protected ItemSO[] startingItems;
    [SerializeField] protected Transform itemsParent;

    protected override void Awake()
    {
        base.Awake();
        SetStartingItems();
    }

    protected override void OnValidate()
    {
        if(itemsParent != null)
        {
            itemsParent.GetComponentsInChildren(includeInactive: true, result: itemSlots);
        }
        SetStartingItems();
    }

    void SetStartingItems()
    {
        Clear();

        foreach(ItemSO item in startingItems)
        {
            AddItem(item.GetCopy());
        }
    }
}
