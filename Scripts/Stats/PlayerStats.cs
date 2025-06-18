using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;


    protected override void Start()
    {
        base.Start();
        //��һ�������ֵ
        Lucky.AddModifier(1);
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();

        player.Die();
    }
}
