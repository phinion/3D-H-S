using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerState
{
    private bool UseRootAnim = false;
    bool isRun = false;
    bool transitionToIdle = false;

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
        transitionToIdle = false;
        player.movesManager.ResetAvailableMoves();
    }

    public override void Exit()
    {
        base.Exit();

        player.anim.SetBool(animBoolName, false);

        //player.playerLocamotion.rb.velocity = Vector3.zero;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //if (!isExitingState)
        //{
        if (player.movesManager.IsMoveAvailable())
        {
            player.movesManager.DoNextMove();
            stateMachine.ChangeState(player.attackState);
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
            transitionToIdle = true;
        }
        else if (input.movementInput != Vector2.zero && UseRootAnim == true)
        {
            player.anim.SetBool(animBoolName, true);
            UseRootAnim = false;
            transitionToIdle = false;
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

        if (input.run || (isRun && transitionToIdle))
        {
            if (!isRun)
            {
                isRun = true;
            }
            
            player.playerLocamotion.HandleMovement(player.playerLocamotion.runningSpeed, !transitionToIdle);
            player.playerLocamotion.HandleRotation(true, true);
        }
        else if (input.walk)
        {
            if (isRun)
            {
                isRun = false;
            }
            
            player.playerLocamotion.HandleAllMovement(player.playerLocamotion.walkingSpeed);
        }
        else
        {
            player.playerLocamotion.HandleAllMovement(player.playerLocamotion.movementSpeed);
            //Debug.Log("Current Magnitude: " + player.playerLocamotion.rb.velocity.magnitude);
        }

    }


}
