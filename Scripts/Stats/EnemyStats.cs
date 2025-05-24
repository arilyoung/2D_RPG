using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;

    [Header("Enemy Level")]
    [SerializeField] private int level = 1;
    [SerializeField] private float percentageModifier = .25f;

    protected override void Start()
    {
        ApplyLevelModifer();

        base.Start();

        enemy = GetComponent<Enemy>();
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
        Debug.Log(level + " modifier: " + modifier);
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
    }
}
