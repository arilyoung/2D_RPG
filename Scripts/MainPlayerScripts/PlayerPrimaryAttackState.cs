using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter { get; private set; }

    private float lastTimeAttacked;
    private float comboWindow = 2f;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0;
        
        player.StartCoroutine("BusyFor", .3f);

        //连击
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow) 
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);

        //攻击方向
        float attackDir = player.facingDir;
        if (xInput != 0)
            attackDir = xInput;

        //攻击时小位移
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir,
                           player.attackMovement[comboCounter].y);

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetZeroVelocity();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
