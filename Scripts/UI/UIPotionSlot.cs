using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPotionSlot : UIItemSlot
{
    public PotionType potionType;

    private void OnValidate()
    {
        gameObject.name = "Potion Slot - " + potionType.ToString();
    }

    private void Start()
    {
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            (item.data as ItemPotionData).DrinkPotion();
            Inventory.instance.ConsumePotion(item);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("2");
        }
    }
}
