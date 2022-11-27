using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : PlayerState
{
    int combocount = 1;

    public PlayerAttack(PlayerManager _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    //activationcheckfunction
    public override void ActivationCheck()
    {

        // is the character in a place where they are able to do an attack

        //if they are then changestate(attack)

        //otherwise ignore

    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        //if movesmanager.canmove
        // move
        // else 
        //return to idle or movestate

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
    }

    public override void DoChecks()
    {
        base.DoChecks();

        player.hitbox.CheckHit();
    }

    public override void Enter()
    {
        base.Enter();

        player.objectsHit.Clear();
        Debug.Log("PlayerAttack");
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
