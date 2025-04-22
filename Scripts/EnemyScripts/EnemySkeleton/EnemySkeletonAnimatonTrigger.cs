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
//        //���õ��˹�����Χ��ײ��
//        Collider2D[] colliders = Physics2D.OverlapCircleAll(
//            enemy.attackCheck.position,
//            enemy.attackCheckRadius);

//        //���˹������
//        foreach (var hit in colliders)
//        {
//            if (hit.GetComponent<Player>() != null)
//            {
//                //�õ�������� PlayerStats �ű�
//                PlayerStats target = hit.GetComponent<PlayerStats>();
//                //DoDamage���� Ϊ PlayerStats �ű�
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
    private EnemySkeleton enemy => GetComponentInParent<EnemySkeleton>();//�õ�enemyʵ��
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();//����ʵ���ϵĺ�����ʹtriggerCalledΪtrue��
    }

    private void AttackTrigger()
    {
        //���õ��˹�����Χ��ײ��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            enemy.attackCheck.position,
            enemy.attackCheckRadius);

        //���˹������
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                //�õ�������� PlayerStats �ű�
                PlayerStats target = hit.GetComponent<PlayerStats>();
                //DoDamage���� Ϊ PlayerStats �ű�
                enemy.stats.DoDamage(target);
            }
        }
    }

    protected void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    protected void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
