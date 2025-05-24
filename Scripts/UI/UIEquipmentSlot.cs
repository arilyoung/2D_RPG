using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEquipmentSlot : UIItemSlot
{
    public EquipmentType equipmentType;

    private void OnValidate()
    {
        gameObject.name = "Equipment Slot - " + equipmentType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;
        Inventory.instance.UnEquipItem(item.data as ItemEquipmentData);
        Inventory.instance.AddItem(item.data as ItemEquipmentData);
        ClearUISlot();
    }
}
