using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment,
}

//ScriptableObject 是 Unity 提供的一个数据配置存储基类，
//它是一个可以用来保存大量数据的数据容器，
//我们可以将它保存为自定义的数据资源文件。

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]

public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
}
