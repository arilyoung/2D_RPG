using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    //�ڳ�ʼ�����غͽű�����״̬�ı�ʱ��OnValidate�ᱻ���ã�������GameObject�Ļ��Ա仯��������
    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Obejct - " +itemData.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            //��ӵ��ߵ�����
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
