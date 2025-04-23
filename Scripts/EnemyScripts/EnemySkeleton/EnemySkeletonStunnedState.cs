using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonStunnedState : EnemyState
{
    private EnemySkeleton enemy;
    public EnemySkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemy.stunDuration;//stunned����ʱ��

        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);//stunned�ı����ٶȣ�����SetVelocity��FlipCheck�����������rb.velocity�����ٶ�
    }

    public override void Exit()
    {
        base.Exit();

        enemy.fx.Invoke("CancelColorChange", 0); //�� time ������ methodName ������
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}