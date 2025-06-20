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
//        //结束攻击
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

        enemy.lastTimeAttacked = Time.time;//记录攻击的最后时间以便于判断是否能够进入攻击状态
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();//使速度为0

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}