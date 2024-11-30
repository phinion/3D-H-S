using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerState
{
    public bool canDoubleJump = true;

    public PlayerJump(PlayerManager _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationComboTrigger()
    {
        base.AnimationComboTrigger();

        player.playerLocamotion.Jump();
    }

    public override void AnimationFinishTrigger()
    {
        isAnimationFinished = true;

        stateMachine.ChangeState(player.fallState);
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        if (!canDoubleJump)
        {
            player.anim.SetInteger("comboCount", 1);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
