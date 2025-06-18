using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDamageEnchantingController : MonoBehaviour
{
    protected PlayerStats playerStats;

    protected void Start()
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
    }

    //检测是否与敌人碰撞，如果有则造成伤害触发DoMagicDamage
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();

            playerStats.DoMagicDamage(enemyTarget);
        }
    }
}
