using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment,
}

//ScriptableObject �� Unity �ṩ��һ���������ô洢���࣬
//����һ��������������������ݵ�����������
//���ǿ��Խ�������Ϊ�Զ����������Դ�ļ���

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]

public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
}
