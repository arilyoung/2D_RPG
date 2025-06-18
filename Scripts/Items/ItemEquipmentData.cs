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
    public int strength; //力量 增强伤害 百分比增加
    public int agility; //敏捷 增强闪避率 暴击率 百分比增加
    public int intelligence; //智力 增强魔法伤害 魔法抵抗 百分比增加
    public int vitality; //体力 增强生命值 数值增加

    [Header("Offensive stats")]
    public int damage; //基础伤害
    public int critChance; //暴击率
    public int critPower; //暴击倍率

    [Header("Defensive stats")]
    public int maxHealth; //最大生命值
    public int evasion; //闪避
    public int armor; //护甲值
    public int magicResistence; //魔抗值

    [Header("Magic stats")]
    public int fireDamage; //火系魔法伤害
    public int iceDamage; //冰系魔法伤害
    public int lightingDamage; //雷系魔法伤害

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials; //合成材料


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

    //执行附魔列表
    public void ItemEnchanting(Transform _enemyPosition)
    {
        foreach (var item in itemEnchantings)
        {
            item.ExecuteEnchanting(_enemyPosition);
        }
    }
}
