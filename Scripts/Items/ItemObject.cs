using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    //在初始化加载和脚本启用状态改变时，OnValidate会被调用，但不因GameObject的活性变化而触发。
    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Obejct - " +itemData.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            //添加道具到背包
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
