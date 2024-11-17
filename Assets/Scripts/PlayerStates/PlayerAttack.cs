using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : PlayerState
{

    public PlayerAttack(PlayerManager _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        //base.AnimationFinishTrigger();

        isAnimationFinished = true;

        //if movesmanager.canmove
        // move
        // else 
        //return to idle or movestate

        //if (input.attack && combocount < 3)
        //{
        //    combocount++;
        //    stateMachine.ChangeState(player.attackState);
        //}
        //else
        //{
        //if (player.movesManager.IsMoveAvailable())
        //{
        //    stateMachine.ChangeState(this);
        //    player.movesManager.DoNextMove();
        //} else

        //if (input.movementInput == Vector2.zero)
        //{
        //    stateMachine.ChangeState(player.idleState);
        //    player.movesManager.ClearAvailableMoves();
        //}
        //else
        //{

        // if (input.movementInput != Vector2.zero)
        // {
        //     stateMachine.ChangeState(player.moveState);
        // }
        // else
        // {
            player.movesManager.ClearAvailableMoves();
            stateMachine.ChangeState(player.idleState);
        //}


        //}


        //}

    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        player.anim.SetTrigger(animBoolName);
    }



    public override void DoChecks()
    {
        base.DoChecks();

        if (!canCombo)
        {
            player.hitbox.CheckHit();
        }
    }

    public override void OnAnimatorMove()
    {
        player.playerLocamotion.RootAnimMove(player.anim.deltaPosition);
    }

    public override void Enter()
    {
        base.Enter();

        player.objectsHit.Clear();
        Debug.Log("PlayerAttack");

        player.playerLocamotion.DashForward(player.movesManager.CurrentAttack().forwardImpulse);
    }

    public override void Exit()
    {
        base.Exit();


        player.anim.ResetTrigger(animBoolName);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (canCombo)
        {
            if (player.movesManager.IsMoveAvailable())
            {
                player.movesManager.DoNextMove();
                stateMachine.ChangeState(this);
            }
            else if (player.movesManager.CurrentAttackMaintainsMomentum() && input.movementInput != Vector2.zero)
            {
                player.movesManager.ClearAvailableMoves();
                anim.CrossFade("Movement", 0.2f);
                stateMachine.ChangeState(player.moveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
