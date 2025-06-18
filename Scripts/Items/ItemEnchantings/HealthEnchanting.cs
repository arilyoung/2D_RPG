using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Heal Enchanting", menuName = "Data/Item Enchanting/Heal Enchanting")]

public class HealthEnchanting : ItemEnchanting
{
    [SerializeField] private float leechValue;

    public override void ExecuteEnchanting(Transform _enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        int playerDamage = playerStats.finalDamage;

        float healAmount = playerDamage * leechValue;

        playerStats.IncreaseHealthBy((int)healAmount);
    }
}
