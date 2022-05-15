using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : PlayerState
{
    Vector3 normalizedMoveDir;

    public PlayerDodge(PlayerManager _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        //if (input.movementInput == Vector2.zero)
        //{
        //    stateMachine.ChangeState(player.idleState);
        //}
        //else
        //{
            stateMachine.ChangeState(player.moveState);
        //}

    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        //inputDir = new Vector2(player.inputManager.horizontalInput, player.inputManager.verticalInput);
        normalizedMoveDir = player.playerLocamotion.GetNormalizedMoveDirection();
    }

    public override void Exit()
    {
        base.Exit();

        normalizedMoveDir = Vector3.zero;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.anim.GetBool("combo"))
        {
            if (input.dodge)
            {
                stateMachine.ChangeState(player.dodgeState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!player.anim.GetBool("combo"))
        {
            player.playerLocamotion.Dash(20f, normalizedMoveDir);
        }
        else
        {
            player.playerLocamotion.HandleMovement(normalizedMoveDir);
        }

        player.playerLocamotion.HandleRotation(normalizedMoveDir);
    }
}
