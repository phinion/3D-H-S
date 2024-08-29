using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerState
{
    private bool UseRootAnim = false;

    public PlayerMove(PlayerManager _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.ChangeState(player.idleState);
    }

    public override void AnimationComboTrigger()
    {
        base.AnimationComboTrigger();
    }

    public override void OnAnimatorMove()
    {
        if (UseRootAnim)
            player.playerLocamotion.RootAnimMove(player.anim.deltaPosition);
    }

    public override void Enter()
    {
        base.Enter();
        UseRootAnim = false;
        player.movesManager.ResetAvailableMoves();
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
        if (player.movesManager.IsMoveAvailable())
        {
            stateMachine.ChangeState(player.attackState);
            player.movesManager.DoNextMove();
        }
        else if (input.dodge)
        {
            stateMachine.ChangeState(player.dodgeState);
        }
        else if (input.defend)
        {
            stateMachine.ChangeState(player.defendState);
        }
        else if (input.movementInput == Vector2.zero && UseRootAnim == false)
        {
            player.anim.SetBool(animBoolName, false);
            UseRootAnim = true;
        }
        else if (input.movementInput != Vector2.zero && UseRootAnim == true)
        {
            player.anim.SetBool(animBoolName, true);
            UseRootAnim = false;
        }
        else if (input.jump && player.playerLocamotion.isGrounded)
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //if (player.anim.GetBool(animBoolName) == false)
        //    return;

        if (input.run)
        {
            player.playerLocamotion.HandleMovement(player.playerLocamotion.runningSpeed);
            player.playerLocamotion.HandleRotation(true, true);
        }
        else
        {
            player.playerLocamotion.HandleAllMovement(player.playerLocamotion.movementSpeed);
            //Debug.Log("Current Magnitude: " + player.playerLocamotion.rb.velocity.magnitude);
        }

    }


}
