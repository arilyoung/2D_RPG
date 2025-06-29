using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float floatTime = .4f;
    private bool skillUsed;

    private float defaultGravity;
    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity = player.rb.gravityScale;

        skillUsed = false;
        stateTimer = floatTime;
        rb.gravityScale = 0;

    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = defaultGravity;

        player.fx.MakeTransparent(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.velocity = new Vector2(.4f, 15);

        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -.1f);

            if(!skillUsed)
            {
                if (player.skill.blakcholeSkill.UseSkillTrigger())
                    skillUsed = true;
            }
        }

    }
}
