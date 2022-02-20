using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : PlayerState
{
    //Vector3 dodgeDirection;

    public PlayerDodge(PlayerManager _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        if (input.movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(player.idleState);
        }
        else
        {
            stateMachine.ChangeState(player.moveState);
        }

    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        //dodgeDirection = player.inputManager.inp
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
