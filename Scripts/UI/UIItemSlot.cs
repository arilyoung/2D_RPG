using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIItemSlot : MonoBehaviour , IPointerDownHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;

    public void UpdateSlots(InventoryItem _item)
    {
        item = _item;

        itemImage.color = Color.white;

        //将物品图片和数量显示在UI上
        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackCount > 1)
            {
                itemText.text = item.stackCount.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void ClearUISlot()
    {
        item = null; //如果物品为空，则清空UI

        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    //鼠标点击事件 (接口)
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null)
            return;

        if(Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        if (item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquiptItem(item.data);
    }
}