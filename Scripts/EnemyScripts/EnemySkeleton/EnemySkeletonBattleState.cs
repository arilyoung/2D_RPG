using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonBattleState : EnemyState
{
    private Transform player;
    private EnemySkeleton enemy;
    private int moveDir;
    public EnemySkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsGroundDetected())
        {
            if (Vector2.Distance(player.transform.position, enemy.transform.position) > .8)
            {
                enemy.SetVelocity(enemy.moveSpeed * moveDir * 1.5f, rb.velocity.y);
            }
            else
                enemy.SetVelocity(enemy.moveSpeed * moveDir * .1f, rb.velocity.y);

            if (enemy.IsPlayerDetected())
            {
                stateTimer = enemy.battleTime;
                if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
                {
                    if (CanAttack())
                        stateMachine.ChangeState(enemy.attackState);
                }
            }
            else
            {
                if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 20)
                    stateMachine.ChangeState(enemy.idleState);
            }

            if (player.position.x > enemy.transform.position.x)
            {
                moveDir = 1;
            }
            else if (player.position.x < enemy.transform.position.x)
            {
                moveDir = -1;
            }
        }
        else
        {
            enemy.Flip();
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
    }

    private bool CanAttack()
    {
        if(Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }

}