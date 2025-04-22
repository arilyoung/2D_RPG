//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemySkeletonAnimatonTrigger : MonoBehaviour
//{
//    private EnemyState enemy => GetComponentInParent<EnemyState>();
//    private Player player;
//    private void AnimationTrigger()
//    {
//        enemy.AnimationFinishTrigger();
//    }

//    private void AttackTrigger()
//    {
//        //设置敌人攻击范围碰撞器
//        Collider2D[] colliders = Physics2D.OverlapCircleAll(
//            enemy.attackCheck.position,
//            enemy.attackCheckRadius);

//        //敌人攻击检测
//        foreach (var hit in colliders)
//        {
//            if (hit.GetComponent<Player>() != null)
//            {
//                //得到玩家身上 PlayerStats 脚本
//                PlayerStats target = hit.GetComponent<PlayerStats>();
//                //DoDamage对象 为 PlayerStats 脚本
//                enemy.stats.DoDamage(target);
//            }
//        }
//    }

//    protected void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
//    protected void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimationTriggers : MonoBehaviour
{
    private EnemySkeleton enemy => GetComponentInParent<EnemySkeleton>();//拿到enemy实体
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();//调用实体上的函数，使triggerCalled为true；
    }

    private void AttackTrigger()
    {
        //设置敌人攻击范围碰撞器
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            enemy.attackCheck.position,
            enemy.attackCheckRadius);

        //敌人攻击检测
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                //得到玩家身上 PlayerStats 脚本
                PlayerStats target = hit.GetComponent<PlayerStats>();
                //DoDamage对象 为 PlayerStats 脚本
                enemy.stats.DoDamage(target);
            }
        }
    }

    protected void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    protected void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
