using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop enemyDropSystem;

    [Header("Enemy Level")]
    public int level = 1;
    [SerializeField] private float percentageModifier = .25f;




    protected override void Start()
    {
        ApplyLevelModifer();
        base.Start();

        enemy = GetComponent<Enemy>();
        enemyDropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifer()
    {
        EnemyLevelModifier(damage);
        EnemyLevelModifier(maxHealth);
    }

    private void EnemyLevelModifier(Stat _stat)
    {
        float modifier = 0f;
        for (int i = 1; i < level; i++)
        {
            modifier += percentageModifier;
        }
        modifier *= _stat.GetValue();
        _stat.AddModifier(Mathf.RoundToInt(modifier));
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();

        enemyDropSystem.GenerateDrop();
    }
}
