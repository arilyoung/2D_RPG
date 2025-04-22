using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        //设置攻击范围碰撞器
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            player.attackCheck.position,
            player.attackCheckRadius);
        //攻击检测
        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //得到敌人身上 EnemyStats 脚本
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                //对 ES 脚本 DoDamage
                player.stats.DoDamage(_target);
            }
        }
    }

    private void ThorwSword()
    {
        SkillManager.instance.swordThrowSkill.CreateSword();
    }
}
