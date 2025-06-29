using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirJumpState : PlayerAirState
{
    public PlayerAirJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.airJumpLeft = player.airJumpMax ;
        player.airJumpLeft--;
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space) && player.airJumpLeft != 0)
            rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);

        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);

        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * xInput, rb.velocity.y);

        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState);

        if(player.airJumpLeft == 0)
        {
            stateMachine.ChangeState(player.airState);
        }    
    }
}
