using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerState
{
    public PlayerMove(PlayerManager _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
    }

    public override void Exit()
    {
        base.Exit();

        player.anim.SetBool(animBoolName, false);

        player.playerLocamotion.rb.velocity = Vector3.zero;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //if (!isExitingState)
        //{
        if (input.movementInput == Vector2.zero && player.playerLocamotion.rb.velocity.magnitude < 0.8f)
        {
            stateMachine.ChangeState(player.idleState);
        }
        else if (player.movesManager.IsMoveAvailable())
        {
            stateMachine.ChangeState(player.attackState);
            player.movesManager.DoNextMove();
        }
        else if (input.dodge)
        {
            stateMachine.ChangeState(player.dodgeState);
        }
        //}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.playerLocamotion.HandleAllMovement();

        //Debug.Log("Current Magnitude: " + player.playerLocamotion.rb.velocity.magnitude);
    }
}
