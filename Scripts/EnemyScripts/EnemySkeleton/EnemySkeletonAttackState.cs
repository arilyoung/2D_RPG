//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemySkeletonAttackState : EnemyState
//{
//    private EnemyState enemy;
//    public EnemySkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyState _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
//    {
//        this.enemy = _enemy;
//    }

//    public override void Enter()
//    {
//        base.Enter();
//    }

//    public override void Exit()
//    {
//        base.Exit();

//        enemy.lastTimeAttacked = Time.time;
//    }

//    protected override void Update()
//    {
//        base.Update();

//        enemy.SetZeroVelocity();
//        //��������
//        if (tirggerCalled)
//            stateMachine.ChangeState(enemy.battleState);
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAttackState : EnemyState
{
    private EnemySkeleton enemy;
    public EnemySkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;//��¼���������ʱ���Ա����ж��Ƿ��ܹ����빥��״̬
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();//ʹ�ٶ�Ϊ0

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}