using System;
using UnityEngine.EventSystems;
using UnityEngine;

public class DropItemArea : MonoBehaviour, IDropHandler
{
    public event Action OnDropEvent;

    public void OnDrop(PointerEventData eventData)
    {
        if (OnDropEvent != null) OnDropEvent();
    }
}
