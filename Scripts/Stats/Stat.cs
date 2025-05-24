using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//封装 并 存储数据
[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue;

    public List<int> modifiers;
    public int GetValue()
    {
        int finalValue = baseValue;
        
        foreach(int modifier in modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;
    }
    //设置默认初始值
    public void SetDefultValue(int _value)
    {
        baseValue = _value;
    }
    //添加修改值
    public  void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }
    //移除修改值
    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
    }
}
