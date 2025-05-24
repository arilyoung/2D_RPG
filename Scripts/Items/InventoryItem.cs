using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackCount;
    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        //设置默认的堆叠数量为1
        AddToStack();
    }

    public void AddToStack() => stackCount++;
    public void RemoveFromStack() => stackCount--;
}
