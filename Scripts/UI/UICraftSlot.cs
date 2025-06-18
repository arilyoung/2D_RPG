using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//�ϳɣ����죩��
public class UICraftSlot : UIItemSlot
{
    private void OnEnable()
    {
        UpdateSlots(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null)
            return;

        ItemEquipmentData craftData = item.data as ItemEquipmentData;

        Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);
    }
}
