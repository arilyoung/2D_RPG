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

    //����Ƿ��������ײ�������������˺�����DoMagicDamage
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();

            playerStats.DoMagicDamage(enemyTarget);
        }
    }
}
