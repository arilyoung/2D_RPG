using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{ 
    Weapon,
    Armor,
    Amulet,
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]

public class ItemEquipmentData : ItemData
{
    public EquipmentType equipmentType;

    public ItemEnchanting[] itemEnchantings;

    [Header("Major stats")]
    public int strength; //���� ��ǿ�˺� �ٷֱ�����
    public int agility; //���� ��ǿ������ ������ �ٷֱ�����
    public int intelligence; //���� ��ǿħ���˺� ħ���ֿ� �ٷֱ�����
    public int vitality; //���� ��ǿ����ֵ ��ֵ����

    [Header("Offensive stats")]
    public int damage; //�����˺�
    public int critChance; //������
    public int critPower; //��������

    [Header("Defensive stats")]
    public int maxHealth; //�������ֵ
    public int evasion; //����
    public int armor; //����ֵ
    public int magicResistence; //ħ��ֵ

    [Header("Magic stats")]
    public int fireDamage; //��ϵħ���˺�
    public int iceDamage; //��ϵħ���˺�
    public int lightingDamage; //��ϵħ���˺�

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials; //�ϳɲ���


    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.evasion.AddModifier(evasion);
        playerStats.armor.AddModifier(armor);
        playerStats.magicResistence.AddModifier(magicResistence);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance); 
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.armor.RemoveModifier(armor);
        playerStats.magicResistence.RemoveModifier(magicResistence);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }

    //ִ�и�ħ�б�
    public void ItemEnchanting(Transform _enemyPosition)
    {
        foreach (var item in itemEnchantings)
        {
            item.ExecuteEnchanting(_enemyPosition);
        }
    }
}
