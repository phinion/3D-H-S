using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    public PlayerIdle(PlayerManager _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        player.playerLocamotion.rb.velocity = Vector3.zero;

        player.movesManager.ResetAvailableMoves();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(input.movementInput != Vector2.zero)
        {
            stateMachine.ChangeState(player.moveState);
        }else if (player.movesManager.IsMoveAvailable())
        {
            player.movesManager.DoNextMove();
            stateMachine.ChangeState(player.attackState);
        }
        else if (input.defend)
        {
            stateMachine.ChangeState(player.defendState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
