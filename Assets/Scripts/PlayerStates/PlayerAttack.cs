using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : PlayerState
{
    int combocount = 1;

    public PlayerAttack(PlayerManager _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        Debug.Log("Set animation trigger false");

        Debug.Log("input attack:" + input.attack);

        if (input.attack && combocount < 3)
        {
            combocount++;
            stateMachine.ChangeState(player.attackState);
        }
        else
        {
            combocount = 1;

            if (input.movementInput == Vector2.zero)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.moveState);
            }


        }


    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        Debug.Log("Set animation trigger true");
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
