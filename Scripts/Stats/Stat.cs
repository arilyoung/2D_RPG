using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��װ �� �洢����
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
    //����Ĭ�ϳ�ʼֵ
    public void SetDefultValue(int _value)
    {
        baseValue = _value;
    }
    //����޸�ֵ
    public  void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }
    //�Ƴ��޸�ֵ
    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
    }
}
