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
        //���ù�����Χ��ײ��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            player.attackCheck.position,
            player.attackCheckRadius);
        //�������
        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //�õ��������� EnemyStats �ű�
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                //�� ES �ű� DoDamage
                player.stats.DoDamage(_target);
            }
        }
    }

    private void ThorwSword()
    {
        SkillManager.instance.swordThrowSkill.CreateSword();
    }
}
