using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFall : PlayerState
{
    public PlayerFall(PlayerManager _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        
        stateMachine.ChangeState(player.idleState);
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        //player.anim.applyRootMotion = true;
    }

    public override void Exit()
    {
        base.Exit();
        
        //player.anim.applyRootMotion = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.playerLocamotion.isGrounded)
        {
            player.anim.SetBool("jump", false);
            player.anim.SetBool(animBoolName, false);

            if (!player.jumpState.canDoubleJump)
            {
                player.jumpState.canDoubleJump = true;
            }
        }
        else if (input.jump && player.jumpState.canDoubleJump)
        {
            player.jumpState.canDoubleJump = false;
            stateMachine.ChangeState(player.jumpState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
