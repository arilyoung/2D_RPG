using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonMoveState : EnemySkeletonGroundState
{
    public EnemySkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())//当撞墙或者没有路的时候翻转
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}